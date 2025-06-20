using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OralExamManager.Models;
using OralExamManager.Services;

namespace OralExamManager.ViewModels
{
    public partial class ExamViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private readonly int _examId;
        private List<Student> _students = new();
        private int _currentStudentIndex = 0;
        private IDispatcherTimer? _timer;
        private DateTime _examStartTime;

        [ObservableProperty]
        private Exam? _exam;

        [ObservableProperty]
        private Student? _currentStudent;

        [ObservableProperty]
        private int _drawnQuestion = 0;

        [ObservableProperty]
        private bool _isExamStarted = false;

        [ObservableProperty]
        private bool _isExamEnded = false;

        [ObservableProperty]
        private TimeSpan _remainingTime;

        [ObservableProperty]
        private TimeSpan _actualExaminationTime;

        [ObservableProperty]
        private string _notes = string.Empty;

        [ObservableProperty]
        private int _selectedGrade = 0;

        public ExamViewModel(DatabaseService databaseService, int examId)
        {
            _databaseService = databaseService;
            _examId = examId;
            
            InitializeTimer();
            _ = LoadExamData();
        }

        private void InitializeTimer()
        {
            _timer = Application.Current?.Dispatcher.CreateTimer();
            if (_timer != null)
            {
                _timer.Interval = TimeSpan.FromSeconds(1);
                _timer.Tick += Timer_Tick!;
            }
        }

        private async void Timer_Tick(object? sender, EventArgs e)
        {
            if (RemainingTime.TotalSeconds > 0)
            {
                RemainingTime = RemainingTime.Subtract(TimeSpan.FromSeconds(1));
            }
            else
            {
                _timer?.Stop();
                var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
                if (mainPage != null)
                {
                    await mainPage.DisplayAlert("Time's Up!", 
                        "The examination time has expired.", "OK");
                }
                await EndExamination();
            }
        }

        [RelayCommand]
        private async Task LoadExamData()
        {
            Exam = await _databaseService.GetExamAsync(_examId);
            _students = await _databaseService.GetStudentsForExamAsync(_examId);
            
            if (_students.Count > 0 && Exam != null)
            {
                CurrentStudent = _students[0];
                RemainingTime = TimeSpan.FromMinutes(Exam.ExaminationTimeMinutes);
            }
        }

        [RelayCommand]
        private async Task DrawQuestion()
        {
            if (Exam == null) return;

            var random = new Random();
            DrawnQuestion = random.Next(1, Exam.NumberOfQuestions + 1);
            
            var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
            if (mainPage != null)
            {
                await mainPage.DisplayAlert("Question Drawn", 
                    $"Question number: {DrawnQuestion}", "OK");
            }
        }

        [RelayCommand]
        private async Task StartExamination()
        {
            var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
            if (DrawnQuestion == 0)
            {
                if (mainPage != null)
                {
                    await mainPage.DisplayAlert("Error", 
                        "Please draw a question first", "OK");
                }
                return;
            }

            IsExamStarted = true;
            _examStartTime = DateTime.Now;
            _timer?.Start();
            
            if (mainPage != null)
            {
                await mainPage.DisplayAlert("Examination Started", 
                    "The timer has started. Good luck!", "OK");
            }
        }

        [RelayCommand]
        private async Task EndExamination()
        {
            var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
            if (!IsExamStarted) return;

            _timer?.Stop();
            IsExamStarted = false;
            IsExamEnded = true;
            
            ActualExaminationTime = DateTime.Now - _examStartTime;
            
            if (mainPage != null)
            {
                await mainPage.DisplayAlert("Examination Ended", 
                    $"Actual time: {ActualExaminationTime:mm\\:ss}", "OK");
            }
        }

        [RelayCommand]
        private async Task SaveGrade1() => await SaveGrade(1);

        [RelayCommand]
        private async Task SaveGrade2() => await SaveGrade(2);

        [RelayCommand]
        private async Task SaveGrade3() => await SaveGrade(3);

        [RelayCommand]
        private async Task SaveGrade4() => await SaveGrade(4);

        [RelayCommand]
        private async Task SaveGrade5() => await SaveGrade(5);

        private async Task SaveGrade(int grade)
        {
            var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
            SelectedGrade = grade;
            
            if (string.IsNullOrWhiteSpace(Notes))
            {
                if (mainPage != null)
                {
                    var result = await mainPage.DisplayAlert("Warning", 
                        "No notes entered. Continue without notes?", "Yes", "No");
                    if (!result) return;
                }
            }

            if (CurrentStudent == null) return;

            var examResult = new ExamResult
            {
                ExamId = _examId,
                StudentId = CurrentStudent.Id,
                QuestionNumber = DrawnQuestion,
                ActualExaminationTime = ActualExaminationTime,
                Notes = Notes,
                Grade = SelectedGrade
            };

            await _databaseService.SaveExamResultAsync(examResult);
            
            if (mainPage != null)
            {
                await mainPage.DisplayAlert("Grade Saved", 
                    $"Grade {SelectedGrade} saved for {CurrentStudent.Name}", "OK");
            }
            
            await NextStudent();
        }

        [RelayCommand]
        private async Task NextStudent()
        {
            var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
            _currentStudentIndex++;
            
            if (_currentStudentIndex >= _students.Count)
            {
                // Mark exam as completed
                if (Exam != null)
                {
                    Exam.IsCompleted = true;
                    await _databaseService.SaveExamAsync(Exam);
                }
                
                if (mainPage != null)
                {
                    await mainPage.DisplayAlert("Exam Complete", 
                        "All students have been examined!", "OK");
                }
                await Shell.Current.GoToAsync("..");
                return;
            }

            // Reset for next student
            CurrentStudent = _students[_currentStudentIndex];
            DrawnQuestion = 0;
            IsExamStarted = false;
            IsExamEnded = false;
            if (Exam != null)
            {
                RemainingTime = TimeSpan.FromMinutes(Exam.ExaminationTimeMinutes);
            }
            ActualExaminationTime = TimeSpan.Zero;
            Notes = string.Empty;
            SelectedGrade = 0;
        }
    }
} 