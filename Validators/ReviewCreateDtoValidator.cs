
using FluentValidation;
using Models.DTO;

namespace Validators
{
    public class ReviewCreateDtoValidator : AbstractValidator<ReviewCreateDto>
    {
        public ReviewCreateDtoValidator()
        {
            RuleFor(review => review.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");
        }
    }
}
