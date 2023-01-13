using app_grades.Data;
using app_grades.Models;
using app_grades.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace app_grades.Controllers
{
    public class GradeController : Controller
    {
        public UnitOfWork unitOfWork = new UnitOfWork(new AppGradeContext());

        public List<SelectListItem> GetLevels()
        {
            List<SelectListItem> levels = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "1 ère année" },
            new SelectListItem { Value = "2", Text = "2 ème année" },
            new SelectListItem { Value = "3", Text = "3 ème année"  },
            new SelectListItem { Value = "4", Text = "4 ème année"  },
            new SelectListItem { Value = "5", Text = "5 ème année"  },
        };
            return levels;
        }

        public List<SelectListItem> GetFilieres()
        {
            List<SelectListItem> filieres = new List<SelectListItem>
        {

            new SelectListItem { Value = "MPI", Text = "CYCLE PRÉPARATOIRE INTÉGRÉ MPI" },
            new SelectListItem { Value = "CBA", Text = "CYCLE PRÉPARATOIRE INTÉGRÉ CBA"  },
            new SelectListItem { Value = "IIA", Text = "Informatique Industrielle & Automatique (IIA)"  },
            new SelectListItem { Value = "IMI", Text = "Instrumentation et Maintenance Industrielle (IMI)"  },
            new SelectListItem { Value = "RT", Text = "Réseaux Informatiques et Télécommunications (RT)"  },
            new SelectListItem { Value = "GL", Text = "Génie Logiciel (GL)"  },
            new SelectListItem { Value = "CH", Text = "Chimie Industrielle (CH)"  },
            new SelectListItem { Value = "BIO", Text = "Biologie industrielle (BIO)"  },
        };
            return filieres;

        }

        public Dictionary<int, string>GetSubjects(){
            var subjects = new Dictionary<int, string>();
            foreach (var s in unitOfWork.Subjects.GetAll()) { subjects.Add(s.SubjectId, s.SubjectName + " / Coef : " + s.Coefficient); }
            return subjects;
        }
        public Dictionary<int, string> GetStudents()
        {
            var students = new Dictionary<int, string>();
            foreach (var s in unitOfWork.Students.GetAll()) { students.Add(s.StudentId, "CIN :" + s.Cin + " / " + s.FirstName + " " + s.LastName); }
            return students;
        }


        public IActionResult Find(string? filiere, string? level, int? subjectId)
        {
            ViewBag.ERROR = "";
            ViewBag.levels = GetLevels();
            ViewBag.filieres = GetFilieres(); 
            ViewBag.subjects = new SelectList(GetSubjects(), "Key", "Value");


            var grades = new List<Grade>();
            if (filiere !="none" && level != "none" && subjectId!=null)
            {
                var s = unitOfWork.Subjects.GetById((int)subjectId);
                if (s !=null)
                {
                    
                    
                    ViewBag.filiere = filiere;
                    ViewBag.level = level;
                    ViewBag.Subject = s;

                    ViewData["searchFiliere"] = filiere;
                    ViewData["searchLevel"] = level;
                    ViewData["searchSubject"] = subjectId;

                    grades = unitOfWork.Grades.GetGradesByFiliereLevelSubject(filiere, level,(int) subjectId).ToList();
                }

            }
            if (grades.Any() == false)
            {
                ViewBag.ERROR = " La Liste des notes est vide ! Veuillez saisir la filiere , le niveau et la matiere .";

            }


            return View("gradesByFiliereLevelSubject", grades);
        }

        
        
        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {

            //ViewBag.students = new SelectList(unitOfWork.Students.GetAll(), nameof(Student.StudentId), nameof(Student.Cin) );

          
            ViewBag.subjects = new SelectList(GetSubjects(), "Key", "Value");
            ViewBag.students = new SelectList(GetStudents(), "Key", "Value");
            ViewBag.ERROR = "";

            if (id == 0)
            {
                return View("add_grade", new Grade());
            }
            else
            {
                return View("add_grade", unitOfWork.Grades.GetById(id));
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(Grade g)
        {
            
            ViewBag.subjects = new SelectList(GetSubjects(), "Key", "Value");
            ViewBag.students = new SelectList(GetStudents(), "Key", "Value");



            if (ModelState.IsValid)
            {
                ViewBag.ERROR = "";

                if (g.GradeId == 0)
                {
                    if (!unitOfWork.Grades.GetGradesByStudentSubject(g.StudentId,g.SubjectId).Any())
                    {
                        unitOfWork.Grades.Add(g);
                        unitOfWork.Complete();
                        TempData["success"] = "la note est ajoutée avec succés";
                        return RedirectToAction("Find", "Grade", new
                        {
                            filiere = unitOfWork.Students.GetById(g.StudentId).Filiere,
                            level = unitOfWork.Students.GetById(g.StudentId).Level,
                            subjectId = unitOfWork.Subjects.GetById(g.SubjectId).SubjectId
                        });
                   
                    }
                    else
                    {
                        ViewBag.ERROR = "Désolé , l'étudiant a déja une note dans cette matière  !";
                    }

                }
                else
                {
                    var u2 = new UnitOfWork(new AppGradeContext());
                    var existingGrade = u2.Grades.GetById(g.GradeId);
                    if ((existingGrade.StudentId == g.StudentId && existingGrade.SubjectId==g.SubjectId) || ((existingGrade.StudentId != g.StudentId || existingGrade.SubjectId != g.SubjectId) && !unitOfWork.Grades.GetGradesByStudentSubject(g.StudentId, g.SubjectId).Any()))
                    {
                        unitOfWork.Grades.Update(g);
                        unitOfWork.Complete();
                        TempData["success"] = "la note est modifiée avec succés";
                        return RedirectToAction("Find", "Grade", new
                        {
                            filiere = unitOfWork.Students.GetById(g.StudentId).Filiere,
                            level = unitOfWork.Students.GetById(g.StudentId).Level,
                            subjectId = unitOfWork.Subjects.GetById(g.SubjectId).SubjectId
                        });
                    }
                    else
                    {
                        ViewBag.ERROR = "Désolé , l'étudiant a déja une note dans cette matière  !";
                    }
                }


               


            }
            return View("add_grade", g);
        }



        public IActionResult Delete(int id)
        {
            var grade = unitOfWork.Grades.GetById(id);
            if (grade == null)
            {
                return NotFound();
            }
            unitOfWork.Grades.Remove(grade);
            unitOfWork.Complete();
            TempData["success"] = "la note est supprimée avec succés ";
            return RedirectToAction("Find", "Grade", new
            {
                filiere = unitOfWork.Students.GetById(grade.StudentId).Filiere,
                level = unitOfWork.Students.GetById(grade.StudentId).Level,
                subjectId = unitOfWork.Subjects.GetById(grade.SubjectId).SubjectId
            });
        }






    }
}
