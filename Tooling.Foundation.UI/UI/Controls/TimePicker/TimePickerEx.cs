using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Tooling.Foundation.UI.Controls.TimePicker
{
    /// <summary>
    ///     Represents a control that allows the user to select a time.
    /// </summary>
    public class TimePickerEx : TimePickerExBase
    {
        static TimePickerEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimePickerEx), new FrameworkPropertyMetadata(typeof(TimePickerEx)));
        }

        public TimePickerEx()
        {
            IsDatePickerVisible = false;
        }

        protected override void OnTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            TimeSpan ts;
            if (TimeSpan.TryParse(((DatePickerTextBox)sender).Text, CultureInfo.CurrentCulture, out ts))
            {
                this.SetCurrentValue(SelectedDateTimeProperty, this.SelectedDateTime.GetValueOrDefault().Date + ts);
            }
            else
            {
                if (string.IsNullOrEmpty(((DatePickerTextBox)sender).Text))
                {
                    this.SetCurrentValue(SelectedDateTimeProperty, null);
                    WriteValueToTextBox(string.Empty);
                }
                else
                {
                    WriteValueToTextBox();
                }
            }
        }
    }
}