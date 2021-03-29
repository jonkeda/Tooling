using System.Windows;

namespace Tooling.Foundation.UI.Controls.TimePicker
{
    public class TimePickerExBaseSelectionChangedEventArgs<T> : RoutedEventArgs
    {
        public TimePickerExBaseSelectionChangedEventArgs(RoutedEvent eventId, T oldValue, T newValue) :
            base(eventId)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public T OldValue { get; }
        public T NewValue { get; }
    }
}