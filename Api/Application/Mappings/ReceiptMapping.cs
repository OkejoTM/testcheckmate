using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Dtos.Receipts;

namespace Application.Mappings;

public class ReceiptMapping : Profile {
    public ReceiptMapping() {
        CreateMap<Receipt, ReceiptDto>().ReverseMap();

        CreateMap<Receipt, ReceiptViewDto>()
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.ToString()))
            .ForMember(dest => dest.OperationType, opt => opt.MapFrom(src => src.OperationType.ToString()))
            .ForMember(dest => dest.CategoryByStore, opt => opt.MapFrom(src => src.CategoryByStore.ToString()))
            .ForMember(dest => dest.CategoryByPrice, opt => opt.MapFrom(src => src.CategoryByPrice.ToString()));

        CreateMap<Receipt, ReceiptTableItemDto>()
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.ToString()))
            .ForMember(dest => dest.CategoryByStore, opt => opt.MapFrom(src => src.CategoryByStore.ToString()));
    }
}
