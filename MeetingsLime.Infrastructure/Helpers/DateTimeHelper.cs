using System.Globalization;

namespace MeetingsLime.Infrastructure.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime ParseToDateTime(string input)
        {
            const string format = "M/d/yyyy h:mm:ss tt";
            if (DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {

                return result;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public static bool IsValidDateTimeSlot(DateTime start, DateTime end)
        {
            if (start == DateTime.MinValue || end == DateTime.MinValue)
            {
                return false;
            }
            else
                return true;
        }
    }
}
