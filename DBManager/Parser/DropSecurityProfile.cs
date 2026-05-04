using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using DbManager.Parser;

namespace DbManager
{

    public class DropSecurityProfile : MiniSqlQuery
    {
        public string ProfileName { get; set; }

        public DropSecurityProfile(string profileName)
        {
            //TODO DEADLINE 4: Initialize member variables
            ProfileName = profileName;

        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, DropSecurityProfileSuccess

            if (database.SecurityManager.IsUserAdmin() == false)
            {
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;


            }

            bool exito = database.SecurityManager.RemoveProfile(ProfileName);

            if (exito)
            {

                return Constants.DropSecurityProfileSuccess;

            }
            else
            {
                return Constants.SecurityProfileDoesNotExistError;
            }

        }
    }

}

    
