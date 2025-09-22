using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace YashCustomToolRitesh
{
    public class IdentityHelper
    {
        private readonly IOptions<AppSettings> _appSettings;

        public IdentityHelper(IOptions<AppSettings> appSettings)
        {
            this._appSettings = appSettings;
        }

        
 
    }
}
