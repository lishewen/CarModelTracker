using Microsoft.Maui.LifecycleEvents;
using CarModelTracker.Services;

namespace CarModelTracker;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // 注册服务
        builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
        builder.Services.AddSingleton<IExportService, ExportService>();
        builder.Services.AddSingleton<IStatisticsService, StatisticsService>();

        // 注册 ViewModel
        builder.Services.AddTransient<ViewModels.HomeViewModel>();
        builder.Services.AddTransient<ViewModels.CarModelViewModel>();
        builder.Services.AddTransient<ViewModels.StatisticsViewModel>();
        builder.Services.AddTransient<ViewModels.SettingsViewModel>();

        // 注册页面
        builder.Services.AddTransient<Views.HomePage>();
        builder.Services.AddTransient<Views.CarModelPage>();
        builder.Services.AddTransient<Views.StatisticsPage>();
        builder.Services.AddTransient<Views.SettingsPage>();

#if ANDROID
        builder.ConfigureLifecycleEvents(events =>
        {
            events.AddAndroid(android => android.OnRequestPermissionsResult((activity, requestCode, permissions, grantResults) =>
            {
                // 处理权限请求结果
            }));
        });
#endif

        var app = builder.Build();

        // 初始化数据库
        var dbService = app.Services.GetRequiredService<IDatabaseService>();
        _ = dbService.GetAllAsync();

        // 设置主页面为 Shell
        Microsoft.Maui.Controls.Application.Current.MainPage = new AppShell();

        return app;
    }
}
