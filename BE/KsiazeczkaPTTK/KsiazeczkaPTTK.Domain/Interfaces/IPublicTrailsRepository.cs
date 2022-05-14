using KsiazeczkaPttk.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KsiazeczkaPttk.DAL.Interfaces
{
    public interface IPublicTrailsRepository
    {
        Task<IEnumerable<MountainGroup>> GetAllMountainGroups();

        Task<Result<IEnumerable<MountainRange>>> GetAllMountainRangesForGroup(int groupId);

        Task<IEnumerable<MountainRange>> GetAllMountainRanges();

        Task<Result<IEnumerable<Segment>>> GetAllSegmentsForMountainRange(int rangeId);

        Task<Result<IEnumerable<NeighboringSegment>>> GetAllNeighboringSegmentsForTerrainPoint(int pointId);

        Task<IEnumerable<TerrainPoint>> GetAllTerrainPoints();

        Task<IEnumerable<Segment>> GetAllPublicSegments();

        Task<Result<Segment>> GetPublicSegmentById(int segmentId);

        Task<Result<Segment>> CreatePublicSegment(Segment segment);

        Task<Result<Segment>> EditPublicSegment(int segmentId, Segment segment);

        Task<bool> DeletePublicSegment(int odcinekId);

        Task<IEnumerable<TerrainPoint>> GetAllTerrainPointsWithBook();
    }
}
