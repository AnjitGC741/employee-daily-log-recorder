using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace employeeDailyTaskRecorder.Models
{
    public class Record
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Task { get; set; }
        public DateTime TaskPerformedDate { get; set; }
        public string Ipaddress { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [NotMapped]
        public bool CanUpdate => TaskPerformedDate.Date == DateTime.Now.Date;
        //login user id matches employee id
    }
}
