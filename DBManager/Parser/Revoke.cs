using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;
using DbManager.Security;

namespace DbManager
{
 
    public class Revoke : MiniSqlQuery
    {
        public string PrivilegeName { get; set; }
        public string TableName { get; set; }
        public string ProfileName { get; set; }

        public Revoke(string privilegeName, string tableName, string profileName)
        {
            //TODO DEADLINE 4: Initialize member variables
            PrivilegeName = privilegeName;
            TableName = tableName;
            ProfileName = profileName;


        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, RevokePrivilegeSuccess, 
            Privilege privilegio;
            if (PrivilegeName == "DELETE")
            {
                privilegio = Privilege.Delete;
            } else if (PrivilegeName == "INSERT")
            {
                privilegio = Privilege.Insert;
            }
            else if (PrivilegeName == "SELECT")
            {
                privilegio = Privilege.Select;
            }
            else
            {
                privilegio = Privilege.Update;
            }

            if(database.SecurityManager.ProfileByName(ProfileName)== null)
            {
                return Constants.SecurityProfileDoesNotExistError;
            }
            else if(!database.SecurityManager.ProfileByName(ProfileName).PrivilegesOn[TableName].Contains(privilegio))
            {
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            }
            else
            {
                return Constants.RevokePrivilegeSuccess;
            }
            
        }

    }
}
