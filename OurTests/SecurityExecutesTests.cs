using System;
using System.Collections.Generic;
using System.Globalization;
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
           Database db = new Database("admin", "admin");
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
            Database db = Database.CreateTestDatabase();


            User usuarioTest = new User("Test", "1234");
            Profile perfilTest = new Profile { Name = "UsuarioPrueba" };
            perfilTest.Users.Add(usuarioTest);
            db.SecurityManager.AddProfile(perfilTest);
            db.SecurityManager.GrantPrivilege(perfilTest.Name, "TestTable", Privilege.Select);

            Revoke prueba = new Revoke("SELECT", db.TableByName("TestTable").Name, perfilTest.Name);
            Assert.Equal(Constants.RevokePrivilegeSuccess, prueba.Execute(db));

            User usuarioTest2 = new User("Pablo", "1234");
            Profile perfilTest2 = new Profile { Name = "UsuarioPablo" };
            perfilTest2.Users.Add(usuarioTest2);
            Revoke prueba2 = new Revoke("SELECT", db.TableByName("TestTable").Name, perfilTest2.Name);
            Assert.Equal(Constants.SecurityProfileDoesNotExistError, prueba2.Execute(db));

            User usuarioTest3 = new User("Luis", "1234");
            Profile perfilTest3 = new Profile { Name = "UsuarioLuis" };
            perfilTest3.Users.Add(usuarioTest3);
            db.SecurityManager.AddProfile(perfilTest3);
            db.SecurityManager.GrantPrivilege(perfilTest3.Name, "TestTable", Privilege.Delete);
            Revoke prueba3 = new Revoke("SELECT", db.TableByName("TestTable").Name, perfilTest2.Name);
            Assert.Equal(Constants.SecurityProfileDoesNotExistError, prueba3.Execute(db));
        }


       
        [Fact]
        public void CreateProfileTest()
        {
            Database db = new Database("admin", "admin");

            CreateSecurityProfile query1 = new CreateSecurityProfile("Novatos");

            string resultado1 = query1.Execute(db);

            Assert.Equal(Constants.CreateSecurityProfileSuccess, resultado1);

            Assert.NotNull(db.SecurityManager.ProfileByName("Novatos"));

            CreateSecurityProfile query2 = new CreateSecurityProfile("Novatos");

            string result2 = query2.Execute(db);

            Assert.Equal(Constants.CreateSecurityProfileSuccess, result2);


            
        }

        [Fact]
        public void DropProfileTest()
        {
            Database db = new Database("admin","admin");

            CreateSecurityProfile query1 = new CreateSecurityProfile("Novatos");

            string resultado1 = query1.Execute(db);

            Assert.NotNull(db.SecurityManager.ProfileByName("Novatos"));

            DropSecurityProfile query2 = new DropSecurityProfile("Novatos");

            string resultado2 = query2.Execute(db);

            Assert.Equal(Constants.DropSecurityProfileSuccess, resultado2);

            Assert.Null(db.SecurityManager.ProfileByName("Novatos"));

            DropSecurityProfile query3 = new DropSecurityProfile("Kaiets");
            string resultado3 = query3.Execute(db);

            Assert.Equal(Constants.SecurityProfileDoesNotExistError, resultado3);

            Assert.Null(db.SecurityManager.ProfileByName("Kaiets"));
           


        }

    }
}
