using Infra.Wpf.Converters;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Infra.Wpf.Common
{
    [TypeConverter(typeof(PersianDateConverter))]
    public class PersianDate : IComparable<PersianDate>
    {
        #region Properties
        
        private DateTime _currentDateTime;
        
        private DateTime currentDateTime
        {
            set
            {
                _currentDateTime = value;
                Day = persianCalendar.GetDayOfMonth(value);
                Month = persianCalendar.GetMonth(value);
                Year = persianCalendar.GetYear(value);
                PersianDayOfWeek = (PersianDayOfWeek)persianCalendar.GetDayOfWeek(value);
                DayofYear = persianCalendar.GetDayOfYear(value);
            }
            get
            {
                return _currentDateTime;
            }
        }
        
        private static PersianCalendar persianCalendar;
        
        private static CultureInfo culture;
        
        public static readonly PersianDate MaxValue;
        
        public static readonly PersianDate MinValue;
        
        public static PersianDate Today
        {
            get
            {
                return new PersianDate(DateTime.Today);
            }
        }
        
        private int _Day;
        
        public int Day
        {
            private set 
            {
                _Day = value;
            }
            get
            {
                return _Day;
            }
        }
        
        private int _Month;
        
        public int Month
        {
            private set
            {
                _Month = value;
            }
            get
            {
                return _Month;
            }
        }
        
        private int _Year;
        
        public int Year
        {
            private set
            {
                _Year = value;
            }
            get
            {
                return _Year;
            }
        }
        
        private int _DayofYear;
        
        public int DayofYear
        {
            private set
            {
                _DayofYear = value;
            }
            get
            {
                return _DayofYear;
            }
        }
        
        public PersianMonth MonthAsPersianMonth
        {
            get
            {
                return (PersianMonth)Month;
            }
        }
        
        private PersianDayOfWeek _PersianDayOfWeek;
        
        public PersianDayOfWeek PersianDayOfWeek
        {
            private set
            {
                _PersianDayOfWeek = value;
            }
            get
            {
                return _PersianDayOfWeek;
            }
        }
        
        #endregion
        
        #region Static Methods
        
        static PersianDate()
        {
            culture = new CultureInfo("fa-IR");
            
            DateTimeFormatInfo formatInfo = culture.DateTimeFormat;
            formatInfo.AbbreviatedDayNames = new[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };
            formatInfo.DayNames = Enum.GetNames(typeof(PersianDayOfWeek));
            string[] monthNames = Enum.GetNames(typeof(PersianMonth)).Concat(new string[1] { "" }).ToArray();
            formatInfo.AbbreviatedMonthNames = monthNames;
            formatInfo.MonthNames = monthNames;
            formatInfo.MonthGenitiveNames = monthNames;
            formatInfo.AbbreviatedMonthGenitiveNames = monthNames;
            formatInfo.AMDesignator = "ق.ظ";
            formatInfo.PMDesignator = "ب.ظ";
            formatInfo.ShortDatePattern = "yyyy/MM/dd";
            formatInfo.LongDatePattern = "dddd, dd MMMM,yyyy";
            formatInfo.FirstDayOfWeek = DayOfWeek.Saturday;
            
            persianCalendar = new PersianCalendar();
            
            FieldInfo fieldInfo = culture.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
                fieldInfo.SetValue(culture, persianCalendar);
            
            FieldInfo info = formatInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
            if (info != null)
                info.SetValue(formatInfo, persianCalendar);
            
            culture.NumberFormat.NumberDecimalSeparator = "/";
            culture.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
            culture.NumberFormat.NumberNegativePattern = 0;
            
            MaxValue = new PersianDate(9378, 10, 10);
            MinValue = new PersianDate(1, 1, 1);
        }
        
        public static int DaysInMonth(int year, int month)
        {
            return persianCalendar.GetDaysInMonth(year, month);
        }
        
        public static PersianDate Parse(string persianDateString)
        {
            string[] parts = persianDateString.Split('/');
            if (parts.Length != 3)
                throw new ArgumentException("The date string must be in the form y/m/d");
            
            var style = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;
            try
            {
                if (string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]) || string.IsNullOrWhiteSpace(parts[2]))
                    throw new Exception();
                
                int y = int.Parse(parts[0], style);
                int m = int.Parse(parts[1], style);
                int d = int.Parse(parts[2], style);
                return new PersianDate(y, m, d);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("The date string must be in the form y/m/d", ex);
            }
        }
        
        public static bool TryParse(string presianDateString, out PersianDate result)
        {
            try
            {
                result = Parse(presianDateString);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static bool TryParse(DateTime date, out PersianDate result)
        {
            try
            {
                result = new PersianDate(date);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }
        
        public static bool IsValid(int year, int month, int day)
        {
            if (month < 1 || month > 12)
                return false;
            
            if (day < 1)
                return false;
            
            if (year < 1 || year > 9378)
                return false;
            
            if (year == 9378)
            {
                if (month > 10)
                    return false;
                if (month == 10)
                {
                    if (day > 10)
                        return false;
                }
            }
            
            if (month < 7 && day > 31)
                return false;
            
            if (month >= 7 && day > 30)
                return false;
            
            if (month == 12 && day > 29 && !IsLeapYear(year))
                return false;
            
            return true;
        }
        
        public static bool IsValid(int year, int month, int day, int hour, int minute, int second)
        {
            if (!IsValid(year, month, day))
                return false;
            
            if (hour < 0 && hour > 23)
                return false;
            
            if (minute < 0 && minute > 59)
                return false;
            
            if (second < 0 && second > 59)
                return false;
            
            return true;
        }
        
        private static bool IsLeapYear(int year)
        {
            return persianCalendar.IsLeapYear(year);
        }
        
        public static bool operator <(PersianDate x, PersianDate y)
        {
            if (x == null && y == null)
                return false;
            else if (x == null)
                return true;
            else if (y == null)
                return false;
            else
                return x.CompareTo(y) < 0;
        }
        
        public static bool operator <=(PersianDate x, PersianDate y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null)
                return true;
            else if (y == null)
                return false;
            else
                return x.CompareTo(y) <= 0;
        }
        
        public static bool operator ==(PersianDate x, PersianDate y)
        {
            if (object.ReferenceEquals(x, null))
                return object.ReferenceEquals(y, null);
            
            return x.CompareTo(y) == 0;
        }
        
        public static bool operator >=(PersianDate x, PersianDate y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null)
                return false;
            else if (y == null)
                return true;
            else
                return x.CompareTo(y) >= 0;
        }
        
        public static bool operator >(PersianDate x, PersianDate y)
        {
            if (x == null && y == null)
                return false;
            else if (x == null)
                return false;
            else if (y == null)
                return true;
            else
                return x.CompareTo(y) > 0;
        }
        
        public static bool operator !=(PersianDate x, PersianDate y)
        {
            if (object.ReferenceEquals(x, null))
                return !object.ReferenceEquals(y, null);
            
            return x.CompareTo(y) != 0;
        }
        
        public static PersianDate operator +(PersianDate persianDate, int days)
        {
            try
            {
                return new PersianDate(persianCalendar.AddDays(persianDate.ToDateTime(), days));
            }
            catch (OverflowException ex)
            {
                throw new OverflowException("The resulting date of the addition is outside of acceptable range.", ex);
            }
        }
        
        public static PersianDate operator -(PersianDate persianDate, int days)
        {
            try
            {
                return new PersianDate(persianCalendar.AddDays(persianDate.ToDateTime(), -days));
            }
            catch (OverflowException ex)
            {
                throw new OverflowException("The resulting date of the addition is outside of acceptable range.", ex);
            }
        }
        
        public static TimeSpan operator -(PersianDate x, PersianDate y)
        {
            long l = x.currentDateTime.Ticks - y.currentDateTime.Ticks;
            return new TimeSpan(l);
        }
        
        #endregion
        
        #region Non-Static Methods
        
        public PersianDate()
        {
            currentDateTime = DateTime.Today;
        }
        
        public PersianDate(int year, int month, int day)
        {
            if (!IsValid(year, month, day))
                throw new ArgumentException(string.Format("The date ({0}/{1}/{2}) is not a valid date.", year, month, day));
            
            currentDateTime = persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
        }
        
        public PersianDate(PersianDate date, TimeSpan time)
        {
            currentDateTime = persianCalendar.ToDateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds, 0);
        }
        
        public PersianDate(TimeSpan time)
        {
            currentDateTime = persianCalendar.ToDateTime(1, 1, 1, time.Hours, time.Minutes, time.Seconds, 0);
        }
        
        public PersianDate(int year, int month, int day, int hour, int minute, int second)
        {
            if (!IsValid(year, month, day, hour, minute, second))
                throw new ArgumentException(string.Format("The date ({0}/{1}/{2} {3}:{4}:{5}) is not a valid date.",
                    year, month, day, hour, minute, second));
            
            currentDateTime = persianCalendar.ToDateTime(year, month, day, hour, minute, second, 0);
        }
        
        public PersianDate(DateTime dateTime)
        {
            currentDateTime = dateTime;
        }
        
        public bool IsLeapYear()
        {
            return IsLeapYear(Year);
        }
        
        public int CompareTo(PersianDate that)
        {
            if (that == null)
                return 1;
            return currentDateTime.Date.CompareTo(that.currentDateTime.Date);
        }
        
        public override bool Equals(object obj)
        {
            if ((obj == null) || !typeof(PersianDate).Equals(obj.GetType()))
                return false;
            else
                return (PersianDate)obj == this;
        }
        
        public override int GetHashCode()
        {
            return currentDateTime.GetHashCode();
        }
        
        public PersianDate AddDays(int days)
        {
            return this + days;
        }
        
        public PersianDate AddMounth(int months)
        {
            return new PersianDate(currentDateTime.AddMonths(months));
        }
        
        public PersianDate AddYear(int years)
        {
            return new PersianDate(currentDateTime.AddYears(years));
        }
        
        public DateTime ToDateTime()
        {
            return currentDateTime;
        }
        
        public override string ToString()
        {
            return string.Format("{0:D4}/{1:D2}/{2:D2}", Year, Month, Day);
        }
        
        public string ToString(string format)
        {
            return currentDateTime.ToString(format, culture);
        }
        
        public string ToLongDateString()
        {
            return ToString("dddd، dd MMMM، yyyy");
        }
        
        #endregion
    }
}