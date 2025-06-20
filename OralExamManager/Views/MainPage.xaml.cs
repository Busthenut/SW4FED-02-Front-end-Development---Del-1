using OralExamManager.ViewModels;
using OralExamManager.Services;

namespace OralExamManager.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
} 