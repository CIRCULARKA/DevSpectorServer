using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;

namespace InvMan.Desktop.UI.Views.Shared
{
    public partial class HiddenableTextBlock : TemplatedControl
    {
        private string _outputText;

        private bool _isNotHidden;

        public static readonly StyledProperty<bool> IsHiddenProperty =
            AvaloniaProperty.Register<HiddenableTextBlock, bool>(
                nameof(IsHidden), false
            );

        public static readonly DirectProperty<HiddenableTextBlock, bool> IsNotHiddenProperty =
            AvaloniaProperty.RegisterDirect<HiddenableTextBlock, bool>(
                nameof(IsNotHidden), o => o.IsNotHidden
            );

        public static readonly StyledProperty<string> TextProperty =
            AvaloniaProperty.Register<HiddenableTextBlock, string>(
                nameof(Text)
            );

        public static readonly DirectProperty<HiddenableTextBlock, string> OutputTextProperty =
            AvaloniaProperty.RegisterDirect<HiddenableTextBlock, string>(
                nameof(OutputText), o => o.OutputText
            );

        public HiddenableTextBlock()
        {
            TextProperty.Changed.Subscribe(
                (o) => SetOutputText(o.NewValue.Value)
            );

            IsHiddenProperty.Changed.Subscribe(
                (o) => ToggleHiddenMode()
            );

            IsHiddenProperty.Changed.Subscribe(
                (o) => IsNotHidden = !IsHidden
            );
        }

        public string OutputText
        {
            get => _outputText;
            set => SetAndRaise(OutputTextProperty, ref _outputText, value);
        }

        public string Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public bool IsHidden
        {
            get => GetValue(IsHiddenProperty);
            set => SetValue(IsHiddenProperty, value);
        }

        public bool IsNotHidden
        {
            get => _isNotHidden;
            set => SetAndRaise(IsNotHiddenProperty, ref _isNotHidden, value);
        }

        public void SetOutputText(string value)
        {
            Text = value;

            if (IsHidden)
                OutputText = new string('*', Text.Length);
            else
                OutputText = Text;
        }

        public void ToggleHiddenMode()
        {
            if (IsHidden)
                OutputText = Text == null ? "" : new string('*', Text.Length);
            else
                OutputText = Text == null ? "" : Text;
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            var hideButton = e.NameScope.Find<Button>("hideButton");
            var showButton = e.NameScope.Find<Button>("showButton");

            hideButton.Click += (o, info) => {
                IsHidden = true;
            };

            showButton.Click += (o, info) => {
                IsHidden = false;
            };
        }
    }
}
