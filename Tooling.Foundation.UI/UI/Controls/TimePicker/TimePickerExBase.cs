using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace Tooling.Foundation.UI.Controls.TimePicker
{
    /// <summary>
    ///     Represents a base-class for time picking.
    /// </summary>
    [TemplatePart(Name = ElementButton, Type = typeof(Button))]
    [TemplatePart(Name = ElementHourHand, Type = typeof(UIElement))]
    [TemplatePart(Name = ElementHourPicker, Type = typeof(Selector))]
    [TemplatePart(Name = ElementMinuteHand, Type = typeof(UIElement))]
    [TemplatePart(Name = ElementSecondHand, Type = typeof(UIElement))]
    [TemplatePart(Name = ElementSecondPicker, Type = typeof(Selector))]
    [TemplatePart(Name = ElementMinutePicker, Type = typeof(Selector))]
    [TemplatePart(Name = ElementTextBox, Type = typeof(DatePickerTextBox))]
    [DefaultEvent("SelectedDateTimeChanged")]
    public abstract class TimePickerExBase : Control
    {
        public static readonly DependencyProperty SourceHoursProperty = DependencyProperty.Register(
          "SourceHours",
          typeof(IEnumerable<int>),
          typeof(TimePickerExBase),
          new FrameworkPropertyMetadata(Enumerable.Range(0, 24), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CoerceSourceHours));

        public static readonly DependencyProperty SourceMinutesProperty = DependencyProperty.Register(
            "SourceMinutes",
            typeof(IEnumerable<int>),
            typeof(TimePickerExBase),
            new FrameworkPropertyMetadata(Enumerable.Range(0, 60), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CoerceSource60));

        public static readonly DependencyProperty SourceSecondsProperty = DependencyProperty.Register(
            "SourceSeconds",
            typeof(IEnumerable<int>),
            typeof(TimePickerExBase),
            new FrameworkPropertyMetadata(Enumerable.Range(0, 60), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CoerceSource60));

        public static readonly DependencyProperty IsDropDownOpenProperty = DatePicker.IsDropDownOpenProperty.AddOwner(typeof(TimePickerExBase), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsClockVisibleProperty = DependencyProperty.Register(
            "IsClockVisible",
            typeof(bool),
            typeof(TimePickerExBase),
            new PropertyMetadata(true));

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
            "IsReadOnly",
            typeof(bool),
            typeof(TimePickerExBase),
            new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty HandVisibilityProperty = DependencyProperty.Register(
            "HandVisibility",
            typeof(TimePartExVisibility),
            typeof(TimePickerExBase),
            new PropertyMetadata(TimePartExVisibility.All, OnHandVisibilityChanged));

        public static readonly DependencyProperty CultureProperty = DependencyProperty.Register(
            "Culture",
            typeof(CultureInfo),
            typeof(TimePickerExBase),
            new PropertyMetadata(null, OnCultureChanged));

        public static readonly DependencyProperty PickerVisibilityProperty = DependencyProperty.Register(
            "PickerVisibility",
            typeof(TimePartExVisibility),
            typeof(TimePickerExBase),
            new PropertyMetadata(TimePartExVisibility.All, OnPickerVisibilityChanged));

        public static readonly RoutedEvent SelectedDateTimeChangedEvent = EventManager.RegisterRoutedEvent(
            "SelectedDateTimeChanged", 
            RoutingStrategy.Direct,
            typeof(EventHandler<TimePickerExBaseSelectionChangedEventArgs<DateTime?>>), 
            typeof(TimePickerExBase));

        public static readonly DependencyProperty SelectedDateTimeProperty = DependencyProperty.Register(
            "SelectedDateTime",
            typeof(DateTime?),
            typeof(TimePickerExBase),
            new FrameworkPropertyMetadata(default(DateTime?), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedDateTimeChanged));

        public static readonly DependencyProperty SelectedTimeExFormatProperty = DependencyProperty.Register(
            nameof(SelectedTimeExFormat),
            typeof(TimePickerExFormat),
            typeof(TimePickerExBase),
            new PropertyMetadata(TimePickerExFormat.Long, OnSelectedTimeFormatChanged));

        public static readonly DependencyProperty HoursItemStringFormatProperty = DependencyProperty.Register(nameof(HoursItemStringFormat), typeof(string), typeof(TimePickerExBase), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty MinutesItemStringFormatProperty = DependencyProperty.Register(nameof(MinutesItemStringFormat), typeof(string), typeof(TimePickerExBase), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty SecondsItemStringFormatProperty = DependencyProperty.Register(nameof(SecondsItemStringFormat), typeof(string), typeof(TimePickerExBase), new FrameworkPropertyMetadata(null));

        private const string ElementButton = "PART_Button";
        private const string ElementHourHand = "PART_HourHand";
        private const string ElementHourPicker = "PART_HourPicker";
        private const string ElementMinuteHand = "PART_MinuteHand";
        private const string ElementMinutePicker = "PART_MinutePicker";
        private const string ElementPopup = "PART_Popup";
        private const string ElementSecondHand = "PART_SecondHand";
        private const string ElementSecondPicker = "PART_SecondPicker";
        private const string ElementTextBox = "PART_TextBox";

        #region Do not change order of fields inside this region

        /// <summary>
        /// This readonly dependency property is to control whether to show the date-picker (in case of <see cref="DateTimePickerEx"/>) or hide it (in case of <see cref="TimePickerEx"/>.
        /// </summary>
        private static readonly DependencyPropertyKey IsDatePickerVisiblePropertyKey = DependencyProperty.RegisterReadOnly(
          "IsDatePickerVisible", typeof(bool), typeof(TimePickerExBase), new PropertyMetadata(true));

        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess",Justification = "Otherwise we have \"Static member initializer refers to static member below or in other type part\" and thus resulting in having \"null\" as value")]
        public static readonly DependencyProperty IsDatePickerVisibleProperty = IsDatePickerVisiblePropertyKey.DependencyProperty;

        #endregion

        /// <summary>
        /// This list contains values from 0 to 55 with an interval of 5. It can be used to bind to <see cref="SourceMinutes"/> and <see cref="SourceSeconds"/>.
        /// </summary>
        /// <example>
        /// <code>&lt;MahApps:TimePicker SourceSeconds="{x:Static MahApps:TimePickerBase.IntervalOf5}" /&gt;</code>
        /// <code>&lt;MahApps:DateTimePicker SourceSeconds="{x:Static MahApps:TimePickerBase.IntervalOf5}" /&gt;</code>
        /// </example>
        /// <returns>
        /// Returns a list containing {0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55}.
        /// </returns>
        public static readonly IEnumerable<int> IntervalOf5 = CreateValueList(5);

        /// <summary>
        /// This list contains values from 0 to 50 with an interval of 10. It can be used to bind to <see cref="SourceMinutes"/> and <see cref="SourceSeconds"/>.
        /// </summary>
        /// <example>
        /// <code>&lt;MahApps:TimePicker SourceSeconds="{x:Static MahApps:TimePickerBase.IntervalOf10}" /&gt;</code>
        /// <code>&lt;MahApps:DateTimePicker SourceSeconds="{x:Static MahApps:TimePickerBase.IntervalOf10}" /&gt;</code>
        /// </example>
        /// <returns>
        /// Returns a list containing {0, 10, 20, 30, 40, 50}.
        /// </returns>
        public static readonly IEnumerable<int> IntervalOf10 = CreateValueList(10);

        /// <summary>
        /// This list contains values from 0 to 45 with an interval of 15. It can be used to bind to <see cref="SourceMinutes"/> and <see cref="SourceSeconds"/>.
        /// </summary>
        /// <example>
        /// <code>&lt;MahApps:TimePicker SourceSeconds="{x:Static MahApps:TimePickerBase.IntervalOf15}" /&gt;</code>
        /// <code>&lt;MahApps:DateTimePicker SourceSeconds="{x:Static MahApps:TimePickerBase.IntervalOf15}" /&gt;</code>
        /// </example>
        /// <returns>
        /// Returns a list containing {0, 15, 30, 45}.
        /// </returns>
        public static readonly IEnumerable<int> IntervalOf15 = CreateValueList(15);

        private Button _button;
        private bool _deactivateRangeBaseEvent;
        private bool _deactivateTextChangedEvent;
        private bool _textInputChanged;
        private UIElement _hourHand;
        private Selector _hourInput;
        private UIElement _minuteHand;
        private Selector _minuteInput;
        private Popup _popup;
        private UIElement _secondHand;
        private Selector _secondInput;
        protected DatePickerTextBox _textBox;

        static TimePickerExBase()
        {
            EventManager.RegisterClassHandler(typeof(TimePickerExBase), UIElement.GotFocusEvent, new RoutedEventHandler(OnGotFocus));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimePickerExBase), new FrameworkPropertyMetadata(typeof(TimePickerExBase)));
            VerticalContentAlignmentProperty.OverrideMetadata(typeof(TimePickerExBase), new FrameworkPropertyMetadata(VerticalAlignment.Center));
            LanguageProperty.OverrideMetadata(typeof(TimePickerExBase), new FrameworkPropertyMetadata(OnLanguageChanged));
        }

        protected TimePickerExBase()
        {
            Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, this.OutsideCapturedElementHandler);
        }

        /// <summary>
        ///     Occurs when the <see cref="SelectedDateTime" /> property is changed.
        /// </summary>
        public event EventHandler<TimePickerExBaseSelectionChangedEventArgs<DateTime?>> SelectedDateTimeChanged
        {
            add { AddHandler(SelectedDateTimeChangedEvent, value); }
            remove { RemoveHandler(SelectedDateTimeChangedEvent, value); }
        }

        /// <summary>
        ///     Gets or sets a value indicating the culture to be used in string formatting operations.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(null)]
        public CultureInfo Culture
        {
            get { return (CultureInfo)GetValue(CultureProperty); }
            set { SetValue(CultureProperty, value); }
        }

        /// <summary>
        ///     Gets or sets a value indicating the visibility of the clock hands in the user interface (UI).
        /// </summary>
        /// <returns>
        ///     The visibility definition of the clock hands. The default is <see cref="TimePartExVisibility.All" />.
        /// </returns>
        [Category("Appearance")]
        [DefaultValue(TimePartExVisibility.All)]
        public TimePartExVisibility HandVisibility
        {
            get { return (TimePartExVisibility)GetValue(HandVisibilityProperty); }
            set { SetValue(HandVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the date can be selected or not. This property is read-only.
        /// </summary>
        public bool IsDatePickerVisible
        {
            get { return (bool)GetValue(IsDatePickerVisibleProperty); }
            protected set { SetValue(IsDatePickerVisiblePropertyKey, value); }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the clock of this control is visible in the user interface (UI). This is a
        ///     dependency property.
        /// </summary>
        /// <remarks>
        ///     If this value is set to false then <see cref="Orientation" /> is set to
        ///     <see cref="System.Windows.Controls.Orientation.Vertical" />
        /// </remarks>
        /// <returns>
        ///     true if the clock is visible; otherwise, false. The default value is true.
        /// </returns>
        [Category("Appearance")]
        public bool IsClockVisible
        {
            get { return (bool)GetValue(IsClockVisibleProperty); }
            set { SetValue(IsClockVisibleProperty, value); }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the drop-down for a <see cref="TimePickerExBase"/> box is currently
        ///         open.
        /// </summary>
        /// <returns>true if the drop-down is open; otherwise, false. The default is false.</returns>
        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the contents of the <see cref="TimePickerExBase" /> are not editable.
        /// </summary>
        /// <returns>
        ///     true if the <see cref="TimePickerExBase" /> is read-only; otherwise, false. The default is false.
        /// </returns>
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        /// <summary>
        ///     Gets or sets a value indicating the visibility of the selectable date-time-parts in the user interface (UI).
        /// </summary>
        /// <returns>
        ///     visibility definition of the selectable date-time-parts. The default is <see cref="TimePartExVisibility.All" />.
        /// </returns>
        [Category("Appearance")]
        [DefaultValue(TimePartExVisibility.All)]
        public TimePartExVisibility PickerVisibility
        {
            get { return (TimePartExVisibility)GetValue(PickerVisibilityProperty); }
            set { SetValue(PickerVisibilityProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the currently selected date and time.
        /// </summary>
        /// <returns>
        ///     The date and time which is currently selected. The default is null.
        /// </returns>
        public DateTime? SelectedDateTime
        {
            get { return (DateTime?)GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the format that is used to display the selected time.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(TimePickerExFormat.Long)]
        public TimePickerExFormat SelectedTimeExFormat
        {
            get { return (TimePickerExFormat)GetValue(SelectedTimeExFormatProperty); }
            set { SetValue(SelectedTimeExFormatProperty, value); }
        }

        public string HoursItemStringFormat
        {
            get { return (string)GetValue(HoursItemStringFormatProperty); }
            set { SetValue(HoursItemStringFormatProperty, value); }
        }

        public string MinutesItemStringFormat
        {
            get { return (string)GetValue(MinutesItemStringFormatProperty); }
            set { SetValue(MinutesItemStringFormatProperty, value); }
        }

        public string SecondsItemStringFormat
        {
            get { return (string)GetValue(SecondsItemStringFormatProperty); }
            set { SetValue(SecondsItemStringFormatProperty, value); }
        }

        /// <summary>
        ///     Gets or sets a collection used to generate the content for selecting the hours.
        /// </summary>
        /// <returns>
        ///     A collection that is used to generate the content for selecting the hours. The default is a list of interger from 0
        ///     to 23 if <see cref="IsMilitaryTime" /> is false or a list of interger from
        ///     1 to 12 otherwise..
        /// </returns>
        [Category("Common")]
        public IEnumerable<int> SourceHours
        {
            get { return (IEnumerable<int>)GetValue(SourceHoursProperty); }
            set { SetValue(SourceHoursProperty, value); }
        }

        /// <summary>
        ///     Gets or sets a collection used to generate the content for selecting the minutes.
        /// </summary>
        /// <returns>
        ///     A collection that is used to generate the content for selecting the minutes. The default is a list of int from
        ///     0 to 59.
        /// </returns>
        [Category("Common")]
        public IEnumerable<int> SourceMinutes
        {
            get { return (IEnumerable<int>)GetValue(SourceMinutesProperty); }
            set { SetValue(SourceMinutesProperty, value); }
        }

        /// <summary>
        ///     Gets or sets a collection used to generate the content for selecting the seconds.
        /// </summary>
        /// <returns>
        ///     A collection that is used to generate the content for selecting the minutes. The default is a list of int from
        ///     0 to 59.
        /// </returns>
        [Category("Common")]
        public IEnumerable<int> SourceSeconds
        {
            get { return (IEnumerable<int>)GetValue(SourceSecondsProperty); }
            set { SetValue(SourceSecondsProperty, value); }
        }

        protected internal Popup Popup
        {
            get { return _popup; }
        }

        /// <summary>
        ///     When overridden in a derived class, is invoked whenever application code or internal processes call
        ///     <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            UnSubscribeEvents();

            base.OnApplyTemplate();

            _popup = GetTemplateChild(ElementPopup) as Popup;
            _button = GetTemplateChild(ElementButton) as Button;
            _hourInput = GetTemplateChild(ElementHourPicker) as Selector;
            _minuteInput = GetTemplateChild(ElementMinutePicker) as Selector;
            _secondInput = GetTemplateChild(ElementSecondPicker) as Selector;
            _hourHand = GetTemplateChild(ElementHourHand) as FrameworkElement;
            _minuteHand = GetTemplateChild(ElementMinuteHand) as FrameworkElement;
            _secondHand = GetTemplateChild(ElementSecondHand) as FrameworkElement;
            _textBox = GetTemplateChild(ElementTextBox) as DatePickerTextBox;

            SetHandVisibility(HandVisibility);
            SetPickerVisibility(PickerVisibility);

            SetHourPartValues(SelectedDateTime.GetValueOrDefault().TimeOfDay);
            WriteValueToTextBox();

            SetDefaultTimeOfDayValues();
            SubscribeEvents();
            ApplyCulture();
            ApplyBindings();

        }

        protected virtual void ApplyBindings()
        {
            if (Popup != null)
            {
                Popup.SetBinding(Popup.IsOpenProperty, GetBinding(IsDropDownOpenProperty));
            }
        }

        protected virtual void ApplyCulture()
        {
            _deactivateRangeBaseEvent = true;

            CoerceValue(SourceHoursProperty);

            if (SelectedDateTime.HasValue)
            {
                SetHourPartValues(SelectedDateTime.Value.TimeOfDay);
            }

            SetDefaultTimeOfDayValues();
            _deactivateRangeBaseEvent = false;

            WriteValueToTextBox();
        }

        protected Binding GetBinding(DependencyProperty property)
        {
            return new Binding(property.Name) { Source = this };
        }
        
        protected virtual string GetValueForTextBox()
        {
            var valueForTextBox = SelectedDateTime?.ToString();
            return valueForTextBox;
        }

        protected abstract void OnTextBoxLostFocus(object sender, RoutedEventArgs e);

        protected virtual void OnRangeBaseValueChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetCurrentValue(SelectedDateTimeProperty, this.SelectedDateTime.GetValueOrDefault(DateTime.Today).Date + this.GetSelectedTimeFromGUI());
        }

        protected virtual void OnSelectedTimeChanged(TimePickerExBaseSelectionChangedEventArgs<DateTime?> e)
        {
            RaiseEvent(e);
        }

        private static void OnSelectedTimeFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tp = d as TimePickerExBase;
            if (tp != null)
            {
                tp.WriteValueToTextBox();
            }
        }

        protected void SetDefaultTimeOfDayValues()
        {
            SetDefaultTimeOfDayValue(_hourInput);
            SetDefaultTimeOfDayValue(_minuteInput);
            SetDefaultTimeOfDayValue(_secondInput);
        }

        protected virtual void SubscribeEvents()
        {
            SubscribeRangeBaseValueChanged(_hourInput, _minuteInput, _secondInput);

            if (_button != null)
            {
                _button.Click += OnButtonClicked;
            }
            if (_textBox != null)
            {
                _textBox.TextChanged += OnTextChanged;
                _textBox.LostFocus += InternalOnTextBoxLostFocus;
            }
        }

        protected virtual void UnSubscribeEvents()
        {
            UnsubscribeRangeBaseValueChanged(_hourInput, _minuteInput, _secondInput);

            if (_button != null)
            {
                _button.Click -= OnButtonClicked;
            }
            if (_textBox != null)
            {
                _textBox.TextChanged -= OnTextChanged;
                _textBox.LostFocus -= InternalOnTextBoxLostFocus;
            }
        }

        protected void WriteValueToTextBox(string value)
        {
            if (_textBox != null)
            {
                _deactivateTextChangedEvent = true;
                _textBox.Text = value;
                _deactivateTextChangedEvent = false;
            }
        }

        protected virtual void WriteValueToTextBox()
        {
            WriteValueToTextBox(GetValueForTextBox());
        }

        private static IList<int> CreateValueList(int interval)
        {
            return Enumerable.Repeat(interval, 60 / interval)
                             .Select((value, index) => value * index)
                             .ToList();
        }

        private static object CoerceSource60(DependencyObject d, object basevalue)
        {
            var list = basevalue as IEnumerable<int>;
            if (list != null)
            {
                return list.Where(i => i >= 0 && i < 60);
            }

            return Enumerable.Empty<int>();
        }

        private static object CoerceSourceHours(DependencyObject d, object basevalue)
        {
            var timePickerBase = d as TimePickerExBase;
            var hourList = basevalue as IEnumerable<int>;
            if (timePickerBase != null && hourList != null)
            {
                return hourList.Where(i => i >= 0 && i < 24);
            }
            return Enumerable.Empty<int>();
        }

        private void InternalOnTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (_textInputChanged)
            {
                _textInputChanged = false;

                OnTextBoxLostFocus(sender, e);
            }
        }

        private void InternalOnRangeBaseValueChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_deactivateRangeBaseEvent)
            {
                OnRangeBaseValueChanged(sender, e);
            }
        }

        private static void OnCultureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var timePartPickerBase = (TimePickerExBase)d;

            var info = e.NewValue as CultureInfo;
            timePartPickerBase.Language = info != null ? XmlLanguage.GetLanguage(info.IetfLanguageTag) : XmlLanguage.Empty;

            timePartPickerBase.ApplyCulture();
        }

        private static void OnLanguageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var timePartPickerBase = (TimePickerExBase)d;

            timePartPickerBase.Language = e.NewValue as XmlLanguage ?? XmlLanguage.Empty;

            timePartPickerBase.ApplyCulture();
        }

        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsKeyboardFocusWithinChanged(e);
            // To hide the popup when the user e.g. alt+tabs, monitor for when the window becomes a background window.
            if (!(bool)e.NewValue)
            {
                this.IsDropDownOpen = false;
            }
        }

        private void OutsideCapturedElementHandler(object sender, MouseButtonEventArgs e)
        {
            this.IsDropDownOpen = false;
        }

        private static void OnGotFocus(object sender, RoutedEventArgs e)
        {
            TimePickerExBase pickerEx = (TimePickerExBase)sender;
            if (!e.Handled && pickerEx.Focusable)
            {
                if (Equals(e.OriginalSource, pickerEx))
                {
                    // MoveFocus takes a TraversalRequest as its argument.
                    var request = new TraversalRequest((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift ? FocusNavigationDirection.Previous : FocusNavigationDirection.Next);
                    // Gets the element with keyboard focus.
                    var elementWithFocus = Keyboard.FocusedElement as UIElement;
                    // Change keyboard focus.
                    if (elementWithFocus != null)
                    {
                        elementWithFocus.MoveFocus(request);
                    }
                    else
                    {
                        pickerEx.Focus();
                    }

                    e.Handled = true;
                }
                else if (pickerEx._textBox != null && Equals(e.OriginalSource, pickerEx._textBox))
                {
                    pickerEx._textBox.SelectAll();
                    e.Handled = true;
                }
            }
        }

        private static void OnHandVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TimePickerExBase)d).SetHandVisibility((TimePartExVisibility)e.NewValue);
        }

        private static void OnPickerVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TimePickerExBase)d).SetPickerVisibility((TimePartExVisibility)e.NewValue);
        }

        private static void OnSelectedDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var timePartPickerBase = (TimePickerExBase)d;

            if (timePartPickerBase._deactivateRangeBaseEvent)
            {
                return;
            }

            timePartPickerBase.SetHourPartValues((e.NewValue as DateTime?).GetValueOrDefault().TimeOfDay);

            timePartPickerBase.OnSelectedTimeChanged(new TimePickerExBaseSelectionChangedEventArgs<DateTime?>(SelectedDateTimeChangedEvent, (DateTime?)e.OldValue, (DateTime?)e.NewValue));

            timePartPickerBase.WriteValueToTextBox();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_deactivateTextChangedEvent)
            {
                _textInputChanged = true;
            }
        }

        private static void SetVisibility(UIElement partHours, UIElement partMinutes, UIElement partSeconds, TimePartExVisibility visibility)
        {
            if (partHours != null)
            {
                partHours.Visibility = visibility.HasFlag(TimePartExVisibility.Hour) ? Visibility.Visible : Visibility.Collapsed;
            }

            if (partMinutes != null)
            {
                partMinutes.Visibility = visibility.HasFlag(TimePartExVisibility.Minute) ? Visibility.Visible : Visibility.Collapsed;
            }

            if (partSeconds != null)
            {
                partSeconds.Visibility = visibility.HasFlag(TimePartExVisibility.Second) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private static bool IsValueSelected(Selector selector)
        {
            return selector != null && selector.SelectedItem != null;
        }

        private static void SetDefaultTimeOfDayValue(Selector selector)
        {
            if (selector != null)
            {
                if (selector.SelectedValue == null)
                {
                    selector.SelectedIndex = 0;
                }
            }
        }

        protected TimeSpan? GetSelectedTimeFromGUI()
        {
            {
                if (IsValueSelected(_hourInput) &&
                    IsValueSelected(_minuteInput) &&
                    IsValueSelected(_secondInput))
                {
                    var hours = (int)_hourInput.SelectedItem;
                    var minutes = (int)_minuteInput.SelectedItem;
                    var seconds = (int)_secondInput.SelectedItem;

                    return new TimeSpan(hours, minutes, seconds);
                }

                return this.SelectedDateTime.GetValueOrDefault().TimeOfDay;
            }
        }

       

        private void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            IsDropDownOpen = !IsDropDownOpen;
            if (Popup != null)
            {
                Popup.IsOpen = IsDropDownOpen;
            }
        }

        private void SetHandVisibility(TimePartExVisibility visibility)
        {
            SetVisibility(_hourHand, _minuteHand, _secondHand, visibility);
        }

        private void SetHourPartValues(TimeSpan timeOfDay)
        {
            if (this._deactivateRangeBaseEvent)
            {
                return;
            }

            _deactivateRangeBaseEvent = true;
            if (_hourInput != null)
            {

                _hourInput.SelectedValue = timeOfDay.Hours;
            }

            if (_minuteInput != null)
            {
                _minuteInput.SelectedValue = timeOfDay.Minutes;
            }

            if (_secondInput != null)
            {
                _secondInput.SelectedValue = timeOfDay.Seconds;
            }

            _deactivateRangeBaseEvent = false;
        }

        private void SetPickerVisibility(TimePartExVisibility visibility)
        {
            SetVisibility(_hourInput, _minuteInput, _secondInput, visibility);
        }

        private void SubscribeRangeBaseValueChanged(params Selector[] selectors)
        {
            foreach (var selector in selectors.Where(i => i != null))
            {
                selector.SelectionChanged += InternalOnRangeBaseValueChanged;
            }
        }

        private void UnsubscribeRangeBaseValueChanged(params Selector[] selectors)
        {
            foreach (var selector in selectors.Where(i => i != null))
            {
                selector.SelectionChanged -= InternalOnRangeBaseValueChanged;
            }
        }
    }
}