using OralExamManager.ViewModels;
using OralExamManager.Services;

namespace OralExamManager.Views
{
    [QueryProperty(nameof(ExamId), "ExamId")]
    public partial class AddStudentsPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        
        public int ExamId { get; set; }

        public AddStudentsPage(DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            
            var viewModel = new AddStudentsViewModel(_databaseService, ExamId);
            BindingContext = viewModel;
        }
    }
} 