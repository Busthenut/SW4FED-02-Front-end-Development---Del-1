using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OralExamManager.Models;
using OralExamManager.Services;

namespace OralExamManager.ViewModels
{
    public partial class CreateExamViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private string _examTerm = string.Empty;

        [ObservableProperty]
        private string _courseName = string.Empty;

        [ObservableProperty]
        private DateTime _date = DateTime.Today;

        [ObservableProperty]
        private int _numberOfQuestions = 10;

        [ObservableProperty]
        private int _examinationTimeMinutes = 15;

        [ObservableProperty]
        private TimeSpan _startTime = TimeSpan.FromHours(9);

        public CreateExamViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [RelayCommand]
        private async Task SaveExam()
        {
            var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
            if (string.IsNullOrWhiteSpace(ExamTerm))
            {
                if (mainPage != null)
                {
                    await mainPage.DisplayAlert("Error", "Please enter an exam term", "OK");
                }
                return;
            }

            if (string.IsNullOrWhiteSpace(CourseName))
            {
                if (mainPage != null)
                {
                    await mainPage.DisplayAlert("Error", "Please enter a course name", "OK");
                }
                return;
            }

            if (NumberOfQuestions <= 0)
            {
                if (mainPage != null)
                {
                    await mainPage.DisplayAlert("Error", "Number of questions must be greater than 0", "OK");
                }
                return;
            }

            if (ExaminationTimeMinutes <= 0)
            {
                if (mainPage != null)
                {
                    await mainPage.DisplayAlert("Error", "Examination time must be greater than 0", "OK");
                }
                return;
            }

            var exam = new Exam
            {
                ExamTerm = ExamTerm,
                CourseName = CourseName,
                Date = Date,
                NumberOfQuestions = NumberOfQuestions,
                ExaminationTimeMinutes = ExaminationTimeMinutes,
                StartTime = StartTime
            };

            await _databaseService.SaveExamAsync(exam);
            if (mainPage != null)
            {
                await mainPage.DisplayAlert("Success", "Exam created successfully", "OK");
            }
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
} 