using Avalonia;
using Avalonia.Controls;

namespace InvMan.Desktop.UI.Views.Shared
{
    public partial class GroupBox : UserControl
    {
        public static readonly StyledProperty<Thickness> PerimeterThicknessProperty =
            AvaloniaProperty.Register<GroupBox, Thickness>(
                "BorderThickness",
                coerce: (element, newValue) => ChangeBorderThickness(element, newValue)
            );

        public Thickness PerimeterThickness
        {
            get => GetValue(PerimeterThicknessProperty);
            set
            {
                SetValue(PerimeterThicknessProperty, value);
            }
        }

        private static Thickness ChangeBorderThickness(IAvaloniaObject element, Thickness newValue)
        {
            return default(Thickness);
        }
    }
}
