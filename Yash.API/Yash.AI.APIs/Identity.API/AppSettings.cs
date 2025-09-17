namespace Identity.API
{
    public class AppSettings
    {
        public string EmployeeId { get; set; }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string ActiveDirectoryServer { get; set; }
        public string Key { get; set; }

        public int TokenExpiryTime { get; set; }
        public string Issuer { get; set; }

        public string AD_EmployeeId { get; set; }
        public string AD_FirstName { get; set; }
        public string AD_LastName { get; set; }
        public string AD_FullName { get; set; }
        public string AD_Email { get; set; }
        public string AD_Phone { get; set; }
        public string AD_EmpTitle { get; set; }
        public string AD_Department { get; set; }

        public string UserAccountControl { get; set; }
        public string ObjectGUID { get; set; }

        public string LoginId { get; set; }

        public string Features { get; set; }

        public string Msg_Invalid_Username_Password { get; set; }

        public string Msg_UnAuthorized { get; set; }

    }
}
