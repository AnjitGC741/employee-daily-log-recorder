using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace employeeDailyTaskRecorder.Models

{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [MinLength(5)]
        public string Password { get; set; }
        public EnumEmployeeType EmpType { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool Status { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public string ProfileImg { get; set; }
        public ICollection<Record> Records { get; set; }
        [NotMapped]
        public bool IsAdmin => EmpType == EnumEmployeeType.Admin;
        [NotMapped]
        public bool IsUser => !IsAdmin;
    }
    public enum EnumEmployeeType
    {
        Admin = 1,
        Employee = 2
    }
    public class VMAdminIndex
    {
        public bool CanAddTask { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeEmail { get; set; }
        public string SearchTerm { get; set; }
        public string strToDate => ToDate.ToString("yyyy/MM/dd");
        public string strFromDate => FromDate.ToString("yyyy/MM/dd");
        public IList<Record> TaskList { get; set; } = new List<Record>();
        public IList<Employee> EmployeeList
        { get; set; } = new List<Employee>();
        public VMAdminIndex()
        {
            FromDate = DateTime.Now.AddMonths(-1);
            ToDate = DateTime.Now;
        }
    }
}
