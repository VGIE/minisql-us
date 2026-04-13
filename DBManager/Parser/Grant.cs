using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;
using DbManager.Security;

namespace DbManager
{
 
    public class Grant : MiniSqlQuery
    {
        public string PrivilegeName { get; set; }
        public string TableName { get; set; }
        public string ProfileName { get; set; }

        public Grant(string privilegeName, string tableName, string profileName)
        {
            //TODO DEADLINE 4: Initialize member variables
                PrivilegeName = privilegeName;
                TableName = tableName;
                ProfileName = profileName;
            
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, PrivilegeDoesNotExistError, GrantPrivilegeSuccess, ProfileAlreadyHasPrivilege
            
            Privilege privilegeEnum;
            bool isValidPrivilege = Enum.TryParse<Privilege>(PrivilegeName, true, out privilegeEnum);
            
            if (isValidPrivilege == false)
            {
                return "PrivilegeDoesNotExistError"; 
            }

            // 2. Comprobamos si el usuario actual tiene permisos de Administrador
            if (database.SecurityManager.IsUserAdmin() == false)
            {
                return "UsersProfileIsNotGrantedRequiredPrivilege";
            }

           
            Profile targetProfile = database.SecurityManager.ProfileByName(ProfileName);
            
            if (targetProfile == null)
            {
                return "SecurityProfileDoesNotExistError";
            }

          
            bool success = targetProfile.GrantPrivilege(TableName, privilegeEnum);

            if (success == true)
            {
                return "GrantPrivilegeSuccess";
            }
            else
            {
                return "ProfileAlreadyHasPrivilege";
            }
        }
            
        }

    }

