using KsiazeczkaPttk.DAL.Interfaces;
using KsiazeczkaPttk.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace KsiazeczkaPttk.DAL.Repositories
{
    public class WycieczkaRepository : ITripRepository
    {
        private readonly KsiazeczkaContext _context;
        private readonly IFileService _fileService;

        public WycieczkaRepository(KsiazeczkaContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<Trip> GetById(int id)
        {
            var wycieczka = await GetBaseWycieczkaIQueryable().FirstOrDefaultAsync(w => w.Id == id);
            PreventCycleReferences(wycieczka);
            return wycieczka;
        }

        public async Task<IEnumerable<Trip>> GetAllWycieczka()
        {
            var wycieczki = await GetBaseWycieczkaIQueryable().ToListAsync();

            foreach (var wycieczka in wycieczki)
            {
                PreventCycleReferences(wycieczka);
            }

            return wycieczki;
        }

        public async Task<SegmentTravel> GetPrzebytyOdcinekById(int id)
        {
            return await _context.SegmentTravels
                .Include(p => p.Segment)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<SegmentConfirmation>> GetPotwierdzeniaForOdcinek(SegmentTravel odcinek)
        {
            if (odcinek is null)
            {
                return new List<SegmentConfirmation>();
            }

            return await _context.SegmentConfirmations
                .Include(p => p.Confirmation)
                .ThenInclude(p => p.TerrainPoint)
                .Where(p => p.SegmentTravelId == odcinek.Id)
                .ToListAsync();
        }

        public async Task<Result<Trip>> CreateWycieczka(Trip wycieczka)
        {
            if (!wycieczka.Segments?.Any() ?? true)
            {
                return Result<Trip>.Error("Pusta wycieczka");
            }

            wycieczka.TouristsBook = await _context.TouristsBooks.FirstOrDefaultAsync(u => u.OwnerId == wycieczka.TouristsBookId);

            if (wycieczka.TouristsBook is null)
            {
                return Result<Trip>.Error("Nie znaleziono książeczki");
            }

            wycieczka.Status = Domain.Enums.TripStatus.Planned;
            var odcinki = wycieczka.Segments.OrderBy(o => o.Order).ToList();
            wycieczka.Segments = odcinki;

            var orderResult = await ValidateOdcinkiOrder(odcinki);
            if (!orderResult.Item1)
            {
                return Result<Trip>.Error(orderResult.Item2);
            }

            await _context.Trips.AddAsync(wycieczka);
            if (!(await AssignOdcinkiToWycieczka(wycieczka)))
            {
                return Result<Trip>.Error("Nie znaleziono odcinka wycieczki");
            }
            await _context.SaveChangesAsync();

            PreventCycleReferences(wycieczka);
            return Result<Trip>.Ok(wycieczka);
        }

        public async Task<Result<TerrainPoint>> CreatePunktPrywatny(TerrainPoint punkt)
        {
            var wlasciciel = await _context.TouristsBooks.FirstOrDefaultAsync(k => k.OwnerId == punkt.TouristsBookOwner);
            if (wlasciciel is null)
            {
                return Result<TerrainPoint>.Error("Nie znaleziono właściciela");
            }

            punkt.TouristsBook = wlasciciel;

            if (await _context.TerrainPoints.FirstOrDefaultAsync(p => p.Name == punkt.Name) != null)
            {
                return Result<TerrainPoint>.Error("Nazwa punktu terenowego nie jest unikalna");
            }

            await _context.TerrainPoints.AddAsync(punkt);
            await _context.SaveChangesAsync();
            return Result<TerrainPoint>.Ok(punkt);
        }

        public async Task<Result<Segment>> CreateOdcinekPrywatny(Segment odcinek)
        {
            var validityResult = await CheckNewOdcinekValidity(odcinek);
            if (!validityResult.Item1)
            {
                return Result<Segment>.Error(validityResult.Item2);
            }

            odcinek.Version = 1;

            await _context.Segments.AddAsync(odcinek);
            await _context.SaveChangesAsync();
            return Result<Segment>.Ok(odcinek);
        }

        private async Task<(bool, string)> CheckNewOdcinekValidity(Segment odcinek)
        {
            odcinek.From = await _context.TerrainPoints.FirstOrDefaultAsync(p => p.Id == odcinek.FromId);
            if (odcinek.From is null)
            {
                return (false, "Nie znaleziono punktu początkowego");
            }

            odcinek.Target = await _context.TerrainPoints.FirstOrDefaultAsync(p => p.Id == odcinek.TargetId);
            if (odcinek.Target is null)
            {
                return (false, "Nie znaleziono punktu końcowego");
            }

            odcinek.MountainRange = await _context.MountainRanges.FirstOrDefaultAsync(p => p.Id == odcinek.MountainRangeId);
            if (odcinek.MountainRange is null)
            {
                return (false, "Nie znaleziono pasma górskiego");
            }

            odcinek.TouristsBook = await _context.TouristsBooks.FirstOrDefaultAsync(k => k.OwnerId == odcinek.TouristsBookOwner);
            if (odcinek.TouristsBook is null)
            {
                return (false, "Nie znaleziono właściciela");
            }
            return (true, string.Empty);
        }

        public async Task<Result<Confirmation>> AddPotwierdzenieToOdcinekWithOr(Confirmation potwierdzenie, int odcinekId)
        {
            var odcinekFromDb = await _context.SegmentTravels.FirstOrDefaultAsync(o => o.Id == odcinekId);
            var punktTerenowy = await _context.TerrainPoints.FirstOrDefaultAsync(p => p.Id == potwierdzenie.TerrainPointId);

            if (odcinekFromDb is null)
            {
                return Result<Confirmation>.Error("Nie znaleziono Odcinka");
            }

            if (punktTerenowy is null)
            {
                return Result<Confirmation>.Error("Nie znaleziono Punktu terenowego");
            }

            potwierdzenie.Type = Domain.Enums.ConfirmationType.QrCode;

            var potwierdzenieAdministracyjne = await _context.Confirmations
                .FirstOrDefaultAsync(p => p.TerrainPointId == punktTerenowy.Id && p.IsAdministration);

            if (potwierdzenieAdministracyjne.Url == potwierdzenie.Url)
            {
                return await AddPotwierdzenieToOdcinek(potwierdzenie, odcinekFromDb);
            }

            return Result<Confirmation>.Error("Nieprawidłowa lokalizacja kodu QR");
        }

        public async Task<Result<Confirmation>> AddPotwierdzenieToOdcinekWithPhoto(Confirmation potwierdzenie, int odcinekId, IFormFile file, string rootFileName)
        {
            var odcinekFromDb = await _context.SegmentTravels.FirstOrDefaultAsync(o => o.Id == odcinekId);
            var punktTerenowy = await _context.TerrainPoints.FirstOrDefaultAsync(p => p.Id == potwierdzenie.TerrainPointId);

            if (odcinekFromDb is null)
            {
                return Result<Confirmation>.Error("Nie znaleziono Odcinka");
            }
            if (punktTerenowy is null)
            {
                return Result<Confirmation>.Error("Nie znaleziono Punktu terenowego");
            }
            if (file is null)
            {
                return Result<Confirmation>.Error("Brak zdjęcia");
            }

            potwierdzenie.Type = Domain.Enums.ConfirmationType.Image;
            potwierdzenie.Url = await _fileService.SaveFile(file, rootFileName);

            return await AddPotwierdzenieToOdcinek(potwierdzenie, odcinekFromDb);
        }

        private async Task<Result<Confirmation>> AddPotwierdzenieToOdcinek(Confirmation potwierdzenie, SegmentTravel przebycieOdcinka)
        {
            potwierdzenie.Date = DateTime.Now;
            await _context.Confirmations.AddAsync(potwierdzenie);
            await _context.SaveChangesAsync();
            var potwierdzeniePrzebytego = new SegmentConfirmation()
            {
                ConfirmationId = potwierdzenie.Id,
                Confirmation = potwierdzenie,
                SegmentTravelId = przebycieOdcinka.Id,
                SegmentTravel = przebycieOdcinka
            };
            await _context.SegmentConfirmations.AddAsync(potwierdzeniePrzebytego);
            await _context.SaveChangesAsync();
            return Result<Confirmation>.Ok(potwierdzenie);
        }

        public async Task<bool> DeletePotwierdzenia(int id, string rootFileName)
        {
            var potwierdzenie = await _context.Confirmations.FirstOrDefaultAsync(p => p.Id == id);
            if (potwierdzenie is null)
            {
                return false;
            }

            await RemoveConnectedPotwierdzeniaOdcinkow(id);

            _context.Confirmations.Remove(potwierdzenie);
            await _context.SaveChangesAsync();

            if (potwierdzenie.Type == Domain.Enums.ConfirmationType.Image)
            {
                _fileService.RemoveFile(potwierdzenie.Url, rootFileName);
            }

            return true;
        }

        private async Task<bool> AssignOdcinkiToWycieczka(Trip wycieczka)
        {
            foreach (var przebycieOdcinka in wycieczka.Segments)
            {
                var odcinekFromDb = await _context.Segments.FirstOrDefaultAsync(o => o.Id == przebycieOdcinka.SegmentId);
                if (odcinekFromDb is null)
                {
                    return false;
                }

                przebycieOdcinka.Segment = odcinekFromDb;
                przebycieOdcinka.Trip = wycieczka;
                przebycieOdcinka.TripId = wycieczka.Id;

                await _context.SegmentTravels.AddAsync(przebycieOdcinka);
            }
            return true;
        }

        private async Task<(bool, string)> ValidateOdcinkiOrder(List<SegmentTravel> odcinki)
        {
            if (odcinki.Where((o, index) => (o.Order != index + 1)).Any())
            {
                return (false, "Niepoprawna kolejność odcinków");
            }

            for (int i = 1; i < odcinki.Count; i++)
            {
                var current = odcinki[i];
                var previous = odcinki[i - 1];

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

        private async Task RemoveConnectedPotwierdzeniaOdcinkow(int potwierdzenieId)
        {
            var potwierdzeniaOdcinkow = await _context.SegmentConfirmations
                .Where(p => p.ConfirmationId == potwierdzenieId).ToListAsync();

            foreach (var potwierdzenieOdcinka in potwierdzeniaOdcinkow)
            {
                _context.SegmentConfirmations.Remove(potwierdzenieOdcinka);
            }
        }

        private void PreventCycleReferences(Trip wycieczka)
        {
            foreach (var odcinek in wycieczka?.Segments ?? Array.Empty<SegmentTravel>())
            {
                odcinek.Trip = null;
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
