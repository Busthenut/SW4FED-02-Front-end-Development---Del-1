using SQLite;

namespace OralExamManager.Models
{
    [Table("Students")]
    public class Student
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public int ExamId { get; set; }
        
        [MaxLength(50)]
        public string StudentId { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        public int Order { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
} 