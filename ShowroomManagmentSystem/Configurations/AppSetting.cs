namespace ShowroomManagmentSystem.Configurations
{
    public class AppSetting
    {
        public string SqlServerConnection { get; set; }
        public string RedisConnection { get; set; }

        // Phương thức ánh xạ từ IConfiguration
        public static AppSetting MapValues(IConfiguration configuration)
        {
            var appSetting = new AppSetting();
            configuration.Bind("AppSettings", appSetting);
            return appSetting;
        }
    }
}
