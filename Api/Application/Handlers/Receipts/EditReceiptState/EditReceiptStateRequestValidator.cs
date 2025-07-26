using Domain.Entities;
using Domain.Enums;
using FluentValidation;

namespace Application.Handlers.Receipts.EditReceiptState;

public class EditReceiptStateRequestValidator : AbstractValidator<EditReceiptStateRequest> {
    public EditReceiptStateRequestValidator() {
        RuleFor(x => x.State)
            .Must(s => Enum.TryParse<EReceiptState>(s, true, out _))
            .WithMessage($"Receipt's processing state must be: {string.Join(", ", Enum.GetNames(typeof(EReceiptState)))}")
            .OverridePropertyName("State");
    }
}
