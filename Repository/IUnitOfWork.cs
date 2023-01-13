namespace app_grades.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository Students { get; }
        IGradeRepository Grades { get; }
        ISubjectRepository Subjects { get; }
        bool Complete();
    }
}
