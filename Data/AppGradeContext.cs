using app_grades.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace app_grades.Data
{
    public class AppGradeContext:DbContext
    {   public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        /*
        public static AppGradeContext? _context;

        private AppGradeContext(DbContextOptions o):base(o) { }

        private static AppGradeContext Instantiate_AppGradeContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppGradeContext>();
            optionsBuilder.UseSqlite(@"Data Source = Database\AppDatabase.db");


            return new AppGradeContext(optionsBuilder.Options);
        }
        public static AppGradeContext Instance()
        {
            if (_context == null)
            {
                _context = Instantiate_AppGradeContext();

            }
            return _context;
        }
        */

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source = Database\AppDatabase.db", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);

            });
            base.OnConfiguring(optionsBuilder);
        }
 

    }
}
