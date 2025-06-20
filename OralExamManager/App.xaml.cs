using OralExamManager.Views;
using OralExamManager.ViewModels;
using OralExamManager.Services;

namespace OralExamManager;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var appShell = Handler?.MauiContext?.Services.GetRequiredService<AppShell>();
		return new Window(appShell ?? new AppShell());
	}
}