using AutoMapper;
using DocumentProcessor.Application.DTO.Request;

namespace DocumentProcessor.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Request mappings (API -> Application)
            //CreateMap<DocumentApiRequest, AddDocumentRequest>();
        }
    }
}
