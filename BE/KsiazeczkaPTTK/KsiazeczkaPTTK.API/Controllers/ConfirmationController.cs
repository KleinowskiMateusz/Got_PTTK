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
        private readonly ITripRepository _wycieczkaRepository;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public ConfirmationController(ITripRepository wycieczkaRepository, IFileService fileService, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _wycieczkaRepository = wycieczkaRepository;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        [HttpGet("{segmentId}")]
        public async Task<ActionResult> GetPotwierdzeniaOdcinka([FromRoute] int segmentId)
        {
            var odcinek = await _wycieczkaRepository.GetSegmentTravelById(segmentId);
            if (odcinek is null)
            {
                return NotFound($"Not found Przebyty Odcinek with id {segmentId}");
            }

            return Ok(await _wycieczkaRepository.GetSegmentConfirmationForSegment(odcinek));
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
        public async Task<ActionResult> CreatePotwierdzenieTerenoweForOdcinekWithQrCode([FromBody] CreatePotwierdzenieWithQrViewModel modelPotwierdzenia)
        {
            var potwierdzenie = _mapper.Map<Confirmation>(modelPotwierdzenia);

            var result = await _wycieczkaRepository.AddConfirmationToSegmentWithOr(potwierdzenie, modelPotwierdzenia.OdcinekId);
            return UnWrapResultWithBadRequest(result);
        }

        [HttpPost("withPhoto")]
        public async Task<ActionResult> CreatePotwierdzenieTerenoweForOdcinekWithPhoto([FromForm] CreatePotwierdzenieWithImageViewModel modelPotwierdzenia)
        {
            var potwierdzenie = _mapper.Map<Confirmation>(modelPotwierdzenia);

            var result = await _wycieczkaRepository.AddConfirmationToSegmentWithPhoto(potwierdzenie, modelPotwierdzenia.OdcinekId, modelPotwierdzenia.Image, _webHostEnvironment.ContentRootPath);
            return UnWrapResultWithBadRequest(result);
        }

        [HttpDelete("{confirmationId}")]
        public async Task<ActionResult> DeletePotwierdzenieOdcinka([FromRoute] int idPotwierdzenia)
        {
            if (await _wycieczkaRepository.DeleteConfirmation(idPotwierdzenia, _webHostEnvironment.ContentRootPath))
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
