using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Hierarchy;
using System.Linq;

namespace ProjektSwagger.Models {

    public class Employee {
        public Employee() {
        }

        public Employee(string name, string surname, int wage, string hierarchy) {
            Name = name;
            Surname = surname;
            Hierarchy = new HierarchyId(hierarchy);
            Wage = wage;
        }

        public int Id { get; set; }

        [Required, MaxLength(32)]
        public string Name { get; set; }

        [Required, MaxLength(32)]
        public string Surname { get; set; }

        [Required]
        public HierarchyId Hierarchy { get; set; }

        [Required]
        public int Wage { get; set; }

        public IQueryable<Employee> GetSubordinates(EmployeeContext context) {
            return context.Employees.Where(o => Hierarchy == o.Hierarchy.GetAncestor(1));
        }

        public string NodeReadable() {
            return this.Hierarchy.ToString();
        }

        override public string ToString() {
            return "(" + Id + ") " + Name + " " + Surname + ", " + Wage + " Zl. [" + NodeReadable() + "]";
        }
    }

}
