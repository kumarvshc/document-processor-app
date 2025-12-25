using AutoMapper;
using DocumentProcessor.Api.DTO.Request;
using DocumentProcessor.Application.DTO.Request;
using DocumentProcessor.Application.DTO.Response;
using DocumentProcessor.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace DocumentProcessor.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]    
    public class DocumentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(DocumentResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddDocument([FromBody] AddDocumentApiRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var applicationRequest = _mapper.Map<AddDocumentRequest>(request);

            var result = await _documentService.AddDocumentAsync(applicationRequest, cancellationToken);

            return CreatedAtAction(nameof(GetDocumentStatus), new { id = result.Id }, result);
        }


        [HttpGet("{id}/status")]
        [ProducesResponseType(typeof(DocumentStatusResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDocumentStatus(Guid id, CancellationToken cancellationToken)
        {
            var result = await _documentService.GetDocumentStatusAsync(id, cancellationToken);
            return Ok(result);
        }

    }
}
