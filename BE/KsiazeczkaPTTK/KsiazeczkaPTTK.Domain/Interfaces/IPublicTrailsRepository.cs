using KsiazeczkaPttk.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KsiazeczkaPttk.DAL.Interfaces
{
    public interface IPublicTrailsRepository
    {
        Task<IEnumerable<MountainGroup>> GetAllGrupyGorskie();

        Task<Result<IEnumerable<MountainRange>>> GetAllPasmaGorskieForGrupa(int idGrupy);

        Task<IEnumerable<MountainRange>> GetAllPasmaGorskie();

        Task<Result<IEnumerable<Segment>>> GetAllOdcinkiForPasmo(int idPasma);

        Task<Result<IEnumerable<NeighboringSegment>>> GetAllOdcinkiForPunktTerenowy(int idPunktuTerenowego);

        Task<IEnumerable<TerrainPoint>> GetAllPunktyTerenowe();

        Task<IEnumerable<Segment>> GetAllOdcinkiPubliczne();

        Task<Result<Segment>> GetOdcinekPublicznyById(int odcinekId);

        Task<Result<Segment>> CreateOdcinekPubliczny(Segment odcinek);

        Task<Result<Segment>> EditOdcinekPubliczny(int odcinekId, Segment odcinek);

        Task<bool> DeleteOdcinekPubliczny(int odcinekId);

        Task<IEnumerable<TerrainPoint>> GetAllPuntyTerenowe();
    }
}
