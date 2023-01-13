using app_grades.Models;

namespace app_grades.Repository
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        void Update(Subject subject);
        IEnumerable<Subject> GetSubjectsByFiliereLevel(string filiere, string level);
        IEnumerable<Subject> GetSubjectsByName(string name);
        IEnumerable<Subject> GetSubjectsByNameCoeff(string name, double coef);
    }
}
