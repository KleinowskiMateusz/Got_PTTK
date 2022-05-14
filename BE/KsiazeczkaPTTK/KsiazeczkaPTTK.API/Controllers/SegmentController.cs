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
        private readonly IPublicTrailsRepository _trasyPubliczneRepository;
        private readonly IMapper _mapper;

        public SegmentController(IPublicTrailsRepository trasyPubliczneRepository, IMapper mapper)
        {
            _trasyPubliczneRepository = trasyPubliczneRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOdcinekPubliczny()
        {
            return Ok(await _trasyPubliczneRepository.GetAllPublicSegments());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOdcinekPublicznyById(int id)
        {
            var odcinekResult = await _trasyPubliczneRepository.GetPublicSegmentById(id);

            return UnWrapResultWithNotFound(odcinekResult);
        }

        [HttpGet("points")]
        public async Task<ActionResult> GetAllPuntyTerenowe()
        {
            return Ok(await _trasyPubliczneRepository.GetAllTerrainPointsWithBook());
        }

        [HttpPost]
        public async Task<IActionResult> CreateOdcinekPubliczny([FromBody] CreatePublicSegmentViewModel viewModel)
        {
            var odcinek = _mapper.Map<Segment>(viewModel);
            
            var createdResult = await _trasyPubliczneRepository.CreatePublicSegment(odcinek);
            return UnWrapResultWithBadRequest(createdResult);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditOdcinekPubliczny([FromRoute] int id, [FromBody] EditPublicSegmentViewModel viewModel)
        {
            var odcinek = _mapper.Map<Segment>(viewModel);

            var editedResult = await _trasyPubliczneRepository.EditPublicSegment(id, odcinek);
            return UnWrapResultWithBadRequest(editedResult);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOdcinekPubliczny(int id)
        {
            if (await _trasyPubliczneRepository.DeletePublicSegment(id))
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
