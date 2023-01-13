using app_grades.Data;

namespace app_grades.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppGradeContext context;
        public IStudentRepository Students { get; private set; }
        public ISubjectRepository Subjects { get; private set; }
        public IGradeRepository Grades { get; private set; }

        public UnitOfWork(AppGradeContext c)
        {
            this.context = c;
            Students = new StudentRepository(c);
            Subjects = new SubjectRepository(c);
            Grades = new GradeRepository(c);
        }
        public bool Complete()
        {
            try
            {
                int result = context.SaveChanges();
                if (result > 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
