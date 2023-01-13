using app_grades.Data;
using app_grades.Models;
using app_grades.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace app_grades.Controllers
{
    public class SubjectController : Controller
    {



        public UnitOfWork unitOfWork = new UnitOfWork(new AppGradeContext());



        public IActionResult all()
        {
            var list = unitOfWork.Subjects.GetAll();
            ViewBag.levels = GetLevels();
            ViewBag.filieres = GetFilieres();

            return View("subjects", list);

        }

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

        [HttpGet]
        public IActionResult Filter(string filiere, string level)
        {
            ViewBag.levels = GetLevels();
            ViewBag.filieres = GetFilieres();

            ViewData["searchFiliere"] = filiere;
            ViewData["searchLevel"] = level;

            var subjetcts = from s in unitOfWork.Subjects.GetAll() select s;
            if (filiere!="none" && level != "none")
            {
                subjetcts = unitOfWork.Subjects.GetSubjectsByFiliereLevel(filiere, level);
            }

            return View("subjects", subjetcts);

        }


        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            ViewBag.ERROR = "";
            if (id == 0)
            {
                return View("add_subject", new Subject());
            }
            else
            {
                return View("add_subject", unitOfWork.Subjects.GetById(id));
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(Subject s)
        {
            if (ModelState.IsValid)
            {
                ViewBag.ERROR = "";
               
                if (s.SubjectId == 0)
                {
                    if (!unitOfWork.Subjects.GetSubjectsByNameCoeff(s.SubjectName, (double)s.Coefficient).Any())
                    {
                        unitOfWork.Subjects.Add(s);
                        unitOfWork.Complete();
                        TempData["success"] = "la matière est ajoutée avec succés";
                        return RedirectToAction("All");
                    }
                    else
                    {
                        ViewBag.ERROR = "Désolé , cette matière existe déja  !";
                    }
                }
                else
                {
                    var u2 = new UnitOfWork(new AppGradeContext());
                    var existingsubject = u2.Subjects.GetById(s.SubjectId);
                    if ((existingsubject.SubjectName==s.SubjectName && existingsubject.Coefficient==s.Coefficient) || ((existingsubject.SubjectName!=s.SubjectName || existingsubject.Coefficient!=s.Coefficient )&& !unitOfWork.Subjects.GetSubjectsByNameCoeff(s.SubjectName, (double)s.Coefficient).Any()))
                    {
                        unitOfWork.Subjects.Update(s);
                        unitOfWork.Complete();
                        TempData["success"] = "la matière est modifié(e) avec succés";
                        return RedirectToAction("All");
                    }
                    else
                    {
                        ViewBag.ERROR = "Désolé , cette matière existe déja  !";
                    }

                }
              


            }
            return View("add_subject", s);
        }
        public IActionResult Delete(int id)
        {
            var subject = unitOfWork.Subjects.GetById(id);
            if (subject == null)
            {
                return NotFound();
            }
            unitOfWork.Subjects.Remove(subject);
            unitOfWork.Complete();
            TempData["success"] = "la matière est supprimée avec succés ";
            return RedirectToAction("all");
        }






    }
}
