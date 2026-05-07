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
           /* Database db = new Database("admin", "admin");
            CreateSecurityProfile createQuery = new CreateSecurityProfile("Nose");
            createQuery.Execute(db); 
            Grant query1 = new Grant("SELECT", "Tabla1", "Nose");
            string result1 = query1.Execute(db);
            Assert.Equal(Constants.GrantPrivilegeSuccess, result1);
            Grant query2 = new Grant("SELECT", "Tabla1", "Nose");
            string result2 = query2.Execute(db);
            Assert.Equal(Constants.ProfileAlreadyHasPrivilege, result2);
            Grant query3 = new Grant("SELECT", "Tabla1", "SeNo");
            string result3 = query3.Execute(db);
            Assert.Equal(Constants.SecurityProfileDoesNotExistError, result3);
            Grant query4 = new Grant("SALTAR", "Tabla1", "Nose");
            string result4 = query4.Execute(db);
            Assert.Equal(Constants.PrivilegeDoesNotExistError, result4);
            */
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
            //Crear un usuario y una tabala, darle los permisos sobre la tabla con GrantPrivilege
            
            User usuarioTest = new User("Test", "1234");
            Profile perfilTest = new Profile { Name = "UsuarioPrueba" };
            perfilTest.Users.Add(usuarioTest);
            perfilTest.GrantPrivilege("Coches", Privilege.Select);

            
            Manager man = new Manager("Admin");

            
            Profile adminP = new Profile { Name = "Admin" };
            adminP.Users.Add(new User("Admin", "supersecret"));
            man.AddProfile(adminP);

            
            man.AddProfile(perfilTest);
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
