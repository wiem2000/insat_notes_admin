using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app_grades.Models
{
    [Table("Subject")]
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }

       
        
        [DisplayName("Matière"), Required(ErrorMessage = "Veuillez saisir le nom de la matière ! ")]
        public string? SubjectName { get; set; }
        
       
        
        [DisplayName("Coefficient"), Required(ErrorMessage = "Veuillez saisir le coefficient de la matière ! ")]
        public double? Coefficient { get; set; }

       
        
        public virtual ICollection<Grade>? Grades { get; set; }

    }
}
