using System;
using Android.App;
using Android.OS;
using Android.Widget;

namespace Datepicker.Droid
{
    public class DatePickerFragment : DialogFragment, DatePickerDialog.IOnDateSetListener
    {
        public static readonly string TAG = "X:" + typeof(DatePickerFragment).Name.ToUpper();
        Action<DateTime> _dateSelectedHandler = delegate { };
        public DatePickerFragment(Action<DateTime> onDateSelected)
        {
            _dateSelectedHandler = onDateSelected;
        }
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime now = DateTime.Now;
            return new DatePickerDialog(Activity, this, now.Year, now.Month, now.Day);
        }
        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
            _dateSelectedHandler(selectedDate);
        }

    }
}
