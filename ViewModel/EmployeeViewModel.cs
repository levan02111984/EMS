using EMS.Models;

namespace EMS.ViewModel
{
    public class EmployeeViewModel
    {
        public EmployeeViewModel() { }
        public Employee Employee { get; set; }

        public string DepartmentName { get; set; }

        public string CountryName { get; set; }

        public string DesignationName { get; set; }

    }
}
