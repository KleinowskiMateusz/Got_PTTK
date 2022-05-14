using KsiazeczkaPttk.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace KsiazeczkaPttk.DAL.Interfaces
{
    public interface ITripRepository
    {
        Task<Trip> GetById(int id);

        Task<IEnumerable<Trip>> GetAllTrips();

        Task<SegmentTravel> GetSegmentTravelById(int id);

        Task<IEnumerable<SegmentConfirmation>> GetSegmentConfirmationForSegment(SegmentTravel segmentTravel);

        Task<Result<Trip>> CreateTrip(Trip trip);

        Task<Result<Segment>> CreatePrivateSegment(Segment segment);

        Task<Result<TerrainPoint>> CreatePrivateTerrainPoint(TerrainPoint terrainPoint);

        Task<Result<Confirmation>> AddConfirmationToSegmentWithOr(Confirmation confirmation, int segmentId);

        Task<Result<Confirmation>> AddConfirmationToSegmentWithPhoto(Confirmation confirmation, int segmentId, IFormFile file, string rootFileName);

        Task<bool> DeleteConfirmation(int id, string rootFileName);
    }
}
