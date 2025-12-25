using AutoMapper;
using DocumentProcessor.Api.DTO.Request;
using DocumentProcessor.Application.DTO.Request;

namespace DocumentProcessor.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Request mappings (API -> Application)
            CreateMap<AddDocumentApiRequest, AddDocumentRequest>();
        }
    }
}
