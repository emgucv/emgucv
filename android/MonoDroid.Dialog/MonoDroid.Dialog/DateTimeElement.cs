using System;
using Android.Content;

namespace MonoDroid.Dialog
{
    public class DateTimeElement : StringElement
    {
        public DateTime DateValue;

        public DateTimeElement(string caption, DateTime date)
            : base(caption)
        {
            DateValue = date;
            Value = FormatDate(date);
        }

        public virtual string FormatDate(DateTime dt)
        {
            ///FIXME
            // return fmt.ToString(dt) + " " + dt.ToLocalTime().ToShortTimeString();
            return dt.ToString();
        }
    }

    public class DateElement : DateTimeElement
    {
        public DateElement(string caption, DateTime date) : base(caption, date)
        {
        }
    }

    public class TimeElement : DateTimeElement
    {
        public TimeElement(string caption, DateTime date) : base(caption, date)
        {
        }
    }
}