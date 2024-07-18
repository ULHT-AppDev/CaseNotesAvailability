using BusinessObjects;
using DAL;
using System;
using System.Collections.Generic;
using static BusinessObjects.Enums;

namespace BLL
{
    public class LoginBLL
    {

        LoginDAL DAL;

        public LoginBLL()
        {
            DAL = new LoginDAL();
        }

        public int? CheckIfUserExists(Guid userGuid, string username, string forename, string surname)
        {
            return DAL.CheckIfUserExists(userGuid, username, forename, surname);
        }

        public int? CheckIfUserExistsByGuid(Guid userGuid)
        {
            return DAL.CheckIfUserExistsByGuid(userGuid);
        }

        public int? InsertNewUserWithRole(Guid guid, string username, string forename, string surname, string jobRole, string email, string contactNumber, UserRoles userRole)
        {
            if (userRole == UserRoles.NotSet)
            {
                throw new Exception();
            }

            return DAL.InsertNewUserWithRole(guid, username, forename, surname, jobRole, email, contactNumber, userRole);
        }

        public int? InsertNewUserWithoutRole(Guid guid, string username, string forename, string surname, string jobRole, string email, string contactNumber)
        {
            return DAL.InsertNewUserWithoutRole(guid, username, forename, surname, jobRole, email, contactNumber);
        }

        public int SelectUserID(Guid userGuid)
        {
            try
            {
                return DAL.SelectUserID(userGuid);
            }
            catch (Exception ex)
            {
                //LogHelper.LogErrorNoSession(ex);
                throw ex;
            }
        }

        public void UpdateUserAcountDetails(int userID, string username, string forename, string surname)
        {
            UsersOldAccountDetails old = SelectOldAccountDetails(userID);
            string updateDetails = $"UserID: {userID}, Old username: {old.OldUsername}, old Forename: {old.OldForename}, old Surname: {old.OldSurname}";

            DAL.UpdateAccountDetails(userID, username, forename, surname, updateDetails, null);
        }

        public UsersOldAccountDetails SelectOldAccountDetails(int userID)
        {
            return DAL.SelectOldAccountDetails(userID);
        }

    }
}
