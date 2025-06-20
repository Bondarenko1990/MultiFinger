using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows;

namespace MultiFinger.Helpers.UiExtensions
{
    public class OneCheckToogleButtonExtension
    {
        public static readonly DependencyProperty IsMouseDownProperty = DependencyProperty.RegisterAttached(
            "IsMouseDown",
            typeof(bool),
            typeof(OneCheckToogleButtonExtension),
            new PropertyMetadata(default(bool), IsActiveChanged));

        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.RegisterAttached(
            "IsActive",
            typeof(bool),
            typeof(OneCheckToogleButtonExtension),
            new PropertyMetadata(default(bool), IsActiveChanged));

        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.RegisterAttached(
            "IsChecked",
            typeof(bool?),
            typeof(OneCheckToogleButtonExtension),
            new PropertyMetadata(default(bool?), OnIsCheckedChanged));

        public static readonly DependencyProperty IsPanelsVisibleProperty = DependencyProperty.RegisterAttached(
            "IsPanelsVisible",
            typeof(bool),
            typeof(OneCheckToogleButtonExtension),
            new PropertyMetadata(default(bool), OnIsPanelsVisibleChanged));

        public static void SetIsMouseDown(DependencyObject element, bool value)
        {
            element.SetValue(IsMouseDownProperty, value);
        }

        public static bool GetIsMouseDown(DependencyObject element)
        {
            return (bool)element.GetValue(IsMouseDownProperty);
        }

        public static void SetIsActive(DependencyObject element, bool value)
        {
            element.SetValue(IsActiveProperty, value);
        }

        public static bool GetIsActive(DependencyObject element)
        {
            return (bool)element.GetValue(IsActiveProperty);
        }

        public static bool? GetIsChecked(UIElement element)
        {
            return (bool?)element.GetValue(IsCheckedProperty);
        }

        public static void SetIsChecked(UIElement element, bool? value)
        {
            element.SetValue(IsCheckedProperty, value);
        }

        public static bool GetIsPanelsVisibled(UIElement element)
        {
            return (bool)element.GetValue(IsPanelsVisibleProperty);
        }

        public static void SetIsPanelsVisible(UIElement element, bool value)
        {
            element.SetValue(IsPanelsVisibleProperty, value);
        }

        private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var toggleButton = d as ToggleButton;
            if (toggleButton != null)
            {
                if (toggleButton.IsChecked == true && !(bool)toggleButton.GetValue(IsMouseDownProperty))
                {
                    toggleButton.IsChecked = false;
                }

                toggleButton.SetValue(IsMouseDownProperty, false);
            }
        }

        private static void IsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs eventArgs)
        {
            var toggleButton = d as ToggleButton;

            if (toggleButton != null && GetIsActive(toggleButton))
            {
                toggleButton.PreviewMouseDown += OnPreviewMouseDown;
            }
        }

        private static void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var toggleButton = sender as ToggleButton;

            toggleButton?.SetValue(IsMouseDownProperty, true);
        }

        private static void OnIsPanelsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var toggleButton = d as ToggleButton;

            if (toggleButton != null && !GetIsPanelsVisibled(toggleButton))
            {
                SetIsChecked(toggleButton, false);
            }
        }
    }
}
