﻿namespace ShowroomManagmentSystem.Configurations
{
    public static class AppConfiguration
    {
        public static void ConfigureMiddleware(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //app.UseSession();
            app.UseRouting();

            // Using cookie and authen -author in this app
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            // Using toast notify and notyf in this app
            app.UseNToastNotify();
            app.UseNotyf();

        }
    }
}