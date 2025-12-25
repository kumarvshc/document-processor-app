using DocumentProcessor.Api.DTO.Request;
using FluentValidation;

namespace DocumentProcessor.Api.Validator
{
    public class AddDocumentApiRequestValidator : AbstractValidator<AddDocumentApiRequest>
    {
        public AddDocumentApiRequestValidator()
        {
            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("File name is required.")                
                .MaximumLength(255).WithMessage("File name cannot exceed 255 characters.");               
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(1024).WithMessage("Content cannot exceed 1024 characters.");
            RuleFor(x => x.MaxContentSize)
                .GreaterThan(0).WithMessage("Max content size must be greater than 0.")
                .LessThanOrEqualTo(1024).WithMessage("Max content size cannot exceed 1024.");
        }
    }
}
