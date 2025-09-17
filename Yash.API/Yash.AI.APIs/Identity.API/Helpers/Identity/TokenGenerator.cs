
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.Collections.Generic;
//using System.DirectoryServices.AccountManagement;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Reflection.Metadata.Ecma335;
//using System.Security.Authentication;
//using System.Security.Claims;
//using System.Text;

//namespace Identity.API.Helpers.Identity
//{
//    public class TokenGenerator
//    {
//        private readonly IOptions<AppSettings> _appSettings;
        
//        private IUsersBL _loginBL;
//        string userAgent = string.Empty;

//        public TokenGenerator( IUsersBL loginBL, IOptions<AppSettings> appSettings)
//        {
//            this._appSettings = appSettings;
//            //this._identityBL = identityBL;
//            //this._userLoginBL = userLoginBL;
//            this._loginBL = loginBL;
//        }
//        //public string GetToken(Login loginParams)
//        //{
//        //    //bool isValidCredentials = ValidateCredentials(loginParams);

//        //    //if (!isValidCredentials)
//        //    //{
//        //    //    throw new AuthenticationException();
//        //    //}

//        //    //Commneted below line bcz its causing account locke 
//        //    //ValidateCredentials(loginParams);

//        //    //Validiting in  AD
//        //    UserPrincipal userPrincipal = ValidateCredentials(loginParams.LoginName);
//        //    if (userPrincipal == null)
//        //    {
//        //        throw new AuthenticationException();
//        //    }

//        //    IdentityHelper identityHelper = new IdentityHelper(_appSettings);

//        //    User user = identityHelper.GetClaims(loginParams);
//        //    UserLoginDTO loginInfo = new UserLoginDTO()
//        //    {
//        //        UserID = user.EmployeeId,
//        //        LoginName = loginParams.LoginName,
//        //        Broswer = CFM.Common.CommonHelper.GetWebBrowserName(userAgent),
//        //        IpAddress = CFM.Common.CommonHelper.GetMACAddress(),
//        //        LoginDateTime = DateTime.Now,

//        //    };
//        //    List<string> MatchGroupList = new List<string>();

//        //    List<string> ADGroupList = new List<string>();
//        //    foreach (var item in user.Groups)
//        //    {
//        //        ADGroupList.Add(ReplaceGroupForTesting(item));
//        //    }

//        //    List<Claim> lstClaims = new List<Claim>();
//        //    var loginDetailsId = _userLoginBL.SaveLogin(loginInfo);
//        //    int loginId = Convert.ToInt32(loginDetailsId);

//        //    if (ADGroupList.Count >= 1)
//        //    {
//        //        var CFMGroup = _identityBL.GetCFMGroup();
//        //        int MatchCount = 0;
//        //        foreach (var item in CFMGroup)
//        //        {
//        //            if (ADGroupList.Contains(item.MarketGroupName.ToLower().ToString()))
//        //            {
//        //                MatchCount++;
//        //                MatchGroupList.Add(item.MarketGroupName);
//        //            }
//        //        }
//        //        //if  no matched found in AD and DB then its No Group
//        //        if (MatchCount == 0)
//        //        {
//        //            lstClaims.Add(new Claim("NoGroup", "NoGroup"));
//        //        }

//        //        #region Constructing the Claims data
//        //        lstClaims.Add(new Claim(_appSettings.Value.EmployeeId, user.EmployeeId.ToString()));
//        //        lstClaims.Add(new Claim(_appSettings.Value.UserName, user.LoginName));
//        //        lstClaims.Add(new Claim(_appSettings.Value.FirstName, user.FirstName));
//        //        lstClaims.Add(new Claim(_appSettings.Value.LastName, user.LastName));
//        //        lstClaims.Add(new Claim(_appSettings.Value.Email, user.Email));
//        //        lstClaims.Add(new Claim(_appSettings.Value.FullName, user.FullName));
//        //        lstClaims.Add(new Claim(_appSettings.Value.LoginId, loginId.ToString()));


//        //        foreach (string role in MatchGroupList)
//        //        {
//        //            lstClaims.Add(new Claim(ClaimTypes.Role, role));
//        //        }

//        //        #region Fetch the Authorized screens based on the Groups

//        //        lstClaims.Add(new Claim(_appSettings.Value.Features, _loginBL.GetGroupPermissions(user.Groups)));

//        //        #endregion

//        //        #region Write a Audit trial information of the logged in user
//        //        lstClaims.Add(new Claim(_appSettings.Value.LoginId, loginDetailsId.ToString()));



//        //        #endregion



//        //        #endregion


//        //        // Logout timer 
//        //        string logoutTimer = _identityBL.GetApplicationSetting("LogoutTimer");
//        //        lstClaims.Add(new Claim("logoutTimer", logoutTimer.ToString()));

//        //        // TODO:  Better to use a certificate to lock the JWT Token instead of a secret key

//        //        #region Creating the JWT token

//        //        var securityKey = _appSettings.Value.Key;
//        //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
//        //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//        //        //int ExpireToken = _identityBL.();
//        //        var tokenParams = new JwtSecurityToken(
//        //            issuer: _appSettings.Value.Issuer,
//        //            claims: lstClaims,
//        //            expires: DateTime.Now.AddMinutes(60),
//        //            signingCredentials: creds
//        //            );

//        //        var tokenHandler = new JwtSecurityTokenHandler();

//        //        return tokenHandler.WriteToken(tokenParams);

//        //        #endregion
//        //    }

//        //    return string.Empty;
//        //}


//        //public static string ReplaceGroupForTesting(string groupName)
//        //{
//        //    groupName = groupName.ToLower().Trim();
//        //    // This  code just for testing purpose 
//        //    if (groupName == "Market Approvers".ToLower()) //1
//        //    {
//        //        groupName = "SG-CFMTEST_MarketApprovers";
//        //    }
//        //    else if (groupName == "Check Fraud Approvers".ToLower())//2
//        //    {
//        //        groupName = "SG-CFMTEST_CheckFraudApprovers";
//        //    }
//        //    else if (groupName == "Market Assigners".ToLower())//3
//        //    {
//        //        groupName = "SG-CFMTEST_MarketAssigners";
//        //    }
//        //    else if (groupName == "Market FDIC Approvers".ToLower())//4
//        //    {
//        //        groupName = "SG-CFMTEST_MarketFDICApprovers";
//        //    }
//        //    else if (groupName == "Market Operations".ToLower())//5
//        //    {
//        //        groupName = "SG-CFMTEST_MarketOperations";
//        //    }
//        //    else if (groupName == "Quality Control Resolvers Group".ToLower())//6
//        //    {
//        //        groupName = "SG-CFMTEST_QualityControlResolversGroup";
//        //    }
//        //    else if (groupName == "Quality Control Approvers Group".ToLower())//6
//        //    {
//        //        groupName = "SG-CFMTEST_QualityControlResolversGroup";
//        //    }
//        //    else if (groupName == "CFM RegE Submitters".ToLower())//7
//        //    {
//        //        groupName = "SG-CFMTEST_CFMRegESubmitters";
//        //    }
//        //    else if (groupName == "CFM Admin Group".ToLower())//8
//        //    {
//        //        groupName = "SG-CFMTEST_CFMAdministratorsGroup";
//        //    }
//        //    else if (groupName == "RegE Approval Group".ToLower())//9
//        //    {
//        //        groupName = "SG-CFMTEST_RegEApprovalGroup";
//        //    }
//        //    else if (groupName == "Corporate Compliance Reviewers".ToLower())//10
//        //    {
//        //        groupName = "SG-CFMTEST_CorporateComplianceReviewers";
//        //    }
//        //    else if (groupName == "Corporate Operations Approvers".ToLower())//11
//        //    {
//        //        groupName = "SG-CFMTEST_CorporateOperationsApprovers";
//        //    }
//        //    else if (groupName == "Consolidated Refund Group".ToLower())//12
//        //    {
//        //        groupName = "SG-CFMTEST_ConsolidatedRefundGroup";
//        //    }
//        //    else if (groupName == "CFM Administrators".ToLower())//12
//        //    {
//        //        groupName = "CFM Administrators";
//        //    }
//        //    else if (groupName == "Domain Users")
//        //    {
//        //        groupName = "Domain Users";
//        //    }



//        //    // Consolidated Refund Group





//        //    return groupName;
//        //}
//        //private UserPrincipal ValidateCredentials(string loginName)
//        //{
//        //    try
//        //    {
//        //        using (PrincipalContext principalContext = new PrincipalContext(ContextType.Domain))
//        //        {
//        //            UserPrincipal user = UserPrincipal.FindByIdentity(principalContext, loginName);
//        //            //if (user != null)
//        //            //    return true;
//        //            //else
//        //            //    return false;


//        //            return user;
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        return null;
//        //    }
//        //}
//        //private bool ValidateCredentials(Login loginParams)
//        //{
//        //    try
//        //    {
//        //        using (PrincipalContext principalContext = new PrincipalContext(ContextType.Domain))
//        //        {
//        //            return principalContext.ValidateCredentials(loginParams.LoginName, loginParams.Pwd);
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        return false;
//        //    }
//        //}
//    }
//}
