using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OralExamManager.Models;
using OralExamManager.Services;

namespace OralExamManager.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private ObservableCollection<Exam> _exams = new();

        [ObservableProperty]
        private Exam? _selectedExam;

        [ObservableProperty]
        private string _statusMessage = "";

        public MainViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            _ = LoadExams();
        }

        public ObservableCollection<Exam> AvailableExams => Exams;

        public bool CanStartExam => SelectedExam != null;

        [RelayCommand]
        private async Task LoadExams()
        {
            try
            {
                var exams = await _databaseService.GetExamsAsync();
                Exams.Clear();
                foreach (var exam in exams)
                {
                    Exams.Add(exam);
                }
                StatusMessage = $"Loaded {exams.Count} exams";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading exams: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task CreateExam()
        {
            await Shell.Current.GoToAsync("//CreateExamPage");
        }

        [RelayCommand]
        private async Task AddStudents()
        {
            if (SelectedExam == null)
            {
                var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
                if (mainPage != null)
                {
                    await mainPage.DisplayAlert("Error", "Please select an exam first", "OK");
                }
                return;
            }
            
            await Shell.Current.GoToAsync($"//AddStudentsPage?ExamId={SelectedExam.Id}");
        }

        [RelayCommand]
        private async Task StartExam()
        {
            if (SelectedExam == null)
            {
                var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
                if (mainPage != null)
                {
                    await mainPage.DisplayAlert("Error", "Please select an exam first", "OK");
                }
                return;
            }

            var students = await _databaseService.GetStudentsForExamAsync(SelectedExam.Id);
            if (students.Count == 0)
            {
                var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
                if (mainPage != null)
                {
                    await mainPage.DisplayAlert("Error", "No students added to this exam", "OK");
                }
                return;
            }

            await Shell.Current.GoToAsync($"//ExamPage?ExamId={SelectedExam.Id}");
        }

        [RelayCommand]
        private async Task ViewHistory()
        {
            if (SelectedExam == null)
            {
                var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
                if (mainPage != null)
                {
                    await mainPage.DisplayAlert("Error", "Please select an exam first", "OK");
                }
                return;
            }

            await Shell.Current.GoToAsync($"//HistoryPage?ExamId={SelectedExam.Id}");
        }
    }
} 