using AutoMapper;
using Domain.Entities;
using Dtos.Receipts;

namespace Application.Mappings;

public class ReceiptItemMapping : Profile {
    public ReceiptItemMapping() {
        CreateMap<ReceiptItem, ReceiptItemViewDto>().ReverseMap();
    }
}
