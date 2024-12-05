namespace ShowroomManagmentSystem.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceCollectionExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            var appSetting = AppSetting.MapValues(configuration);

            services
                .AddAutoMapper()
                .AddNotification()
                .AddScopedServices()
                .AddSingletonServices()
                .AddCustomAuthentication()
                .AddSqlServerConfiguration(appSetting.SqlServerConnection);
            return services;
        }

        //  // add scope
        private static IServiceCollection AddScopedServices(this IServiceCollection services)
        {
            return services;
        }

        //Add Singleton
        private static IServiceCollection AddSingletonServices(this IServiceCollection services)
        {
            services.AddControllersWithViews()
            .AddRazorOptions(opts =>
            {
                opts.ViewLocationFormats.Add("/{0}.cshtml");
            });
            services.AddHttpClient();
            return services;
        }

        // Đăng ký các dịch vụ scoped
        public static IServiceCollection AddSqlServerConfiguration(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }

        // Cấu hình AutoMapper
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddSingleton<IMapper>(sp =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    //cfg.AddProfile<ProductMapping>();
                    //cfg.AddProfile<UserMapping>();
                    // thêm các mapping khác
                });
                return config.CreateMapper();
            });
            return services;
        }

        // Add Authenticate
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    // Cấu hình cookie authentication
                    options.Cookie = new CookieBuilder
                    {
                        HttpOnly = true,
                        Name = "ShowroomManagmentSystem.cookie", // Đặt tên cookie
                        Path = "/",
                        SameSite = SameSiteMode.Lax,
                        SecurePolicy = CookieSecurePolicy.SameAsRequest
                    };

                    options.LoginPath = "/login"; // Đường dẫn login
                    options.ReturnUrlParameter = "returnUrl"; // Tham số đường dẫn trả về
                    options.SlidingExpiration = true; // Bật tính năng hết hạn session động
                });

            return services; // Trả về dịch vụ để tiếp tục cấu hình thêm
        }

        //Add Notification
        public static IServiceCollection AddNotification(this IServiceCollection services)
        {
            // Cấu hình cho Razor Pages và NToastNotify
            services.AddRazorPages()
                .AddNToastNotifyNoty(new NotyOptions
                {
                    ProgressBar = true,
                    Timeout = 5000 // Thời gian hiển thị thông báo
                });

            // Cấu hình Notyf với các tùy chọn cho thông báo
            services.AddNotyf(config =>
            {
                config.DurationInSeconds = 5; // Thời gian hiển thị thông báo
                config.IsDismissable = true; // Cho phép đóng thông báo
                config.Position = NotyfPosition.BottomRight; // Vị trí thông báo
                config.HasRippleEffect = true; // Hiệu ứng ripple khi click vào thông báo
            });
            return services;
        }
    }
}
