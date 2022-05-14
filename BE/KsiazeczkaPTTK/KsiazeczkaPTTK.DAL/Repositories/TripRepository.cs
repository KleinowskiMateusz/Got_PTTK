using KsiazeczkaPttk.DAL.Interfaces;
using KsiazeczkaPttk.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace KsiazeczkaPttk.DAL.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly TouristsBookContext _context;
        private readonly IFileService _fileService;

        public TripRepository(TouristsBookContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<Trip> GetById(int id)
        {
            var trip = await GetBaseWycieczkaIQueryable().FirstOrDefaultAsync(w => w.Id == id);
            PreventCycleReferences(trip);
            return trip;
        }

        public async Task<IEnumerable<Trip>> GetAllTrips()
        {
            var trips = await GetBaseWycieczkaIQueryable().ToListAsync();

            foreach (var trip in trips)
            {
                PreventCycleReferences(trip);
            }

            return trips;
        }

        public async Task<SegmentTravel> GetSegmentTravelById(int id)
        {
            return await _context.SegmentTravels
                .Include(p => p.Segment)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<SegmentConfirmation>> GetSegmentConfirmationForSegment(SegmentTravel segmentTravel)
        {
            if (segmentTravel is null)
            {
                return new List<SegmentConfirmation>();
            }

            return await _context.SegmentConfirmations
                .Include(p => p.Confirmation)
                .ThenInclude(p => p.TerrainPoint)
                .Where(p => p.SegmentTravelId == segmentTravel.Id)
                .ToListAsync();
        }

        public async Task<Result<Trip>> CreateTrip(Trip trip)
        {
            if (!trip.Segments?.Any() ?? true)
            {
                return Result<Trip>.Error("Pusta wycieczka");
            }

            trip.TouristsBook = await _context.TouristsBooks.FirstOrDefaultAsync(u => u.OwnerId == trip.TouristsBookId);

            if (trip.TouristsBook is null)
            {
                return Result<Trip>.Error("Nie znaleziono książeczki");
            }

            trip.Status = Domain.Enums.TripStatus.Planned;
            var odcinki = trip.Segments.OrderBy(o => o.Order).ToList();
            trip.Segments = odcinki;

            var orderResult = await ValidateOdcinkiOrder(odcinki);
            if (!orderResult.Item1)
            {
                return Result<Trip>.Error(orderResult.Item2);
            }

            await _context.Trips.AddAsync(trip);
            if (!(await AssignOdcinkiToWycieczka(trip)))
            {
                return Result<Trip>.Error("Nie znaleziono odcinka wycieczki");
            }
            await _context.SaveChangesAsync();

            PreventCycleReferences(trip);
            return Result<Trip>.Ok(trip);
        }

        public async Task<Result<TerrainPoint>> CreatePrivateTerrainPoint(TerrainPoint point)
        {
            var owner = await _context.TouristsBooks.FirstOrDefaultAsync(k => k.OwnerId == point.TouristsBookOwner);
            if (owner is null)
            {
                return Result<TerrainPoint>.Error("Nie znaleziono właściciela");
            }

            point.TouristsBook = owner;

            if (await _context.TerrainPoints.FirstOrDefaultAsync(p => p.Name == point.Name) != null)
            {
                return Result<TerrainPoint>.Error("Nazwa punktu terenowego nie jest unikalna");
            }

            await _context.TerrainPoints.AddAsync(point);
            await _context.SaveChangesAsync();
            return Result<TerrainPoint>.Ok(point);
        }

        public async Task<Result<Segment>> CreatePrivateSegment(Segment segment)
        {
            var validityResult = await CheckNewOdcinekValidity(segment);
            if (!validityResult.Item1)
            {
                return Result<Segment>.Error(validityResult.Item2);
            }

            segment.Version = 1;

            await _context.Segments.AddAsync(segment);
            await _context.SaveChangesAsync();
            return Result<Segment>.Ok(segment);
        }

        private async Task<(bool, string)> CheckNewOdcinekValidity(Segment segment)
        {
            segment.From = await _context.TerrainPoints.FirstOrDefaultAsync(p => p.Id == segment.FromId);
            if (segment.From is null)
            {
                return (false, "Nie znaleziono punktu początkowego");
            }

            segment.Target = await _context.TerrainPoints.FirstOrDefaultAsync(p => p.Id == segment.TargetId);
            if (segment.Target is null)
            {
                return (false, "Nie znaleziono punktu końcowego");
            }

            segment.MountainRange = await _context.MountainRanges.FirstOrDefaultAsync(p => p.Id == segment.MountainRangeId);
            if (segment.MountainRange is null)
            {
                return (false, "Nie znaleziono pasma górskiego");
            }

            segment.TouristsBook = await _context.TouristsBooks.FirstOrDefaultAsync(k => k.OwnerId == segment.TouristsBookOwner);
            if (segment.TouristsBook is null)
            {
                return (false, "Nie znaleziono właściciela");
            }
            return (true, string.Empty);
        }

        public async Task<Result<Confirmation>> AddConfirmationToSegmentWithOr(Confirmation confirmation, int segmentId)
        {
            var segmentFromDb = await _context.SegmentTravels.FirstOrDefaultAsync(o => o.Id == segmentId);
            var terrainPoint = await _context.TerrainPoints.FirstOrDefaultAsync(p => p.Id == confirmation.TerrainPointId);

            if (segmentFromDb is null)
            {
                return Result<Confirmation>.Error("Nie znaleziono Odcinka");
            }

            if (terrainPoint is null)
            {
                return Result<Confirmation>.Error("Nie znaleziono Punktu terenowego");
            }

            confirmation.Type = Domain.Enums.ConfirmationType.QrCode;

            var potwierdzenieAdministracyjne = await _context.Confirmations
                .FirstOrDefaultAsync(p => p.TerrainPointId == terrainPoint.Id && p.IsAdministration);

            if (potwierdzenieAdministracyjne.Url == confirmation.Url)
            {
                return await AddPotwierdzenieToOdcinek(confirmation, segmentFromDb);
            }

            return Result<Confirmation>.Error("Nieprawidłowa lokalizacja kodu QR");
        }

        public async Task<Result<Confirmation>> AddConfirmationToSegmentWithPhoto(Confirmation confiramtion, int segmentId, IFormFile file, string rootFileName)
        {
            var segmentFromDb = await _context.SegmentTravels.FirstOrDefaultAsync(o => o.Id == segmentId);
            var terrainPoint = await _context.TerrainPoints.FirstOrDefaultAsync(p => p.Id == confiramtion.TerrainPointId);

            if (segmentFromDb is null)
            {
                return Result<Confirmation>.Error("Nie znaleziono Odcinka");
            }
            if (terrainPoint is null)
            {
                return Result<Confirmation>.Error("Nie znaleziono Punktu terenowego");
            }
            if (file is null)
            {
                return Result<Confirmation>.Error("Brak zdjęcia");
            }

            confiramtion.Type = Domain.Enums.ConfirmationType.Image;
            confiramtion.Url = await _fileService.SaveFile(file, rootFileName);

            return await AddPotwierdzenieToOdcinek(confiramtion, segmentFromDb);
        }

        private async Task<Result<Confirmation>> AddPotwierdzenieToOdcinek(Confirmation confirmation, SegmentTravel segmentTravel)
        {
            confirmation.Date = DateTime.Now;
            await _context.Confirmations.AddAsync(confirmation);
            await _context.SaveChangesAsync();
            var potwierdzeniePrzebytego = new SegmentConfirmation()
            {
                ConfirmationId = confirmation.Id,
                Confirmation = confirmation,
                SegmentTravelId = segmentTravel.Id,
                SegmentTravel = segmentTravel
            };
            await _context.SegmentConfirmations.AddAsync(potwierdzeniePrzebytego);
            await _context.SaveChangesAsync();
            return Result<Confirmation>.Ok(confirmation);
        }

        public async Task<bool> DeleteConfirmation(int id, string rootFileName)
        {
            var confirmation = await _context.Confirmations.FirstOrDefaultAsync(p => p.Id == id);
            if (confirmation is null)
            {
                return false;
            }

            await RemoveConnectedPotwierdzeniaOdcinkow(id);

            _context.Confirmations.Remove(confirmation);
            await _context.SaveChangesAsync();

            if (confirmation.Type == Domain.Enums.ConfirmationType.Image)
            {
                _fileService.RemoveFile(confirmation.Url, rootFileName);
            }

            return true;
        }

        private async Task<bool> AssignOdcinkiToWycieczka(Trip trip)
        {
            foreach (var segmentTravel in trip.Segments)
            {
                var segmentFromDb = await _context.Segments.FirstOrDefaultAsync(o => o.Id == segmentTravel.SegmentId);
                if (segmentFromDb is null)
                {
                    return false;
                }

                segmentTravel.Segment = segmentFromDb;
                segmentTravel.Trip = trip;
                segmentTravel.TripId = trip.Id;

                await _context.SegmentTravels.AddAsync(segmentTravel);
            }
            return true;
        }

        private async Task<(bool, string)> ValidateOdcinkiOrder(List<SegmentTravel> segments)
        {
            if (segments.Where((o, index) => (o.Order != index + 1)).Any())
            {
                return (false, "Niepoprawna kolejność odcinków");
            }

            for (int i = 1; i < segments.Count; i++)
            {
                var current = segments[i];
                var previous = segments[i - 1];

                if (previous.Segment is null)
                {
                    previous.Segment = await _context.Segments.FirstOrDefaultAsync(o => o.Id == previous.SegmentId);
                }

                if (current.Segment is null)
                {
                    current.Segment = await _context.Segments.FirstOrDefaultAsync(o => o.Id == current.SegmentId);
                }

                var endOdPrevious = previous.IsBack ? previous.Segment.FromId : previous.Segment.TargetId;
                var startOfCurrent = current.IsBack ? current.Segment.TargetId : current.Segment.FromId;

                if (endOdPrevious != startOfCurrent)
                {
                    return (false, $"Odcinki o kolejności: {i} oraz {i + 1} nie są połączone");
                }
            }
            return (true, string.Empty);
        }

        private async Task RemoveConnectedPotwierdzeniaOdcinkow(int confirmationId)
        {
            var segmentConfirmations = await _context.SegmentConfirmations
                .Where(p => p.ConfirmationId == confirmationId).ToListAsync();

            foreach (var segment in segmentConfirmations)
            {
                _context.SegmentConfirmations.Remove(segment);
            }
        }

        private void PreventCycleReferences(Trip trip)
        {
            foreach (var segment in trip?.Segments ?? Array.Empty<SegmentTravel>())
            {
                segment.Trip = null;
            }
        }

        private IQueryable<Trip> GetBaseWycieczkaIQueryable()
        {
            return _context.Trips
                .Include(w => w.TouristsBook)
                    .ThenInclude(k => k.Owner)
                        .ThenInclude(u => u.UserRole)
                .Include(w => w.Segments)
                    .ThenInclude(o => o.Segment)
                        .ThenInclude(o => o.Target)
                .Include(w => w.Segments)
                    .ThenInclude(o => o.Segment)
                        .ThenInclude(o => o.From)
                .Include(w => w.Segments)
                    .ThenInclude(o => o.Segment)
                        .ThenInclude(o => o.MountainRange);
        }
    }
}
