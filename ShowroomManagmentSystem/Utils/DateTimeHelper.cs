namespace ShowroomManagmentSystem.Utils
{
    public static class DateTimeHelper
    {
        private static readonly string DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        // Chuyển chuỗi ngày giờ theo định dạng
        public static DateTime? ParseDateTime(string dateTimeStr, string format, string culture = "en-US")
        {
            try
            {
                return DateTime.ParseExact(dateTimeStr, format, new CultureInfo(culture));
            }
            catch (FormatException)
            {
                return null;
            }
        }

        // Chuyển DateTime thành chuỗi theo định dạng mặc định
        public static string ToDefaultString(DateTime dateTime) => dateTime.ToString(DefaultDateTimeFormat);

        // Tính số ngày giữa 2 ngày
        public static int GetDaysDifference(DateTime startDate, DateTime endDate) => (endDate - startDate).Days;

        // Tính số giờ giữa 2 thời điểm
        public static double GetHoursDifference(DateTime startDate, DateTime endDate) => (endDate - startDate).TotalHours;

        // Chuyển đổi DateTime sang múi giờ khác
        public static DateTime ConvertToTimeZone(DateTime dateTime, TimeZoneInfo fromTimeZone, TimeZoneInfo toTimeZone)
        {
            return TimeZoneInfo.ConvertTime(TimeZoneInfo.ConvertTime(dateTime, fromTimeZone), toTimeZone);
        }

        // Lấy ngày đầu tháng hiện tại
        public static DateTime GetFirstDayOfCurrentMonth() => new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        // Lấy ngày cuối tháng hiện tại
        public static DateTime GetLastDayOfCurrentMonth() => new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

        // Kiểm tra ngày có phải cuối tuần không
        public static bool IsWeekend(DateTime date) => date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }
}
