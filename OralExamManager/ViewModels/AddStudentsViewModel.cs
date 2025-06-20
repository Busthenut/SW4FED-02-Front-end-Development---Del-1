using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OralExamManager.Models;
using OralExamManager.Services;

namespace OralExamManager.ViewModels
{
    public partial class AddStudentsViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private readonly int _examId;

        [ObservableProperty]
        private ObservableCollection<Student> _students = new();

        [ObservableProperty]
        private string _studentId = string.Empty;

        [ObservableProperty]
        private string _studentName = string.Empty;

        public AddStudentsViewModel(DatabaseService databaseService, int examId)
        {
            _databaseService = databaseService;
            _examId = examId;
            _ = LoadStudents();
        }

        [RelayCommand]
        private async Task LoadStudents()
        {
            var students = await _databaseService.GetStudentsForExamAsync(_examId);
            Students.Clear();
            foreach (var student in students)
            {
                Students.Add(student);
            }
        }

        [RelayCommand]
        private async Task AddStudent()
        {
            if (string.IsNullOrWhiteSpace(StudentId))
            {
                var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
                if (mainPage != null)
                {
                    await mainPage.DisplayAlert("Error", "Please enter a student ID", "OK");
                }
                return;
            }

            if (string.IsNullOrWhiteSpace(StudentName))
            {
                var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
                if (mainPage != null)
                {
                    await mainPage.DisplayAlert("Error", "Please enter a student name", "OK");
                }
                return;
            }

            var student = new Student
            {
                ExamId = _examId,
                StudentId = StudentId,
                Name = StudentName,
                Order = Students.Count + 1
            };

            await _databaseService.SaveStudentAsync(student);
            Students.Add(student);
            
            StudentId = string.Empty;
            StudentName = string.Empty;
        }

        [RelayCommand]
        private async Task RemoveStudent(Student student)
        {
            if (student == null) return;

            var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
            if (mainPage != null)
            {
                var result = await mainPage.DisplayAlert("Confirm", 
                    $"Remove {student.Name} from the exam?", "Yes", "No");
                
                if (result)
                {
                    await _databaseService.DeleteStudentAsync(student);
                    Students.Remove(student);
                    
                    // Reorder remaining students
                    for (int i = 0; i < Students.Count; i++)
                    {
                        Students[i].Order = i + 1;
                        await _databaseService.SaveStudentAsync(Students[i]);
                    }
                }
            }
        }

        [RelayCommand]
        private async Task Save()
        {
            var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
            if (Students.Count == 0)
            {
                if (mainPage != null)
                {
                    await mainPage.DisplayAlert("Warning", 
                        "No students added. Add at least one student to continue.", "OK");
                }
                return;
            }

            if (mainPage != null)
            {
                await mainPage.DisplayAlert("Success", 
                    $"{Students.Count} students added successfully", "OK");
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