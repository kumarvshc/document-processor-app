using AutoMapper;
using DocumentProcessor.Application.DTO.Response;
using DocumentProcessor.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
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

        [HttpGet("{id}/status")]
        [ProducesResponseType(typeof(DocumentStatusResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDocumentStatus(Guid id, CancellationToken cancellationToken)
        {
            var result = await _documentService.GetDocumentStatusAsync(id, cancellationToken);
            return Ok(result);
        }

    }
}
