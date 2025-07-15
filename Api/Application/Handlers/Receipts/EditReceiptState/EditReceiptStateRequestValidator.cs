using FluentValidation;
using Domain.Entities;

namespace Application.Handlers.Receipts.EditReceiptState;

public class EditReceiptStateRequestValidator : AbstractValidator<EditReceiptStateRequest> {
    public EditReceiptStateRequestValidator() {
        RuleFor(x => x.State)
            .Must(s => Enum.TryParse<ReceiptState>(s, true, out _))
            .WithMessage($"Receipt's processing state must be: {string.Join(", ", Enum.GetNames(typeof(ReceiptState)))}")
            .OverridePropertyName("State");
    }
}
