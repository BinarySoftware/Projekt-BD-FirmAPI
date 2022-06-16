//using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Hierarchy;
using System.Linq;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ProjektSwagger.Models {

    //public class Employee {
    //    private byte[] level;
    //    private SqlHierarchyId levelSql;

    //    public int Id { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string Position { get; set; }
    //    public int Salary { get; set; }

    //    [Required]
    //    [Index("IX_UniqueNode", IsUnique = true)]
    //    [MaxLength(892)]
    //    public byte[] Level {
    //        get => level;
    //        set {
    //            level = value;
    //            levelSql = level.ToSqlHierarchyId();
    //        }
    //    }

    //    [NotMapped]
    //    public string LevelHumanReadable {
    //        get => levelSql.ToString();
    //        set => Level = value.ToSqlHierarchyId().ToByteArray();
    //    }

    //    override public string ToString() {
    //        return "" + Id + "\t|" + FirstName + "\t|" + LastName + "\t|" + levelSql + "\t|" + Position + "\t|" + Salary + "\t|";
    //    }
    //}

    public class Employee {
        public int EmployeeId { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public HierarchyId Node { get; set; }

        public IQueryable<Employee> GetSubordinates(EmployeeContext context) {
            return context.Employees.Where(o => Node == o.Node.GetAncestor(1));
        }

        public string NodeReadable() {
            return this.Node.ToString();
        }
    }

}
