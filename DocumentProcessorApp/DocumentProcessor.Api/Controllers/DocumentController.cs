using AutoMapper;
using DocumentProcessor.Api.DTO.Request;
using DocumentProcessor.Api.DTO.Response;
using DocumentProcessor.Application.DTO.Request;
using DocumentProcessor.Application.DTO.Response;
using DocumentProcessor.Application.ServiceInterfaces;
using DocumentProcessor.Common;
using Microsoft.AspNetCore.Mvc;

namespace DocumentProcessor.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]    
    public class DocumentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDocumentService _documentService;

        public DocumentController(IMapper mapper, IDocumentService documentService)
        {
            _mapper = mapper;
            _documentService = documentService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AddDocumentApiResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddDocument([FromBody] AddDocumentApiRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var applicationRequest = _mapper.Map<AddDocumentRequest>(request);

            var result = await _documentService.AddDocumentAsync(applicationRequest, cancellationToken);

            if (result.IsSuccess && result.Value is not null)
            {
                var responseDto = _mapper.Map<AddDocumentApiResponse>(result.Value);
                return Ok(responseDto);
            }
            return StatusCode(result.StatusCode, ToProblemResult(result.StatusCode, result.Error, HttpContext));
        }


        [HttpGet("{id}/status")]
        [ProducesResponseType(typeof(DocumentStatusApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDocumentStatus(Guid id, CancellationToken cancellationToken)
        {
            var result = await _documentService.GetDocumentStatusAsync(id, cancellationToken);

            if (result.IsSuccess && result.Value is not null)
            {
                var responseDto = _mapper.Map<DocumentStatusApiResponse>(result.Value);

                return Ok(responseDto);
            }
            return StatusCode(result.StatusCode, ToProblemResult(result.StatusCode, result.Error, HttpContext));           
        }

        private IActionResult ToProblemResult(int statusCode, string? error, HttpContext httpContext)
        {
            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = statusCode switch
                {
                    StatusCodes.Status422UnprocessableEntity => "Business Rule Violation",
                    StatusCodes.Status500InternalServerError => "Internal Server Error",
                    _ => "Error"
                },
                Detail = error,
                Instance = httpContext?.Request?.Path
            };

            return new ObjectResult(problem) { StatusCode = statusCode };
        }

    }
}
