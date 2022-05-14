using AutoMapper;
using KsiazeczkaPttk.DAL.Interfaces;
using KsiazeczkaPttk.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace KsiazeczkaPttk.DAL.Repositories
{
    public class PublicTrailsRepository : IPublicTrailsRepository
    {
        private readonly TouristsBookContext _context;
        private readonly IMapper _mapper;

        public PublicTrailsRepository(TouristsBookContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MountainGroup>> GetAllMountainGroups()
        {
            return await _context.MountainGroups.ToListAsync();
        }

        public async Task<Result<IEnumerable<MountainRange>>> GetAllMountainRangesForGroup(int groupId)
        {
            var groupFromDb = await _context.MountainGroups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (groupFromDb is null)
            {
                return Result<IEnumerable<MountainRange>>.Error("Nie znaleziono grupy górskiej");
            }

            var ranges = await _context.MountainRanges
                .Include(p => p.MountainGroup)
                .Where(p => p.GroupId == groupId)
                .ToListAsync();

            return Result<IEnumerable<MountainRange>>.Ok(ranges);
        }

        public async Task<IEnumerable<MountainRange>> GetAllMountainRanges()
        {
            return await _context.MountainRanges
                .Include(p => p.MountainGroup)
                .ToListAsync();
        }

        public async Task<Result<IEnumerable<Segment>>> GetAllSegmentsForMountainRange(int rangeId)
        {
            var rangeFromDb = await _context.MountainRanges.FirstOrDefaultAsync(p => p.Id == rangeId);
            if (rangeFromDb is null)
            {
                return Result<IEnumerable<Segment>>.Error("Nie znaleziono Pasma Górskiego");
            }

            var segments = await GetBaseOdcinekQueryable()
                .Where(o => o.IsActive)
                .Where(p => p.MountainRangeId == rangeId).ToListAsync();

            return Result<IEnumerable<Segment>>.Ok(segments);
        }

        public async Task<Result<IEnumerable<NeighboringSegment>>> GetAllNeighboringSegmentsForTerrainPoint(int terrainPointId)
        {
            var pointFromDb = await _context.TerrainPoints.FirstOrDefaultAsync(p => p.Id == terrainPointId);
            if (pointFromDb is null)
            {
                return Result<IEnumerable<NeighboringSegment>>.Error("Nie znaleziono Punktu Terenowego");
            }

            var segments = await GetBaseOdcinekQueryable()
                .Where(o => o.IsActive)
                .Where(o => o.FromId == terrainPointId || (o.TargetId == terrainPointId && o.PointsBack > 0))
                .ToListAsync();

            var result = segments.Select(o => {
                    var sasiedni = _mapper.Map<NeighboringSegment>(o);
                    sasiedni.IsBack = o.TargetId == terrainPointId;
                    return sasiedni;
                });

            return Result<IEnumerable<NeighboringSegment>>.Ok(result);
        }

        public async Task<IEnumerable<TerrainPoint>> GetAllTerrainPoints()
        {
            return await _context.TerrainPoints
                .Where(p => p.TouristsBook == null)
                .ToListAsync();
        }


        public async Task<IEnumerable<Segment>> GetAllPublicSegments()
        {
            return await GetBaseOdcinekQueryable().Where(o => o.TouristsBook == null && o.IsActive).ToListAsync();
        }

        public async Task<Result<Segment>> GetPublicSegmentById(int segmentId)
        {
            var segment = await GetBaseOdcinekQueryable().FirstOrDefaultAsync(o => o.Id == segmentId);

            if (segment is null || !string.IsNullOrEmpty(segment.TouristsBookOwner))
            {
                return Result<Segment>.Error("Nie znaleziono odcinka publicznego");
            }
            return Result<Segment>.Ok(segment);
        }

        private IQueryable<Segment> GetBaseOdcinekQueryable()
        {
            return _context.Segments
                .Include(o => o.MountainRange)
                .Include(o => o.Target)
                .Include(o => o.From)
                .Include(o => o.TouristsBook);
        }

        public async Task<Result<Segment>> CreatePublicSegment(Segment segment)
        {
            var validity = await CheckCeatedOdcinekValidity(segment);
            if (!validity.Item1)
            {
                return Result<Segment>.Error(validity.Item2);
            }

            segment.Version = 1;
            segment.TouristsBookOwner = null;
            segment.IsActive = true;

            await _context.Segments.AddAsync(segment);
            await _context.SaveChangesAsync();
            return Result<Segment>.Ok(segment);
        }

        public async Task<Result<Segment>> EditPublicSegment(int segmentId, Segment segment)
        {
            var segmentFromDb = await _context.Segments.Include(o => o.TouristsBookOwner)
                                        .FirstOrDefaultAsync(o => o.Id == segmentId);
            if (segmentFromDb is null)
            {
                return Result<Segment>.Error("Nie znaleziono odcinka");
            }
            if (segmentFromDb.TouristsBook != null)
            {
                return Result<Segment>.Error("Nie można modyfikować odcinka prywatnego");
            }

            var validity =await CheckCeatedOdcinekValidity(segment);
            if (!validity.Item1)
            {
                return Result<Segment>.Error(validity.Item2);
            }

            segmentFromDb.IsActive = false;

            segment.Version = segmentFromDb.Version + 1;
            segment.TouristsBookOwner = null;
            segment.IsActive = true;

            await _context.Segments.AddAsync(segment);
            await _context.SaveChangesAsync();
            return Result<Segment>.Ok(segment);
        }

        private async Task<(bool, string)> CheckCeatedOdcinekValidity(Segment segment)
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
            return (true, string.Empty);
        }

        public async Task<bool> DeletePublicSegment(int segmentId)
        {
            var segmentFromDb = await _context.Segments.FirstOrDefaultAsync(o => o.Id == segmentId);
            if (segmentFromDb is null)
            {
                return false;
            }

            var canRemove = await _context.SegmentTravels.FirstOrDefaultAsync(p => p.SegmentId == segmentId) is null;
        
            if (canRemove)
            {
                _context.Segments.Remove(segmentFromDb);
            }
            else
            {
                segmentFromDb.IsActive = false;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TerrainPoint>> GetAllTerrainPointsWithBook()
        {
            return await _context.TerrainPoints
                .Include(p => p.TouristsBook)
                .ToListAsync();
        }
    }
}
