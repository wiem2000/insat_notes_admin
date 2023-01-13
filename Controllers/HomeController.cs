using app_grades.Models;
using app_grades.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using app_grades.Repository;

namespace app_grades.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var dbContext = new AppGradeContext();


            //dbContext.Database.EnsureDeleted();

            /*
             dbContext.Database.EnsureCreated();


                dbContext.Students.AddRange(new Student[]
                    {
                              new Student{ Cin="12345678", FirstName="ahmed" , LastName="ben aaze",Phone="20156",Email="ahmed@rjof.com",Filiere="gl",Level="3" },
                              new Student{ Cin="78945612", FirstName="yassine" , LastName="ellouz",Phone="20156",Email="yassine@rjof.com",Filiere="ch",Level="2" },
                              new Student{ Cin="45612378", FirstName="mariem" , LastName="ben mouhamed",Phone="20156",Email="marimej@rjof.com",Filiere="iia",Level="2" },
                              new Student{ Cin="85236974", FirstName="lina" , LastName="jaziri",Phone="20156",Email="linaj@rjof.com",Filiere="gl",Level="3" },
                              new Student{ Cin="36974112", FirstName="oussama" , LastName="salama",Phone="20156",Email="oussama@rjof.com",Filiere="ch",Level="3" },
                    });
                dbContext.SaveChanges();

                dbContext.Subjects.AddRange(new Subject[]
                 {
                              new Subject{ SubjectName="math",Coefficient=1.5 },
                              new Subject{ SubjectName="algo",Coefficient=2},
                              new Subject{ SubjectName="arabe",Coefficient=1},
                              new Subject{ SubjectName="anglais",Coefficient=1},
                 });
                dbContext.SaveChanges();
                dbContext.Grades.AddRange(new Grade[]
                  {
                       new Grade{StudentId=1,SubjectId=1,Mark=15 },new Grade{StudentId=1,SubjectId=2,Mark=8 },new Grade{StudentId=1,SubjectId=3,Mark=10 },
                       new Grade{StudentId=2,SubjectId=1,Mark=17},new Grade{StudentId=2,SubjectId=2,Mark=5.5 }, new Grade{StudentId=2,SubjectId=3,Mark=16.75 },
                       new Grade{StudentId=3,SubjectId=1,Mark=13.5 },new Grade{StudentId=3,SubjectId=2,Mark=15 },new Grade{StudentId=3,SubjectId=3,Mark=10.25 },
                       new Grade{StudentId=4,SubjectId=1,Mark=12.5},new Grade{StudentId=4,SubjectId=2,Mark=7.75 }, new Grade{StudentId=4,SubjectId=3,Mark=11},
                       new Grade{StudentId=5,SubjectId=1,Mark=5.5},new Grade{StudentId=5,SubjectId=2,Mark=17.75 }, new Grade{StudentId=5,SubjectId=3,Mark=9.5},new Grade{StudentId=5,SubjectId=4,Mark=19.5},


                  });
                dbContext.SaveChanges();


            */
            


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}