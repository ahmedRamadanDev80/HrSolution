using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Tamweely.Application.DTOs;

namespace Tamweely.Application.Validators
{
    public class CreateOrEditEmployeeDtoValidator : AbstractValidator<CreateOrEditEmployeeDto>
    {
        public CreateOrEditEmployeeDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
            RuleFor(x => x.Mobile).NotEmpty().Matches(@"^\d{11}$").WithMessage("Mobile must be 11 digits");
            RuleFor(x => x.DepartmentId).GreaterThan(0);
            RuleFor(x => x.JobTitleId).GreaterThan(0);
        }
    }
}
