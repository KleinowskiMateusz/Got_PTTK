using AutoMapper;
using KsiazeczkaPttk.API.ViewModels;
using KsiazeczkaPttk.DAL.Interfaces;
using KsiazeczkaPttk.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KsiazeczkaPttk.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TripController : Controller
    {
        private readonly ITripRepository _wycieczkaRepository;
        private readonly IPublicTrailsRepository _trasyPubliczneRepository;
        private readonly IMapper _mapper;

        public TripController(ITripRepository wycieczkaRepository, IPublicTrailsRepository trasyPubliczneRepository, IMapper mapper)
        {
            _wycieczkaRepository = wycieczkaRepository;
            _trasyPubliczneRepository = trasyPubliczneRepository;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<ActionResult> GetAllWycieczka()
        {
            return Ok(await _wycieczkaRepository.GetAllTrips());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetWycieczka(int id)
        {
            var wycieczka = await _wycieczkaRepository.GetById(id);

            if (wycieczka is null)
            {
                return NotFound();
            }

            return Ok(wycieczka);
        }

        [HttpGet("mountainGroups")]
        public async Task<ActionResult> GetAvailableGrupyGorskie()
        {
            return Ok(await _trasyPubliczneRepository.GetAllMountainGroups());
        }

        [HttpGet("mountainRanges/{groupId}")]
        public async Task<ActionResult> GetAvailablePasmaGorskie([FromRoute] int groupId)
        {
            var pasmaResult = await _trasyPubliczneRepository.GetAllMountainRangesForGroup(groupId);
            
            return UnWrapResultWithNotFound(pasmaResult);
        }

        [HttpGet("mountainRanges")]
        public async Task<ActionResult> GetAvailablePasmaGorskie()
        {
            return Ok(await _trasyPubliczneRepository.GetAllMountainRanges());
        }

        [HttpGet("segmants/{pasmoId}")]
        public async Task<ActionResult> GetAvailableOdcinkiForPasmo([FromRoute] int pasmoId)
        {
            var odcinkiResult = await _trasyPubliczneRepository.GetAllSegmentsForMountainRange(pasmoId);
            
            return UnWrapResultWithNotFound(odcinkiResult);
        }

        [HttpGet("neighboringSegments/{pointId}")]
        public async Task<ActionResult> GetAvailableOdcinkiForPunktTerenowy([FromRoute] int pointId)
        {
            var odcinkiResult = await _trasyPubliczneRepository.GetAllNeighboringSegmentsForTerrainPoint(pointId);

            return UnWrapResultWithNotFound(odcinkiResult);
        }

        [HttpGet("terrainPoints")]
        public async Task<ActionResult> GetAvailablePunktyTerenowe()
        {
            return Ok(await _trasyPubliczneRepository.GetAllTerrainPoints());
        }

        [HttpPost("trip")]
        public async Task<ActionResult> CreateWycieczka([FromBody] CreateTripViewModel model)
        {
            var wycieczka = _mapper.Map<Trip>(model);

            var createdResult = await _wycieczkaRepository.CreateTrip(wycieczka);
            return UnWrapResultWithBadRequest(createdResult);
        }

        [HttpPost("terrainPoint")]
        public async Task<ActionResult> CreatePunktPrywatny([FromBody] CreateTerrainPointViewModel viewModel)
        {
            var punktTerenowy = _mapper.Map<TerrainPoint>(viewModel);

            var createdResult = await _wycieczkaRepository.CreatePrivateTerrainPoint(punktTerenowy);
            return UnWrapResultWithBadRequest(createdResult);
        }

        [HttpPost("privateSegment")]
        public async Task<ActionResult> CreateOdcinekPrywatny([FromBody] CreateSegmentViewModel viewModel)
        {
            var odcinek = _mapper.Map<Segment>(viewModel);

            var createdResult = await _wycieczkaRepository.CreatePrivateSegment(odcinek);
            return UnWrapResultWithBadRequest(createdResult);
        }

        private ActionResult UnWrapResultWithBadRequest<T>(Result<T> result)
        {
            if (result.IsSuccesful)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Message);
        }

        private ActionResult UnWrapResultWithNotFound<T>(Result<T> result)
        {
            if (result.IsSuccesful)
            {
                return Ok(result.Value);
            }
            return NotFound(result.Message);
        }
    }
}
