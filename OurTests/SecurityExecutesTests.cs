using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbManager;
using DbManager.Security;

namespace OurTests
{
    public class SecurityExecutesTests
    {
        [Fact]
        public void GrantTest()
        {

        }

        [Fact]
        public void AddUserTest()
        {

        }

        [Fact]
        public void DeleteUserTest()
        {

        }

        [Fact]
        public void RevokeTest()
        {

        }

       /* [Fact]
        public void CreateProfileTest()
        {
            Database db = new Database("", "");

            CreateSecurityProfile query = new CreateSecurityProfile("Novatos");

            string result = query.Execute(db);

            Assert.Equal(Constants.CreateSecurityProfileSuccess, result);

            Assert.NotNull(db.SecurityManager.ProfileByName("Novatos"));

            CreateSecurityProfile query2 = new CreateSecurityProfile("Novatos");

            string result2 = query.Execute(db);

            Assert.Equal(Constants.ProfileAlreadyHasPrivilege, result2);


            
        }

        [Fact]
        public void DropProfileTest()
        {
            Database db = new Database("","");

            CreateSecurityProfile query = new CreateSecurityProfile("Novatos");

            string result = query.Execute(db);

            Assert.Equal(Constants.CreateSecurityProfileSuccess, result);

            Assert.NotNull(db.SecurityManager.ProfileByName("Novatos"));

            CreateSecurityProfile query2 = new CreateSecurityProfile("Novatos");

            string result2 = query.Execute(db);







        }*/

    }
}
