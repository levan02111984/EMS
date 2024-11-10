namespace EMS.Models
{
    public class Employee : UserActivity
    {
        public int Id { get; set; }

        public string EmpNo { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set;}

        public string FullName => $"{FirstName} {LastName}";

        public int PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public int CountryID { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; } 

        public int DepartmentID { get; set; }

        public int DesignationID { get; set; }

    }
}
