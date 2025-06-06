﻿using BusinessObjects;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using static BusinessObjects.Enums;

namespace DAL
{
    public class LoginDAL
    {

        public int SelectUserID(Guid userGuid)
        {
            using (var ctx = new Model.CNAEntities())
            {
                return (from i in ctx.People
                        from g in i.PersonUniqueGuids.Where(x => x.Guid == userGuid)
                        where i.IsActive && g.IsActive
                        select i.PersonID).Single();
            }
        }

        public short? SelectUserID(int sessionID)
        {
            using (var ctx = new Model.CNAEntities())
            {
                return ctx.LogSessions.Where(x => x.SessionID == sessionID).Select(x => x.PersonID).Cast<Nullable<short>>().SingleOrDefault();
            }
        }

        public List<KeyValuePair<byte, string>> SelectUserRoleIDs(int userID)
        {
            using (var ctx = new Model.CNAEntities())
            {
                return (from u in ctx.People
                        join ur in ctx.PersonRoles on u.PersonID equals ur.PersonID
                        join r in ctx.LoginRoles on ur.RoleID equals r.RoleID
                        where u.IsActive && ur.IsActive && r.IsActive && u.PersonID == userID
                        select new { r.Role, r.RoleID })
                        .AsEnumerable()
                        .Select(x => new KeyValuePair<byte, string>(x.RoleID, x.Role))
                        .ToList();
            }
        }
        public int? CheckIfUserExists(Guid guid, string username, string forename, string surname)
        {
            using (var ctx = new Model.CNAEntities())
            {
                return (from i in ctx.People
                        from g in i.PersonUniqueGuids.Where(x => x.Guid == guid)
                        where i.IsActive && g.IsActive && i.Username == username && i.Surname == surname && i.Forename == forename
                        select i.PersonID).Cast<int?>().SingleOrDefault();
            }
        }

        public int? CheckIfUserExistsByGuid(Guid guid)
        {
            using (var ctx = new Model.CNAEntities())
            {
                return (from i in ctx.People
                        from g in i.PersonUniqueGuids.Where(x => x.Guid == guid)
                        where i.IsActive && g.IsActive
                        select i.PersonID).Cast<Nullable<int>>().SingleOrDefault();
            }
        }

        public int InsertNewUserWithRole(Guid guid, string username, string forename, string surname, string jobRole, string email, string contactNumber, UserRoles role)
        {
            using (var ctx = new Model.CNAEntities())
            {

                PersonUniqueGuid guidList = new PersonUniqueGuid()
                {
                    Guid = guid,
                    IsActive = true
                };

                var dateQuery = ctx.Database.SqlQuery<DateTime>("SELECT getdate()");
                DateTime serverDate = dateQuery.AsEnumerable().First();

                Person newUser = new Person()
                {
                    Forename = forename,
                    Surname = surname,
                    Username = username,
                    DateAdded = serverDate,
                    JobRole = jobRole,
                    Email = email,
                    PhoneNumber = contactNumber,
                    IsActive = true
                };

                guidList.People.Add(newUser);

                PersonRole ur = new PersonRole()
                {
                    PersonID = newUser.PersonID,
                    RoleID = (byte)role,
                    IsActive = true
                };

                ctx.PersonUniqueGuids.Add(guidList);
                ctx.PersonRoles.Add(ur);
                ctx.SaveChanges();

                return newUser.PersonID;
            }
        }

        public int InsertNewUserWithoutRole(Guid guid, string username, string forename, string surname, string jobRole, string email, string contactNumber)
        {
            using (var ctx = new Model.CNAEntities())
            {

                PersonUniqueGuid guidList = new PersonUniqueGuid()
                {
                    Guid = guid,
                    IsActive = true
                };

                var dateQuery = ctx.Database.SqlQuery<DateTime>("SELECT getdate()");
                DateTime serverDate = dateQuery.AsEnumerable().First();

                Person newUser = new Person()
                {
                    Forename = forename,
                    Surname = surname,
                    Username = username,
                    DateAdded = serverDate,
                    JobRole = jobRole,
                    Email = email,
                    PhoneNumber = contactNumber,
                    IsActive = true
                };

                guidList.People.Add(newUser);
                ctx.PersonUniqueGuids.Add(guidList);
                ctx.SaveChanges();

                return newUser.PersonID;
            }
        }

        public List<short> SelectUserRights(byte roleID)
        {
            using (var ctx = new Model.CNAEntities())
            {
                return (from ri in ctx.LoginRights
                        join rr in ctx.LoginRoleRights on ri.RightID equals rr.RightID
                        where rr.RoleID == roleID && rr.IsActive
                        select ri.RightID).ToList();
            }
        }

        public void UpdateAccountDetails(int userID, string username, string forename, string surname, string logDetails, int? sessionID)
        {
            using (var ctx = new Model.CNAEntities())
            {
                using (var trans = ctx.Database.BeginTransaction())
                {
                    try
                    {

                        var user = (from i in ctx.People
                                    where i.PersonID == userID && i.IsActive
                                    select i).Single();

                        user.Username = username;
                        user.Forename = forename;
                        user.Surname = surname;
                        ctx.SaveChanges();

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public UsersOldAccountDetails SelectOldAccountDetails(int userID)
        {
            using (var ctx = new Model.CNAEntities())
            {
                return (from i in ctx.People
                        where i.PersonID == userID && i.IsActive
                        select new UsersOldAccountDetails
                        {
                            OldUsername = i.Username,
                            OldForename = i.Forename,
                            OldSurname = i.Surname
                        }).Single();
            }
        }

    }
}