using FluentValidation;

namespace Application.Handlers.StoredFiles.CreateStoredFile;

public class CreateStoredFileRequestValidator : AbstractValidator<CreateStoredFileRequest> {
    public static readonly int MaxFileSize = 10 * 1024 * 1024;
    public static readonly string[] AllowedExtensions = {".jpg", ".jpeg", ".png", ".pdf"};

    private bool HasValidExtension(string fileName) =>
        AllowedExtensions.Contains(Path.GetExtension(fileName).ToLower());

    public CreateStoredFileRequestValidator() {
        RuleFor(x => x.File).NotNull().WithMessage("File not found");

        RuleFor(x => x.File.FileName)
            .Must(HasValidExtension)
            .WithMessage($"File extension must be: {string.Join(", ", AllowedExtensions)}")
            .OverridePropertyName("File");

        RuleFor(x => x.File.Length)
            .LessThanOrEqualTo(MaxFileSize)
            .WithMessage("File length is more than 10mb")
            .OverridePropertyName("File");
    }
}
