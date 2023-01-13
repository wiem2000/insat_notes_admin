using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app_grades.Models
{
    [Table("Grade")]
    public class Grade
    {
        [Key, DisplayName("ID note")]
        public int GradeId { get; set; }

        
        
        [Required(ErrorMessage = "Veuillez choisir la matière ! "), DisplayName("Matière"),ForeignKey("SubjectId")] 
        public int SubjectId { get; set; }

        
        
        
        [Required(ErrorMessage = "Veuillez chosiir l'étudiant ! "), DisplayName("Etudiant"),ForeignKey("StudentId")]
         public int StudentId { get; set; }

        
        public virtual Student? Student { get; set; }

        
       
        public virtual Subject? Subject { get; set; }


       
        [Range(0,20,ErrorMessage = "Veuillez saisir une note entre 0 et 20 ! "), DisplayName("Note")]
        public double  Mark { get; set; }
        
     
    }
}
