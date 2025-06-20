using SQLite;

namespace OralExamManager.Models
{
    [Table("Exams")]
    public class Exam
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        [MaxLength(100)]
        public string ExamTerm { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string CourseName { get; set; } = string.Empty;
        
        public DateTime Date { get; set; }
        
        public int NumberOfQuestions { get; set; }
        
        public int ExaminationTimeMinutes { get; set; }
        
        public TimeSpan StartTime { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public bool IsCompleted { get; set; } = false;
    }
} 