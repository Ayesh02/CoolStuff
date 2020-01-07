using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Util;
using Datepicker;
using Datepicker.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomDatePicker), typeof(CustomDatePickerRenderer))]
namespace Datepicker.Droid
{
    public abstract class CustomDatePickerRendererBase<TControl> : ViewRenderer<DatePicker, TControl>, IPickerRenderer
        where TControl : global::Android.Views.View
    {
        DatePickerDialog _dialog;
        bool _disposed;
        protected abstract Android.Widget.EditText EditText { get; }

        public CustomDatePickerRendererBase(Context context) : base(context)
        {
            
        }

        [Obsolete("This constructor is obsolete as of version 2.5. Please use DatePickerRenderer(Context) instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public CustomDatePickerRendererBase()
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                if (_dialog != null)
                {
                    _dialog.Hide();
                    _dialog.Dispose();
                    _dialog = null;
                }
            }
            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                var textField = CreateNativeControl();
                SetNativeControl(textField);
            }

            SetDate(Element.Date);

            UpdateMinimumDate();
            UpdateMaximumDate();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == DatePicker.DateProperty.PropertyName || e.PropertyName == DatePicker.FormatProperty.PropertyName)
                SetDate(Element.Date);
            else if (e.PropertyName == DatePicker.MinimumDateProperty.PropertyName)
                UpdateMinimumDate();
            else if (e.PropertyName == DatePicker.MaximumDateProperty.PropertyName)
                UpdateMaximumDate();
        }

        protected override void OnFocusChangeRequested(object sender, VisualElement.FocusRequestArgs e)
        {
            base.OnFocusChangeRequested(sender, e);

            if (e.Focus)
            {
                if (Clickable)
                    CallOnClick();
                else
                    ((IPickerRenderer)this)?.OnClick();
            }
            else if (_dialog != null)
            {
                _dialog.Hide();
                ((IElementController)Element).SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);

                _dialog = null;
            }
        }

        protected virtual DatePickerDialog CreateDatePickerDialog(int year, int month, int day)
        {
            DatePicker view = Element;
            var dialog = new DatePickerDialog(Context, (o, e) =>
            {
                if (e.Date.DayOfWeek == DayOfWeek.Saturday)
                    view.Date = e.Date.AddDays(2);
                else if (e.Date.DayOfWeek == DayOfWeek.Sunday)
                    view.Date = e.Date.AddDays(1);
                else
                    view.Date = e.Date;
                ((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
            }, year, month, day);
            
            return dialog;
        }

        void DeviceInfoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentOrientation")
            {
                DatePickerDialog currentDialog = _dialog;
                if (currentDialog != null && currentDialog.IsShowing)
                {
                    currentDialog.Dismiss();
                    ShowPickerDialog(currentDialog.DatePicker.Year, currentDialog.DatePicker.Month, currentDialog.DatePicker.DayOfMonth);
                }
            }
        }

        void IPickerRenderer.OnClick()
        {
            if (_dialog != null && _dialog.IsShowing)
            {
                return;
            }

            DatePicker view = Element;
            ((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);

            ShowPickerDialog(view.Date.Year, view.Date.Month - 1, view.Date.Day);
        }

        void ShowPickerDialog(int year, int month, int day)
        {
            _dialog = CreateDatePickerDialog(year, month, day);
            

            UpdateMinimumDate();
            UpdateMaximumDate();
            _dialog.Show();
        }

        void OnCancelButtonClicked(object sender, EventArgs e)
        {
            Element.Unfocus();
        }

        void SetDate(DateTime date)
        {
            EditText.Text = date.ToString(Element.Format);
        }


        void UpdateMaximumDate()
        {
            if (_dialog != null)
            {
                _dialog.DatePicker.MaxDate = (long)Element.MaximumDate.ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
            }
        }

        void UpdateMinimumDate()
        {
            if (_dialog != null)
            {
                _dialog.DatePicker.MinDate = (long)Element.MinimumDate.ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
            }
        }
    }


    public class CustomDatePickerRenderer : CustomDatePickerRendererBase<Android.Widget.EditText>
    {
        [Obsolete("This constructor is obsolete as of version 2.5. Please use DatePickerRenderer(Context) instead.")]
        public CustomDatePickerRenderer()
        {
        }

        public CustomDatePickerRenderer(Context context) : base(context)
        {
        }

        protected override Android.Widget.EditText CreateNativeControl()
        {
            return new PickerEditText(Context);
        }

        protected override Android.Widget.EditText EditText => Control;

    }
}
