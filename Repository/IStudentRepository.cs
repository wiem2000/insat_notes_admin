using app_grades.Models;

namespace app_grades.Repository
{
    public interface IStudentRepository: IRepository<Student>
    {
        void Update(Student student);
       
        IEnumerable<Student> GetStudentsByFiliereLevel (string filiere,string niveau);
        IEnumerable<Student> GetStudentsByFullName(string firstname,string lastname);
        IEnumerable<Student> GetStudentsByCin(string cin );
        IEnumerable<Student> GetStudentsContainingFirstNameLastNameCin(string searchString);

    }
}
