using FluentValidation;
using Domain.Entities;

namespace Application.Handlers.Receipts.GetAllReceipts;

public class GetAllReceiptsRequestValidator : AbstractValidator<GetAllReceiptsRequest> {
    public static readonly string[] ValidSortFields = { nameof(Receipt.CreatedAt) };

    public GetAllReceiptsRequestValidator() {
        RuleFor(x => x.ItemsPerPage)
            .GreaterThanOrEqualTo(1)
            .WithMessage($"Number of receipts per page must be greater or equal to 1")
            .OverridePropertyName("ItemsPerPage");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage($"Page number must be greater or equal to 1")
            .OverridePropertyName("PageNumber");
    }
}
