using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Identity.API.Helpers.Identity
{
    public class User
    {
        public int EmployeeId { get; set; }
        [Display(Name = "Username")]
        public string LoginName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string EmpType { get; set; }
        public string ID { get; set; }
        public string Department { get; set; }
        [Display(Name = "Designation")]
        public string EmpTitle { get; set; }
        public bool IsDeleted { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string DeptName { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string Notes { get; set; }
        public int FacilityID { get; set; }
        public int? EmpTypeID { get; set; }
        public int? EmpTitleID { get; set; }
        public int? DeptID { get; set; }
        public bool? AD_IsActive { get; set; }
        public string AD_Status { get; set; }
        public Guid? AD_GUID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string EmployeeUID { get; set; }
        public string Misc { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? AD_SyncDate { get; set; }
        public bool IsNewGuid { get; set; }
        public List<string> Groups { get; set; }
    }
}
