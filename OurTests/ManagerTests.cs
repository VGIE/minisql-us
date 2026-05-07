using DbManager;
using DbManager.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurTests
{
    public class ManagerTests
    {
        [Fact]
        public void ContrasenyaTest()
        {
            Database db = new Database(Profile.AdminProfileName, "123");

            Assert.True(db.SecurityManager.IsPasswordCorrect(Profile.AdminProfileName, "123"));
            Assert.False(db.SecurityManager.IsPasswordCorrect(Profile.AdminProfileName, "holaadios"));
            Assert.False(db.SecurityManager.IsPasswordCorrect("lentejas", "123"));
        }

        [Fact]
        public void IsUserAdminTest()
        {
            Database db = new Database(Profile.AdminProfileName, "123");

            Assert.True(db.SecurityManager.IsUserAdmin());
        }

        [Fact]
        public void UserByNameTest()
        {
            Database db = new Database(Profile.AdminProfileName, "123");

            Assert.Equal(Profile.AdminProfileName, db.SecurityManager.UserByName("Admin").Username);
            Assert.Null(db.SecurityManager.UserByName("queno%"));
        }

        [Fact]
        public void ProfileByNameTest()
        {
            Database db = new Database(Profile.AdminProfileName, "123");


            Assert.Equal(Profile.AdminProfileName, db.SecurityManager.ProfileByName(Profile.AdminProfileName).Name);
            Assert.Null(db.SecurityManager.ProfileByName("lentejas"));
        }

        [Fact]
        public void AddRemoveProfileTests()
        {
            Database db = new Database(Profile.AdminProfileName, "123");

            Profile nuevoProfile = new Profile();
            nuevoProfile.Name = "Carlitos";
            nuevoProfile.Users.Add(new User("Alcaraz", "pingpong123"));

            db.SecurityManager.AddProfile(nuevoProfile);
            Assert.NotNull(db.SecurityManager.ProfileByName("Carlitos"));
            Assert.True(db.SecurityManager.RemoveProfile("Carlitos"));
            Assert.Null(db.SecurityManager.ProfileByName("Carlitos"));
            Assert.False(db.SecurityManager.RemoveProfile("quenoo"));
        }

        [Fact]
        public void GrantAndRevokePrivilegeTest()
        {
            Database db = new Database(Profile.AdminProfileName, "123");

            Profile nuevoProfile = new Profile();
            nuevoProfile.Name = "Profes";
            nuevoProfile.Users.Add(new User("borja", "pass123"));
            db.SecurityManager.AddProfile(nuevoProfile);

            db.SecurityManager.GrantPrivilege("Profes", "Asignaturas", Privilege.Select);
            Assert.True(db.SecurityManager.IsGrantedPrivilege("borja", "Asignaturas", Privilege.Select));
            Assert.False(db.SecurityManager.IsGrantedPrivilege("borja", "Asignaturas", Privilege.Delete));

            db.SecurityManager.RevokePrivilege("Profes", "Asignaturas", Privilege.Select);
            Assert.False(db.SecurityManager.IsGrantedPrivilege("borja", "Asignaturas", Privilege.Select));
            Assert.False(db.SecurityManager.IsGrantedPrivilege("quenooooo", "Asignaturas", Privilege.Select));
        }

        [Fact]
        public void SaveAndLoadTest()
        {
            Database db = new Database(Profile.AdminProfileName, "123");

            Profile nuevoProfile = new Profile();
            nuevoProfile.Name = "Profe";
            nuevoProfile.Users.Add(new User("borja", "minisql-us"));
            db.SecurityManager.AddProfile(nuevoProfile);
            db.SecurityManager.GrantPrivilege("Profe", "Asignaturas", Privilege.Select);

            db.SecurityManager.Save("guardadoSecurityTest");

            Manager managerCargado = Manager.Load("guardadoSecurityTest", Profile.AdminProfileName);

            Manager managerOriginal = db.SecurityManager;
            //Assert.True(Manager.AreEqual(managerOriginal, managerCargado));
        }

    }

}
