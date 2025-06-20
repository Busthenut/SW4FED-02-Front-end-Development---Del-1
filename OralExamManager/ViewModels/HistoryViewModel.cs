using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OralExamManager.Models;
using OralExamManager.Services;

namespace OralExamManager.ViewModels
{
    public partial class HistoryViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private readonly int _examId;

        [ObservableProperty]
        private Exam? _exam;

        [ObservableProperty]
        private ObservableCollection<ExamResultViewModel> _results = new();

        [ObservableProperty]
        private double _averageGrade;

        public HistoryViewModel(DatabaseService databaseService, int examId)
        {
            _databaseService = databaseService;
            _examId = examId;
            _ = LoadHistory();
        }

        [RelayCommand]
        private async Task LoadHistory()
        {
            Exam = await _databaseService.GetExamAsync(_examId);
            var results = await _databaseService.GetExamResultsAsync(_examId);
            var students = await _databaseService.GetStudentsForExamAsync(_examId);
            
            Results.Clear();
            foreach (var result in results)
            {
                var student = students.FirstOrDefault(s => s.Id == result.StudentId);
                if (student != null)
                {
                    Results.Add(new ExamResultViewModel
                    {
                        StudentName = student.Name,
                        StudentId = student.StudentId,
                        QuestionNumber = result.QuestionNumber,
                        ActualExaminationTime = result.ActualExaminationTime,
                        Notes = result.Notes,
                        Grade = result.Grade
                    });
                }
            }
            
            AverageGrade = await _databaseService.GetAverageGradeAsync(_examId);
        }

        [RelayCommand]
        private async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }
    }

    public partial class ExamResultViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _studentName = string.Empty;

        [ObservableProperty]
        private string _studentId = string.Empty;

        [ObservableProperty]
        private int _questionNumber;

        [ObservableProperty]
        private TimeSpan _actualExaminationTime;

        [ObservableProperty]
        private string _notes = string.Empty;

        [ObservableProperty]
        private int _grade;
    }
} 