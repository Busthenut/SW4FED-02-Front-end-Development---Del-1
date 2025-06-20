using OralExamManager.ViewModels;
using OralExamManager.Services;

namespace OralExamManager.Views
{
    public partial class CreateExamPage : ContentPage
    {
        public CreateExamPage(CreateExamViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
} 