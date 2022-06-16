using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Hierarchy;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektSwagger.Models {
    public class EmployeeContextInitializer : CreateDatabaseIfNotExists<EmployeeContext> {
        protected override void Seed(EmployeeContext context) {
            context.Database.ExecuteSqlCommand(
                "ALTER TABLE [dbo].[Employees] ADD [ManagerNode] AS ([Node].[GetAncestor]((1))) PERSISTED");
            context.Database.ExecuteSqlCommand(
                "ALTER TABLE [dbo].[Employees] ADD CONSTRAINT [UK_EmployeeNode] UNIQUE NONCLUSTERED (Node)");
            context.Database.ExecuteSqlCommand(
                "ALTER TABLE [dbo].[Employees]  WITH CHECK ADD CONSTRAINT [EmployeeManagerNodeNodeFK] " +
                "FOREIGN KEY([ManagerNode]) REFERENCES [dbo].[Employees] ([Node])");
            context.Employees.Add(new Employee { Name = "Root", Node = new HierarchyId("/") });
            context.Employees.Add(new Employee { Name = "Emp1", Node = new HierarchyId("/1/") });
            context.Employees.Add(new Employee { Name = "Emp2", Node = new HierarchyId("/2/") });
            context.Employees.Add(new Employee { Name = "Emp3", Node = new HierarchyId("/1/1/") });
            context.Employees.Add(new Employee { Name = "Emp4", Node = new HierarchyId("/1/1/1/") });
            context.Employees.Add(new Employee { Name = "Emp5", Node = new HierarchyId("/2/1/") });
            context.Employees.Add(new Employee { Name = "Emp6", Node = new HierarchyId("/1/2/") });
            context.SaveChanges();
        }
    }

    public class EmployeeContext : DbContext {
        public EmployeeContext() : base("Data Source=.;Initial Catalog=FirmDBTest;Integrated Security=True;") { }

        public DbSet<Employee> Employees { get; set; }
    }
}
