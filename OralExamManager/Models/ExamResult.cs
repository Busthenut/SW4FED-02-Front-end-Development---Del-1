using SQLite;

namespace OralExamManager.Models
{
    [Table("ExamResults")]
    public class ExamResult
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public int ExamId { get; set; }
        
        public int StudentId { get; set; }
        
        public int QuestionNumber { get; set; }
        
        public TimeSpan ActualExaminationTime { get; set; }
        
        [MaxLength(1000)]
        public string Notes { get; set; } = string.Empty;
        
        public int Grade { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
} 