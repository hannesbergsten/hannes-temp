using FluentValidation;
using ThreadPilot.Insurance.Api.Models.Requests;

namespace ThreadPilot.Insurance.Api.Endpoints;

public class GetInsurancesByIdRequestValidator : AbstractValidator<GetInsurancesByIdRequest>
{
    public GetInsurancesByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .Length(1, 3).WithMessage("Id must be between 1 and 3 characters")
            .Matches(@"^\d+$").WithMessage("Id must be a number");;
    }
}