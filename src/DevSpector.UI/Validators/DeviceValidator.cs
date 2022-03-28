using FluentValidation;
using DevSpector.Database.DTO;

namespace DevSpector.UI.Validators
{
    public class DeviceValidator : AbstractValidator<DeviceToAdd>
    {
        public DeviceValidator()
        {
            RuleFor(d => d.InventoryNumber).NotEmpty().Length(3, 100);
            RuleFor(d => d.ModelName).Length(2, 100);
            RuleFor(d => d.NetworkName).Length(3, 50);
            RuleFor(d => d.TypeID).NotEmpty();
        }
    }
}
