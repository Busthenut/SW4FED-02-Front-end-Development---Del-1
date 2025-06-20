using Microsoft.Extensions.Logging;
using OralExamManager.Services;
using OralExamManager.ViewModels;
using OralExamManager.Views;

namespace OralExamManager;

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

		// Register services
		builder.Services.AddSingleton<DatabaseService>();
		builder.Services.AddTransient<MainViewModel>();
		builder.Services.AddTransient<CreateExamViewModel>();
		builder.Services.AddTransient<AddStudentsViewModel>();
		builder.Services.AddTransient<ExamViewModel>();
		builder.Services.AddTransient<HistoryViewModel>();

		// Register pages
		builder.Services.AddTransient<Views.MainPage>();
		builder.Services.AddTransient<Views.AddStudentsPage>();
		builder.Services.AddTransient<Views.ExamPage>();
		builder.Services.AddTransient<Views.HistoryPage>();
		builder.Services.AddTransient<Views.CreateExamPage>();
		builder.Services.AddTransient<AppShell>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
