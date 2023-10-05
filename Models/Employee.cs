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
        public string ContactNumber { get; set; }
        public EnumEmployeeGender Gender { get; set; }
        public EnumEmployeeStage EmpStage { get; set; }
        public EnumMajorRole EmpRole { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime CurrentStageCompletionDate { get; set; }
        public EnumEmployeeType EmpType { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool Status { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public string ProfileImg { get; set; } = String.Empty;
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
    public enum EnumEmployeeStage
    {
        Internship = 1,
        Probation_Period = 2,
        Contractual = 3
    }
    public enum EnumMajorRole
    {
        CustomerSupport_QA = 1,
        Developer = 2,
        Admin = 3
    }
    public enum EnumEmployeeGender
    {
        Male = 1,
        Female = 2
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
        public string strToDate => ToDate.ToString("yyyy-MM-dd");
        public string strFromDate => FromDate.ToString("yyyy-MM-dd");
        public IList<Record> TaskList { get; set; } = new List<Record>();
        public IList<Employee> EmployeeList { get; set; } = new List<Employee>();
        public VMAdminIndex()
        {
            //  FromDate = DateTime.Now.AddMonths(-1);
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
        }
    }
    public class VMValidatePassword
    {
        public int EmployeeID { get; set; }
        [Required]
        [MinLength(5)]
        public string OldPassword { get; set; }
        [Required]
        [MinLength(5)]
        public string NewPassword { get; set; }
        [Required]
        [MinLength(5)]
        public string ConfirmPassword { get; set; }
        public IList<Employee> EmployeeList { get; set; } = new List<Employee>();
    }
}
