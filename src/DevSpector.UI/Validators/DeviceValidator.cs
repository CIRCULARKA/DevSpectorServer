using FluentValidation;
using DevSpector.Database.DTO;

namespace DevSpector.UI.Validators
{
    public class DeviceValidator : AbstractValidator<DeviceToAdd>
    {
        public DeviceValidator()
        {
            RuleFor(d => d.InventoryNumber).Length(3, 100);
            RuleFor(d => d.ModelName).Length(2, 100);
            RuleFor(d => d.TypeID).NotEmpty();
            RuleFor(d => d.NetworkName).Length(3, 50);
        }
    }
}
