using ActivitiesManagement.DataAccess;
using ActivitiesManagement.Repositories;

namespace ActivitiesManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<DbHelper>();
            builder.Services.AddScoped<ActionTypeRepository>();
            builder.Services.AddScoped<ActivityMasterRepository>();
            builder.Services.AddScoped<ActivityDetailRepository>();
            builder.Services.AddScoped<CountryRepository>();
            builder.Services.AddScoped<StateRepository>();
            builder.Services.AddScoped<CityRepository>();
            builder.Services.AddScoped<AreaRepository>();
            builder.Services.AddScoped<ExamTypeRepository>();
            builder.Services.AddScoped<ExamProviderRepository>();
            builder.Services.AddScoped<ExamCenterRepository>();
            builder.Services.AddScoped<InstituteTypeRepository>();
            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=ActionType}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
