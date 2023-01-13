using app_grades.Data;
using app_grades.Models;
using app_grades.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace app_grades.Controllers
{
    public class StudentController : Controller
    {
        public UnitOfWork unitOfWork = new UnitOfWork(new AppGradeContext());
        private readonly IWebHostEnvironment _hostEnvironment;

        public StudentController(IWebHostEnvironment hostEnvironment)
        {

            this._hostEnvironment = hostEnvironment;
        }

        public IActionResult All()
        {
            ViewBag.ERROR = "";
            var list = unitOfWork.Students.GetAll();
            if (!list.Any())
            {
                ViewBag.ERROR = "La liste des étudiants est vide ! ";
            }


            return View("students", list);


        }
        public IActionResult Detail(int id)
        {

            var student = unitOfWork.Students.GetById(id);

            return View("detail", student);

        }
        public IActionResult Grades(int id)
        {

            var grades = new List<Grade>();
            var student = unitOfWork.Students.GetById(id);
            if (student != null)
            {
                grades = (List<Grade>)unitOfWork.Grades.GetGradesByCinStudent(student.Cin);

                ViewBag.Student = student;

            }




            return View("student_grades", grades);

        }

        [HttpGet]
        public IActionResult Filter(string searchString)
        {
            ViewData["search"] = searchString;
            ViewBag.ERROR = "";

            var students = from s in unitOfWork.Students.GetAll() select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = unitOfWork.Students.GetStudentsContainingFirstNameLastNameCin(searchString);
            }

            if (!students.Any())
            {
                ViewBag.ERROR = "La liste des étudiants est vide ! ";
            }

            return View("students", students);

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
        public IActionResult AddOrEdit(int id = 0)
        {

            ViewBag.levels = GetLevels();

            ViewBag.filieres = GetFilieres();

            if (id == 0)
            {
                return View("add_student", new Student());
            }
            else
            {
                return View("add_student", unitOfWork.Students.GetById(id));
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("StudentId,Photo,ImageFile,FirstName,LastName,DateBirth,Email,Cin,Filiere,Level,Phone")] Student s)
        {
            ViewBag.levels = GetLevels();
            ViewBag.filieres = GetFilieres();


            var studentsCin = unitOfWork.Students.GetStudentsByCin(s.Cin);
            if (ModelState.IsValid)
            {
                ViewBag.ERROR = "";



                if (s.ImageFile != null)
                {


                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    System.Diagnostics.Debug.WriteLine("root" + wwwRootPath);
                    string fileName = Path.GetFileNameWithoutExtension(s.ImageFile.FileName);
                    System.Diagnostics.Debug.WriteLine("file" + fileName);
                    string extension = Path.GetExtension(s.ImageFile.FileName);

                    s.Photo = fileName = fileName + "_" + DateTime.Now.ToString("yymmssfff") + extension;

                    string path = Path.Combine(wwwRootPath + "/images/student/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await s.ImageFile.CopyToAsync(fileStream);
                    }

                }

                if (s.StudentId == 0)
                {
                    if (!studentsCin.Any())

                    {
                        unitOfWork.Students.Add(s);
                        unitOfWork.Complete();
                        TempData["success"] = "l'étudiant(e) est ajouté(e) avec succés";
                        return RedirectToAction("All");
                    }
                    else
                    {
                        ViewBag.ERROR = "Désolé , ce CIN existe déja !";
                    }
                }
                else
                {
                    var u2 = new UnitOfWork(new AppGradeContext());
                    var existingstudent = u2.Students.GetById(s.StudentId);
                    if (existingstudent.Cin == s.Cin || existingstudent.Cin != s.Cin && !studentsCin.Any())
                    {

                        unitOfWork.Students.Update(s);
                        unitOfWork.Complete();
                        TempData["success"] = "l'étudiant(e) est modifié(e) avec succés";
                        return RedirectToAction("All");
                    }
                    else
                    {
                        ViewBag.ERROR = "Désolé , ce CIN existe déja !";
                    }

                }




            }
            return View("add_student", s);
        }





        public IActionResult Delete(int id)
        {
            var student = unitOfWork.Students.GetById(id);
            if (student == null)
            {
                return NotFound();
            }
            unitOfWork.Students.Remove(student);
            unitOfWork.Complete();
            TempData["success"] = "l'étudiant(e) est supprimé(e) avec succés ";
            return RedirectToAction("All");
        }

        public IActionResult Average(int id)
        {
            Student student = unitOfWork.Students.GetById(id);
            // ViewBag.Average = unitOfWork.Grades.ComputeAverageByCinFirstNameLastName(student.Cin, student.LastName, student.FirstName);
            ViewBag.Average = unitOfWork.Grades.ComputeAverageByStudentId(id);
            return View("student_average", student);
        }
        public IActionResult Averages(string? filiere, string? level)
        {
            ViewBag.ERROR = "";
            ViewBag.levels = GetLevels();
            ViewBag.filieres = GetFilieres();


            Dictionary<Student, double> dict =new Dictionary<Student, double>();

            if (filiere != null && level !=null && filiere != "none" && level != "none")
            {
                ViewBag.filiere = filiere;
                ViewBag.level = level;
                ViewData["searchFiliere"] =filiere;
                ViewData["searchLevel"] = level;
                List<Student> students = unitOfWork.Students.GetStudentsByFiliereLevel(filiere, level).ToList();



                foreach (Student student in students)
                {
                    double average = unitOfWork.Grades.ComputeAverageByStudentId(student.StudentId);
                    dict.Add(student, average);
                }

                var ordered = dict.OrderBy(x => x.Value);
            }
            if (dict.Any() == false)
            {
                ViewBag.ERROR = "La liste des moyennes est vide ! Veuillez saisir la filiere et le niveau d'études .";

            }


            return View("studentsByaverage",dict);
        }

    }
}
