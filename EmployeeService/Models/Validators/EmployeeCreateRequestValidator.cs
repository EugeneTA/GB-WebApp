using EmployeeService.Models.Requests;
using FluentValidation;

namespace EmployeeService.Models.Validators
{
    public class EmployeeCreateRequestValidator : AbstractValidator<EmployeeCreateRequest>
    {
        public EmployeeCreateRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().Length(2,255);
            RuleFor(x => x.Surname).NotEmpty().Length(2, 255);
            RuleFor(x => x.Patronymic).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Salary).NotEmpty().GreaterThan(0);
            RuleFor(x => x.DepartmentId).NotEmpty();
            RuleFor(x => x.EmployeeTypeId).NotEmpty();
        }
    }
}
