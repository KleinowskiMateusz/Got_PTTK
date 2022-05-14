using KsiazeczkaPttk.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace KsiazeczkaPttk.DAL.Interfaces
{
    public interface ITripRepository
    {
        Task<Trip> GetById(int id);

        Task<IEnumerable<Trip>> GetAllWycieczka();

        Task<SegmentTravel> GetPrzebytyOdcinekById(int id);

        Task<IEnumerable<SegmentConfirmation>> GetPotwierdzeniaForOdcinek(SegmentTravel odcinek);

        Task<Result<Trip>> CreateWycieczka(Trip wycieczka);

        Task<Result<Segment>> CreateOdcinekPrywatny(Segment odcinek);

        Task<Result<TerrainPoint>> CreatePunktPrywatny(TerrainPoint punkt);

        Task<Result<Confirmation>> AddPotwierdzenieToOdcinekWithOr(Confirmation potwierdzenie, int odcinekId);

        Task<Result<Confirmation>> AddPotwierdzenieToOdcinekWithPhoto(Confirmation potwierdzenie, int odcinekId, IFormFile file, string rootFileName);

        Task<bool> DeletePotwierdzenia(int id, string rootFileName);
    }
}
