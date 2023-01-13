using app_grades.Data;
using app_grades.Models;
using Microsoft.EntityFrameworkCore;

namespace app_grades.Repository
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private readonly AppGradeContext _context;
        public StudentRepository(AppGradeContext context) : base(context)
        {
            this._context = context;
        }

        public void Update(Student student)
        {

            _context.Students.Update(student);
        }
        public IEnumerable<Student> GetStudentsByFiliereLevel(string filiere, string niveau)
        {
            var list = _context.Students.Where(s => s.Filiere.ToUpper() == filiere.ToUpper() && s.Level == niveau);
            return list;


            //throw new NotImplementedException();
        }

        public IEnumerable<Student> GetStudentsByFullName(string firstname, string lastname)
        {
            var list = _context.Students.Where(s => s.FirstName.ToUpper() == firstname.ToUpper() && s.LastName.ToUpper() == lastname.ToUpper());
            return list;
            //throw new NotImplementedException();
        }
        public IEnumerable<Student> GetStudentsByCin(string cin)
        {
            var student = _context.Students.Where(s => s.Cin == cin).AsNoTracking().ToList();
            return student;

            //throw new NotImplementedException();
        }
        public IEnumerable<Student> GetStudentsContainingFirstNameLastNameCin(string searchString)
        {
            var student = _context.Students.Where(s => s.FirstName.ToUpper().Contains(searchString.ToUpper())
            || s.LastName.ToUpper().Contains(searchString.ToUpper())
            || s.Cin.Contains(searchString) );
            return student;

            //throw new NotImplementedException();
        }


    }
}
