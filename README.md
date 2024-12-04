# CLI VS code

    Add-Migration v1 -OutputDir Data\Migrations

    Update-Database

# Exam datetime helper

    public void Example()
    {
        DateTime now = DateTime.Now;
        
        // Sử dụng phương thức ToDefaultString để định dạng ngày giờ
        string formattedDate = DateTimeHelper.ToDefaultString(now);
        Console.WriteLine(formattedDate);  // Output: 2024-12-03 14:30:00

        // Sử dụng phương thức GetDaysDifference để tính số ngày giữa 2 ngày
        DateTime pastDate = new DateTime(2024, 1, 1);
        int daysDiff = DateTimeHelper.GetDaysDifference(pastDate, now);
        Console.WriteLine($"Số ngày giữa {pastDate.ToShortDateString()} và {now.ToShortDateString()} là {daysDiff} ngày.");

        // Chuyển đổi múi giờ
        DateTime convertedTime = DateTimeHelper.ConvertToTimeZone(now, TimeZoneInfo.Local, TimeZoneInfo.Utc);
        Console.WriteLine($"Giờ UTC: {convertedTime}");
    }