using OralExamManager.Views;

namespace OralExamManager;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		
		// Register routes - these will be resolved through DI when needed
		Routing.RegisterRoute("CreateExamPage", typeof(CreateExamPage));
		Routing.RegisterRoute("AddStudentsPage", typeof(AddStudentsPage));
		Routing.RegisterRoute("ExamPage", typeof(ExamPage));
		Routing.RegisterRoute("HistoryPage", typeof(HistoryPage));
	}
}
