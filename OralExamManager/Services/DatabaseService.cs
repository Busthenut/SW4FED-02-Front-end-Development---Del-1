using SQLite;
using OralExamManager.Models;

namespace OralExamManager.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "oralexam.db");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Exam>().Wait();
            _database.CreateTableAsync<Student>().Wait();
            _database.CreateTableAsync<ExamResult>().Wait();
        }

        // Exam operations
        public async Task<List<Exam>> GetExamsAsync()
        {
            return await _database.Table<Exam>().OrderByDescending(x => x.CreatedAt).ToListAsync();
        }

        public async Task<Exam> GetExamAsync(int id)
        {
            return await _database.Table<Exam>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveExamAsync(Exam exam)
        {
            if (exam.Id != 0)
                return await _database.UpdateAsync(exam);
            else
                return await _database.InsertAsync(exam);
        }

        public async Task<int> DeleteExamAsync(Exam exam)
        {
            return await _database.DeleteAsync(exam);
        }

        // Student operations
        public async Task<List<Student>> GetStudentsForExamAsync(int examId)
        {
            return await _database.Table<Student>().Where(x => x.ExamId == examId).OrderBy(x => x.Order).ToListAsync();
        }

        public async Task<int> SaveStudentAsync(Student student)
        {
            if (student.Id != 0)
                return await _database.UpdateAsync(student);
            else
                return await _database.InsertAsync(student);
        }

        public async Task<int> DeleteStudentAsync(Student student)
        {
            return await _database.DeleteAsync(student);
        }

        // ExamResult operations
        public async Task<List<ExamResult>> GetExamResultsAsync(int examId)
        {
            return await _database.Table<ExamResult>().Where(x => x.ExamId == examId).ToListAsync();
        }

        public async Task<ExamResult> GetExamResultForStudentAsync(int examId, int studentId)
        {
            return await _database.Table<ExamResult>().Where(x => x.ExamId == examId && x.StudentId == studentId).FirstOrDefaultAsync();
        }

        public async Task<int> SaveExamResultAsync(ExamResult result)
        {
            if (result.Id != 0)
                return await _database.UpdateAsync(result);
            else
                return await _database.InsertAsync(result);
        }

        public async Task<double> GetAverageGradeAsync(int examId)
        {
            var results = await GetExamResultsAsync(examId);
            if (results.Count == 0) return 0;
            return results.Average(x => x.Grade);
        }
    }
} 