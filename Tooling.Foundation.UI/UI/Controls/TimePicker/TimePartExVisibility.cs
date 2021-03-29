using System;

namespace Tooling.Foundation.UI.Controls.TimePicker
{
    /// <summary>
    /// Defines the visibility for time-parts that are visible for the <see cref="DateTimePickerEx"/>. 
    /// </summary>
    [Flags]
    public enum TimePartExVisibility
    {
        Hour = 1 << 1,
        Minute = 1 << 2,
        Second = 1 << 3,
        HourMinute = Hour | Minute,
        All = HourMinute | Second
    }
}