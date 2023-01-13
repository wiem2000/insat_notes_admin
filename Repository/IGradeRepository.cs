using app_grades.Models;

namespace app_grades.Repository
{
    public interface IGradeRepository : IRepository<Grade>
    {
        void Update(Grade grade);
        IEnumerable<Grade> GetGradesByFiliereLevelSubject(string filiere,string level,int subjectId);
        IEnumerable<Grade> GetGradesByCinStudent(string cin);
        double ComputeAverageByCinFirstNameLastName(string cin, string lastname, string firstname);
        double ComputeAverageByStudentId(int studentId);
        IEnumerable<Grade> GetGradesByStudentSubject(int studentId, int subjectId);
    }
}
