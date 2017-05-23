using FluentValidation;
using com.ebaocloud.client.thai.seg.vmi.parameters;

namespace com.ebaocloud.client.thai.seg.vmi.validator
{
    public class CalculationValidator : AbstractValidator<CalculationParams>
    {
        public CalculationValidator()
        {
            RuleFor(c => c.vehicleMakeName).NotNull().NotEqual("");
            RuleFor(c => c.vehicleModelDescription).NotNull();
            RuleFor(c => c.vehicleModelName).NotNull();
            RuleFor(c => c.vehicleModelYear).NotNull();
            RuleFor(c => c.planCode).NotNull();
            RuleFor(c => c.effectiveDate).NotNull();
            RuleFor(c => c.expireDate).NotNull();
            RuleFor(c => c.productCode).NotNull();
            RuleFor(c => c.vehicleUsage).NotNull();
            RuleFor(c => c.vehicleTotalValue).NotNull();
            RuleFor(c => c.vehicleRegistrationYear).NotNull();
            RuleFor(c => c.vehicleAccessaryValue).NotNull();
            RuleFor(c => c.vehicleGarageType).NotNull();
        }
    }
}
