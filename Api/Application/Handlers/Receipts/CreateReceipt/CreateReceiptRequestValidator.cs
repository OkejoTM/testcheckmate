using FluentValidation;
using Domain.Entities;

namespace Application.Handlers.Receipts.CreateReceipt;

public class CreateReceiptRequestValidator : AbstractValidator<CreateReceiptRequest> {
    public static readonly int MaxCommentLength = 1024;

    public CreateReceiptRequestValidator() {
        RuleFor(x => x.Dto.Comment.Length)
            .LessThanOrEqualTo(MaxCommentLength)
            .WithMessage($"Receipt's comment length is more than {MaxCommentLength} characters")
            .OverridePropertyName("Comment");
    }
}
