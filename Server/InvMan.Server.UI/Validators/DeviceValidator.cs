using FluentValidation;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.UI.Validators
{
    public class DeviceValidator : AbstractValidator<Device>
    {
        public DeviceValidator()
        {
            RuleFor(d => d.TypeID).NotEmpty();
            RuleFor(d => d.LocationID).NotEmpty();

            RuleFor(d => d.InventoryNumber).
                Length(3, 20);
            RuleFor(d => d.NetworkName).Length(3, 40);
        }
    }
}
