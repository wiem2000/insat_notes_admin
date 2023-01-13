using app_grades.Data;
using app_grades.Models;
using Microsoft.EntityFrameworkCore;

namespace app_grades.Repository
{
    public class GradeRepository : Repository<Grade>, IGradeRepository
    {
        private readonly AppGradeContext _context;
        public GradeRepository(AppGradeContext context) : base(context)
        {
            this._context = context;
        }


        public void Update(Grade grade)
        {
            //_context.Entry(subject).State = EntityState.Detached;
            _context.Grades.Update(grade);
        }
   
            public double ComputeAverageByCinFirstNameLastName(string cin, string lastname, string firstname)
            {

                var subjects = (from s in _context.Subjects

                                join g in _context.Grades on s.SubjectId equals g.SubjectId
                                join stu in _context.Students on g.StudentId equals stu.StudentId
                                where stu.Cin == cin && stu.FirstName == firstname && stu.LastName == lastname
                                select s).ToList();
                double GradesSum = 0;
                double CoefSum = 0;
                foreach (var subject in subjects)
                {
                    var mark = (from g in _context.Grades
                                join s in _context.Subjects on g.SubjectId equals s.SubjectId
                                join stu in _context.Students on g.StudentId equals stu.StudentId
                                where stu.Cin == cin && subject.SubjectId == g.SubjectId && stu.LastName == lastname && stu.FirstName == firstname
                                select g).First();
                    var coefficient = (from s in _context.Subjects
                                       where s.SubjectId == subject.SubjectId
                                       select s).First();
                    GradesSum += (double)coefficient.Coefficient * mark.Mark;
                    CoefSum += (double)coefficient.Coefficient;
                }
                double average = GradesSum / CoefSum;
                return average;
            }

        public double ComputeAverageByStudentId(int studentId)
        {
            double average = 0;
            double sumcoef = 0;

            SubjectRepository sr = new SubjectRepository(_context);
            

            Student s =  _context.Students.Where(s=>s.StudentId == studentId).ToList().FirstOrDefault()  ;
            if(s != null)
            {

                var subjects = sr.GetSubjectsByFiliereLevel(s.Filiere, s.Level);
                foreach(var subject in subjects)
                {
                    double mark = 0;
                    var grade =_context.Grades.Where(g => g.StudentId == studentId && g.SubjectId==subject.SubjectId).FirstOrDefault();
                    if(grade != null){mark = grade.Mark;}
                    average = (double)(average + mark * subject.Coefficient);
                    sumcoef += (double)subject.Coefficient;
                }

                average=average / sumcoef;
            }


            return average;

        }




            public IEnumerable<Grade> GetGradesByCinStudent(string cin)
        {
            var list = _context.Subjects.ToList();


            var grades = (from g in _context.Grades

                          join stud in _context.Students on g.StudentId equals stud.StudentId
                          where stud.Cin == cin
                          select g).OrderBy(grade => grade.Subject.SubjectName).ToList();

            return grades;
            throw new NotImplementedException();
        }

        public IEnumerable<Grade> GetGradesByFiliereLevelSubject(string filiere, string level, int subjectId)
        {
            var list= _context.Students.ToList();

            var grades = (from g in _context.Grades

                      join stud in _context.Students on g.StudentId equals stud.StudentId
                     
                      where stud.Filiere.ToUpper()==filiere.ToUpper() && stud.Level==level && g.SubjectId==subjectId
                      select g).ToList();

            return grades;
            //throw new NotImplementedException();
        }
        public IEnumerable<Grade> GetGradesByStudentSubject(int studentId,int subjectId)
        {
            var grades = new List<Grade>();


            grades = this._context.Grades.Where( s => s.StudentId==studentId && s.SubjectId == subjectId ).ToList();

            return grades;

        }
    }
}
