using app_grades.Data;
using app_grades.Models;
using Microsoft.EntityFrameworkCore;

namespace app_grades.Repository
{
    public class SubjectRepository : Repository<Subject>, ISubjectRepository
    {
        private readonly AppGradeContext _context;
        public SubjectRepository(AppGradeContext context) : base(context)
        {
            this._context = context;
        }

        public void Update(Subject subject)
        {
            //_context.Entry(subject).State = EntityState.Detached;
            _context.Subjects.Update(subject);
        }
        public IEnumerable<Subject> GetSubjectsByFiliereLevel(string filiere, string level)
        {
            var subject = new List<Subject>();


            subject = (from s in _context.Subjects
                       join g in _context.Grades on s.SubjectId equals g.SubjectId
                       join stud in _context.Students on g.StudentId equals stud.StudentId
                       where stud.Filiere.ToUpper() == filiere.ToUpper() && stud.Level == level
                       select s).Distinct().ToList();

            return subject;

            throw new NotImplementedException();
        }
        public IEnumerable<Subject> GetSubjectsByName(string name)
        {
            var subject = new List<Subject>();


            subject = this._context.Subjects.Where(s => s.SubjectName.ToUpper() == name.ToUpper()).ToList();

            return subject;

            throw new NotImplementedException();
        }


        public IEnumerable<Subject> GetSubjectsByNameCoeff(string name,double coef)
        {
            var subjects = new List<Subject>();


            subjects = this._context.Subjects.Where(s => s.SubjectName.ToUpper() == name.ToUpper() && s.Coefficient==coef).ToList();

            return subjects;

        }


    }
}
