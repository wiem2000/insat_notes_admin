using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app_grades.Models
{
    [Table("Student")]
    public class Student
    {
        
        [Key]
        public int StudentId { get; set; }
        
        
        
        
        [DisplayName("CIN"),MinLength(8,ErrorMessage ="Cin invalide !"), MaxLength(8, ErrorMessage = "CIN invalide !"), Required(ErrorMessage ="Veuillez saisir le CIN de l'étudiant ! ")] 
        public string? Cin { get; set; }
        
        
        
        
        [DisplayName("Prénom") ,Required(ErrorMessage = "Veuillez saisir le prénom de l'étudiant ! ")]
        public string? FirstName { get; set; }

        
        
        
        
        [DisplayName("Nom"),Required(ErrorMessage = "Veuillez saisir le nom de l'étudiant ! ")]
        public string? LastName { get; set; }
        
        
        
        [DisplayName("Date de naissance")]
        public DateTime DateBirth { get; set; }
        
        
        [DisplayName("Tel")]
        public string? Phone { get; set; }
       
        
        [EmailAddress(ErrorMessage = "Veuillez saisir une adresse mail valide ! "), Required(ErrorMessage = "Veuillez saisir l'email de l'étudiant ! ")]
        public string? Email { get; set; }
        
        
       
        
        [DisplayName("Filière"),Required(ErrorMessage = "Veuillez saisir la filière de l'étudiant ! ")]
        public string? Filiere { get; set; }
       
        
        
        
        [DisplayName("Niveau d'études "),Required(ErrorMessage = "Veuillez saisir le niveau de l'étudiant ! ")]
        public string? Level { get; set; }




        public string? Photo { get; set; }

       
        
        //-------------------photo upload---------------//
         
        [NotMapped]
        [DisplayName("Choisir une image")]
        public IFormFile? ImageFile { get; set; }
        
        //----------------------------------------------//


        public virtual ICollection<Grade>? Grades { get; set; }



    }
}
