using AutoMapper;
using Domain.Entities;
using Dtos.StoredFiles;

namespace Application.Mappings;

public class StoredFileMapping : Profile {
    public StoredFileMapping() {
        CreateMap<StoredFile, StoredFileDto>();
        CreateMap<StoredFile, StoredFileViewDto>();
    }
}
