using AutoMapper;
using KsiazeczkaPttk.DAL.Interfaces;
using KsiazeczkaPttk.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace KsiazeczkaPttk.DAL.Repositories
{
    public class TrasyPubliczneRepository : IPublicTrailsRepository
    {
        private readonly KsiazeczkaContext _context;
        private readonly IMapper _mapper;

        public TrasyPubliczneRepository(KsiazeczkaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MountainGroup>> GetAllGrupyGorskie()
        {
            return await _context.MountainGroups.ToListAsync();
        }

        public async Task<Result<IEnumerable<MountainRange>>> GetAllPasmaGorskieForGrupa(int idGrupy)
        {
            var grupaFromDb = await _context.MountainGroups.FirstOrDefaultAsync(g => g.Id == idGrupy);
            if (grupaFromDb is null)
            {
                return Result<IEnumerable<MountainRange>>.Error("Nie znaleziono grupy górskiej");
            }

            var pasma = await _context.MountainRanges
                .Include(p => p.MountainGroup)
                .Where(p => p.GroupId == idGrupy)
                .ToListAsync();

            return Result<IEnumerable<MountainRange>>.Ok(pasma);
        }

        public async Task<IEnumerable<MountainRange>> GetAllPasmaGorskie()
        {
            return await _context.MountainRanges
                .Include(p => p.MountainGroup)
                .ToListAsync();
        }

        public async Task<Result<IEnumerable<Segment>>> GetAllOdcinkiForPasmo(int idPasma)
        {
            var pasmoFromDb = await _context.MountainRanges.FirstOrDefaultAsync(p => p.Id == idPasma);
            if (pasmoFromDb is null)
            {
                return Result<IEnumerable<Segment>>.Error("Nie znaleziono Pasma Górskiego");
            }

            var odcinki = await GetBaseOdcinekQueryable()
                .Where(o => o.IsActive)
                .Where(p => p.MountainRangeId == idPasma).ToListAsync();

            return Result<IEnumerable<Segment>>.Ok(odcinki);
        }

        public async Task<Result<IEnumerable<NeighboringSegment>>> GetAllOdcinkiForPunktTerenowy(int idPunktuTerenowego)
        {
            var punktFromDb = await _context.TerrainPoints.FirstOrDefaultAsync(p => p.Id == idPunktuTerenowego);
            if (punktFromDb is null)
            {
                return Result<IEnumerable<NeighboringSegment>>.Error("Nie znaleziono Punktu Terenowego");
            }

            var odcinki = await GetBaseOdcinekQueryable()
                .Where(o => o.IsActive)
                .Where(o => o.FromId == idPunktuTerenowego || (o.TargetId == idPunktuTerenowego && o.PointsBack > 0))
                .ToListAsync();

            var result = odcinki.Select(o => {
                    var sasiedni = _mapper.Map<NeighboringSegment>(o);
                    sasiedni.IsBack = o.TargetId == idPunktuTerenowego;
                    return sasiedni;
                });

            return Result<IEnumerable<NeighboringSegment>>.Ok(result);
        }

        public async Task<IEnumerable<TerrainPoint>> GetAllPunktyTerenowe()
        {
            return await _context.TerrainPoints
                .Where(p => p.TouristsBook == null)
                .ToListAsync();
        }


        public async Task<IEnumerable<Segment>> GetAllOdcinkiPubliczne()
        {
            return await GetBaseOdcinekQueryable().Where(o => o.TouristsBook == null && o.IsActive).ToListAsync();
        }

        public async Task<Result<Segment>> GetOdcinekPublicznyById(int odcinekId)
        {
            var odcinek = await GetBaseOdcinekQueryable().FirstOrDefaultAsync(o => o.Id == odcinekId);

            if (odcinek is null || !string.IsNullOrEmpty(odcinek.TouristsBookOwner))
            {
                return Result<Segment>.Error("Nie znaleziono odcinka publicznego");
            }
            return Result<Segment>.Ok(odcinek);
        }

        private IQueryable<Segment> GetBaseOdcinekQueryable()
        {
            return _context.Segments
                .Include(o => o.MountainRange)
                .Include(o => o.Target)
                .Include(o => o.From)
                .Include(o => o.TouristsBook);
        }

        public async Task<Result<Segment>> CreateOdcinekPubliczny(Segment odcinek)
        {
            var validity = await CheckCeatedOdcinekValidity(odcinek);
            if (!validity.Item1)
            {
                return Result<Segment>.Error(validity.Item2);
            }

            odcinek.Version = 1;
            odcinek.TouristsBookOwner = null;
            odcinek.IsActive = true;

            await _context.Segments.AddAsync(odcinek);
            await _context.SaveChangesAsync();
            return Result<Segment>.Ok(odcinek);
        }

        public async Task<Result<Segment>> EditOdcinekPubliczny(int odcinekId, Segment odcinek)
        {
            var odcinekFromDb = await _context.Segments.Include(o => o.TouristsBookOwner)
                                        .FirstOrDefaultAsync(o => o.Id == odcinekId);
            if (odcinekFromDb is null)
            {
                return Result<Segment>.Error("Nie znaleziono odcinka");
            }
            if (odcinekFromDb.TouristsBook != null)
            {
                return Result<Segment>.Error("Nie można modyfikować odcinka prywatnego");
            }

            var validity =await CheckCeatedOdcinekValidity(odcinek);
            if (!validity.Item1)
            {
                return Result<Segment>.Error(validity.Item2);
            }

            odcinekFromDb.IsActive = false;

            odcinek.Version = odcinekFromDb.Version + 1;
            odcinek.TouristsBookOwner = null;
            odcinek.IsActive = true;

            await _context.Segments.AddAsync(odcinek);
            await _context.SaveChangesAsync();
            return Result<Segment>.Ok(odcinek);
        }

        private async Task<(bool, string)> CheckCeatedOdcinekValidity(Segment odcinek)
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
            return (true, string.Empty);
        }

        public async Task<bool> DeleteOdcinekPubliczny(int odcinekId)
        {
            var odcinekFromDb = await _context.Segments.FirstOrDefaultAsync(o => o.Id == odcinekId);
            if (odcinekFromDb is null)
            {
                return false;
            }

            var canRemove = await _context.SegmentTravels.FirstOrDefaultAsync(p => p.SegmentId == odcinekId) is null;
        
            if (canRemove)
            {
                _context.Segments.Remove(odcinekFromDb);
            }
            else
            {
                odcinekFromDb.IsActive = false;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TerrainPoint>> GetAllPuntyTerenowe()
        {
            return await _context.TerrainPoints
                .Include(p => p.TouristsBook)
                .ToListAsync();
        }
    }
}
