using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Hierarchy;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektSwagger.Models {
    public class EmployeeContextInitializer : CreateDatabaseIfNotExists<EmployeeContext> {
        protected override void Seed(EmployeeContext context) {
            Console.WriteLine("*\tAltering DB\t*");
            context.Database.ExecuteSqlCommand(
                "ALTER TABLE [dbo].[Employees] ADD [ManagerHierarchy] AS ([Hierarchy].[GetAncestor]((1))) PERSISTED");
            context.Database.ExecuteSqlCommand(
                "ALTER TABLE [dbo].[Employees] ADD CONSTRAINT [UK_EmployeeHierarchy] UNIQUE NONCLUSTERED (Hierarchy)");
            context.Database.ExecuteSqlCommand(
                "ALTER TABLE [dbo].[Employees]  WITH CHECK ADD CONSTRAINT [EmployeeManagerHierarchyHierarchyFK] " +
                "FOREIGN KEY([ManagerHierarchy]) REFERENCES [dbo].[Employees] ([Hierarchy])");

            Console.WriteLine("*\tAdding test data to DB\t*");
            context.Employees.Add(new Employee("Mikolaj", "Maciejek", 10000, "/"));
            context.Employees.Add(new Employee("Natalia", "Michalska", 8000, "/1/"));
            context.Employees.Add(new Employee("Wiktoria", "Szewczyk", 8000, "/2/"));
            context.Employees.Add(new Employee("Ania", "Qcia", 6000, "/1/1/"));
            context.Employees.Add(new Employee("Jan", "Paluch", 4000, "/1/1/1/"));
            context.Employees.Add(new Employee("Maciej", "Jedynak", 6500, "/2/1/"));
            context.Employees.Add(new Employee("Jan", "Tomasik", 5500, "/1/2/"));

            Console.WriteLine("*\tAdding procedures to DB\t*");
            context.Database.ExecuteSqlCommand(@"
                CREATE PROCEDURE GetAverageSalary
                AS BEGIN
                    SELECT AVG(Wage) as Wage 
                    FROM Employees
                END
            ");

            context.Database.ExecuteSqlCommand(@"
                CREATE PROCEDURE GetMaxSalary
                AS BEGIN
                    SELECT MAX(Wage) as Wage 
                    FROM Employees
                END
            ");

            Console.WriteLine("*\tSaving context\t*");
            context.SaveChanges();
        }
    }

    public class EmployeeContext : DbContext {
        public EmployeeContext() : base("Data Source=.;Initial Catalog=FirmDB;Integrated Security=True;") { }

        public DbSet<Employee> Employees { get; set; }
    }
}
