using AutoMapper;
using Domain.Entities;
using Dtos.Receipts;

namespace Application.Mappings;

public class ReceiptMapping : Profile {
    public ReceiptMapping() {
        CreateMap<Receipt, ReceiptDto>().ReverseMap();
        CreateMap<Receipt, ReceiptViewDto>().ReverseMap()
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.ToString()));
        CreateMap<Receipt, ReceiptTableItemDto>().ReverseMap()
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.ToString()));
    }
}
