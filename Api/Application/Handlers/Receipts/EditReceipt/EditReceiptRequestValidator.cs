using FluentValidation;
using Domain.Entities;

namespace Application.Handlers.Receipts.EditReceipt;

public class EditReceiptRequestValidator : AbstractValidator<EditReceiptRequest> {
    public static readonly int MaxCommentLength = 1024;

    public EditReceiptRequestValidator() {
        RuleFor(x => x.Dto.Comment.Length)
            .LessThanOrEqualTo(MaxCommentLength)
            .WithMessage($"Receipt's comment length is more than {MaxCommentLength} characters")
            .OverridePropertyName("Comment");
    }
}
