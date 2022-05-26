using AutoMapper;
using KsiazeczkaPttk.API.ViewModels;
using KsiazeczkaPttk.DAL.Interfaces;
using KsiazeczkaPttk.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace KsiazeczkaPttk.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TripController : Controller
    {
        private readonly ITripRepository _tripRepository;
        private readonly IPublicTrailsRepository _publicTrailsRepository;
        private readonly IMapper _mapper;

        public TripController(ITripRepository tripRepository, IPublicTrailsRepository publicTrailsRepository, IMapper mapper)
        {
            _tripRepository = tripRepository;
            _publicTrailsRepository = publicTrailsRepository;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<ActionResult> GetAllTrails()
        {
            return Ok(await _tripRepository.GetAllTrips());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTrailsById(int id)
        {
            var trail = await _tripRepository.GetById(id);

            if (trail is null)
            {
                return NotFound();
            }

            return Ok(trail);
        }

        [HttpGet("mountainGroups")]
        public async Task<ActionResult> GetAvailableMountainGroups()
        {
            return Ok(await _publicTrailsRepository.GetAllMountainGroups());
        }

        [HttpGet("mountainRanges/{groupId}")]
        public async Task<ActionResult> GetAvailableMountainRanges([FromRoute] int groupId)
        {
            var mountainRanges = await _publicTrailsRepository.GetAllMountainRangesForGroup(groupId);
            
            return UnWrapResultWithNotFound(mountainRanges);
        }

        [HttpGet("mountainRanges")]
        public async Task<ActionResult> GetAvailableMountainRanges()
        {
            return Ok(await _publicTrailsRepository.GetAllMountainRanges());
        }

        [HttpGet("segments/{rangeId}")]
        public async Task<ActionResult> GetAvailableSegmentsForMountainRange([FromRoute] int rangeId)
        {
            var segments = await _publicTrailsRepository.GetAllSegmentsForMountainRange(rangeId);
            
            return UnWrapResultWithNotFound(segments);
        }

        [HttpGet("neighboringSegments/{pointId}")]
        public async Task<ActionResult> GetAvailableSegmentsForTerrainPoint([FromRoute] int pointId)
        {
            var odcinkiResult = await _publicTrailsRepository.GetAllNeighboringSegmentsForTerrainPoint(pointId);

            return UnWrapResultWithNotFound(odcinkiResult);
        }

        [HttpGet("terrainPoints")]
        public async Task<ActionResult> GetAvailableTerrainPoints()
        {
            return Ok(await _publicTrailsRepository.GetAllTerrainPoints());
        }

        [HttpPost("trip")]
        public async Task<ActionResult> CreateTrail([FromBody] CreateTripViewModel model)
        {
            var trail = _mapper.Map<Trip>(model);

            var createdResult = await _tripRepository.CreateTrip(trail);
            return UnWrapResultWithBadRequest(createdResult);
        }

        [HttpPost("terrainPoint")]
        public async Task<ActionResult> CreatePrivateTerrainPoint([FromBody] CreateTerrainPointViewModel viewModel)
        {
            var terrainPoint = _mapper.Map<TerrainPoint>(viewModel);

            var createdResult = await _tripRepository.CreatePrivateTerrainPoint(terrainPoint);
            return UnWrapResultWithBadRequest(createdResult);
        }

        [HttpPost("privateSegment")]
        public async Task<ActionResult> CreatePrivateSegment([FromBody] CreateSegmentViewModel viewModel)
        {
            var segment = _mapper.Map<Segment>(viewModel);

            var createdResult = await _tripRepository.CreatePrivateSegment(segment);
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
