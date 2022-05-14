using AutoMapper;
using KsiazeczkaPttk.API.ViewModels;
using KsiazeczkaPttk.DAL.Interfaces;
using KsiazeczkaPttk.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace KsiazeczkaPttk.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfirmationController : ControllerBase
    {
        private readonly ITripRepository _tripRepository;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public ConfirmationController(ITripRepository tripRepository, IFileService fileService, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _tripRepository = tripRepository;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        [HttpGet("{segmentId}")]
        public async Task<ActionResult> GetSegmentConfirmations([FromRoute] int segmentId)
        {
            var segment = await _tripRepository.GetSegmentTravelById(segmentId);
            if (segment is null)
            {
                return NotFound($"Not found Segment with id {segmentId}");
            }

            return Ok(await _tripRepository.GetSegmentConfirmationForSegment(segment));
        }

        [HttpGet("photo/{fileName}")]
        public ActionResult GetPotwierdzeniePhoto(string fileName)
        {
            var imageStream = _fileService.GetPhoto(fileName, _webHostEnvironment.ContentRootPath);
            
            if (imageStream is null)
            {
                return NotFound();
            }

            return File(imageStream, "image/jpeg");
        }

        [HttpPost("withQR")]
        public async Task<ActionResult> CreateConfirmationForSegmentWithQrCode([FromBody] CreateConfirmationWithQrViewModel model)
        {
            var confirmation = _mapper.Map<Confirmation>(model);

            var result = await _tripRepository.AddConfirmationToSegmentWithOr(confirmation, model.SegmentId);
            return UnWrapResultWithBadRequest(result);
        }

        [HttpPost("withPhoto")]
        public async Task<ActionResult> CreateConfirmationTerenoweForSegmentWithPhoto([FromForm] CreateConfirmationWithImageViewModel model)
        {
            var confirmation = _mapper.Map<Confirmation>(model);

            var result = await _tripRepository.AddConfirmationToSegmentWithPhoto(confirmation, model.SegmentId, model.Image, _webHostEnvironment.ContentRootPath);
            return UnWrapResultWithBadRequest(result);
        }

        [HttpDelete("{confirmationId}")]
        public async Task<ActionResult> DeleteSegmentConfirmation([FromRoute] int confirmationId)
        {
            if (await _tripRepository.DeleteConfirmation(confirmationId, _webHostEnvironment.ContentRootPath))
            {
                return Ok();
            }
            return NotFound();
        }

        private ActionResult UnWrapResultWithBadRequest<T>(Result<T> result)
        {
            if (result.IsSuccesful)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Message);
        }
    }
}
