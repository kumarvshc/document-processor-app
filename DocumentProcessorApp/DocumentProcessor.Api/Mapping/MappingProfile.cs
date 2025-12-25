using AutoMapper;
using DocumentProcessor.Api.DTO.Request;
using DocumentProcessor.Api.DTO.Response;
using DocumentProcessor.Application.DTO.Request;
using DocumentProcessor.Application.DTO.Response;

namespace DocumentProcessor.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Request mappings (API -> Application)
            CreateMap<AddDocumentApiRequest, AddDocumentRequest>();

            // Response
            CreateMap<DocumentResponse, AddDocumentApiResponse>();
            CreateMap<DocumentStatusResponse, DocumentStatusApiResponse>();
        }
    }
}
