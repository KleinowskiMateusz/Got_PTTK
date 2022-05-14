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
    public class SegmentController : Controller
    {
        private readonly IPublicTrailsRepository _publicTrailsRepository;
        private readonly IMapper _mapper;

        public SegmentController(IPublicTrailsRepository publicTrailsRepository, IMapper mapper)
        {
            _publicTrailsRepository = publicTrailsRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPublicSegments()
        {
            return Ok(await _publicTrailsRepository.GetAllPublicSegments());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPublicSegmentById(int id)
        {
            var segmentResult = await _publicTrailsRepository.GetPublicSegmentById(id);

            return UnWrapResultWithNotFound(segmentResult);
        }

        [HttpGet("points")]
        public async Task<ActionResult> GetAllTerrainPoints()
        {
            return Ok(await _publicTrailsRepository.GetAllTerrainPointsWithBook());
        }

        [HttpPost]
        public async Task<IActionResult> CreatePublicSegment([FromBody] CreatePublicSegmentViewModel viewModel)
        {
            var segment = _mapper.Map<Segment>(viewModel);
            
            var createdResult = await _publicTrailsRepository.CreatePublicSegment(segment);
            return UnWrapResultWithBadRequest(createdResult);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditPublicSegment([FromRoute] int id, [FromBody] EditPublicSegmentViewModel viewModel)
        {
            var segment = _mapper.Map<Segment>(viewModel);

            var editedResult = await _publicTrailsRepository.EditPublicSegment(id, segment);
            return UnWrapResultWithBadRequest(editedResult);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublicSegment(int id)
        {
            if (await _publicTrailsRepository.DeletePublicSegment(id))
            {
                return Ok();
            }

            return BadRequest();
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
