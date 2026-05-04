using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Security
{
    public class Manager
    {
        public List<Profile> Profiles { get; private set; } = new List<Profile>();

        private string m_username;
        public Manager(string username)
        {
            m_username = username;
        }

        public bool IsUserAdmin()
        {
            //TODO DEADLINE 5: Return true if the user logged-in (m_username) is the admin, false otherwise
            Profile p = ProfileByName(m_username);
            if (p != null && p.Name.Equals(Profile.AdminProfileName))
            {
                return true;
            } 
            else
            {
                return false;
            }
        }

        public bool IsPasswordCorrect(string username, string password)
        {
            //TODO DEADLINE 5: Return true if the user's password is correct. The given password should be encrypted before comparing with the saved one

            String passE = Encryption.Encrypt(password);
            User u = UserByName(username);
            if (u != null)
            {
                return u.EncryptedPassword.Equals(passE);
            }
            else
            {
                return false;
            }
        }

        public void GrantPrivilege(string profileName, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Add this privilege on this table to the profile with this name
            //If the profile or the table don't exist, do nothing
            if(!IsUserAdmin()) { return; }
            Profile perfilBuscado = ProfileByName(profileName);

            if (perfilBuscado != null)
            {
                perfilBuscado.GrantPrivilege(table, privilege);
            }
            
        }

        public void RevokePrivilege(string profileName, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Remove this privilege on this table to the profile with this name
            //If the profile or the table don't exist, do nothing
            if (!IsUserAdmin()) { return; }
            Profile perfilBuscado = ProfileByName(profileName);

            if (perfilBuscado != null)
            {
                perfilBuscado.RevokePrivilege(table, privilege);
            }
        }

        public bool IsGrantedPrivilege(string username, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Return true if the username has this privilege on this table. False otherwise (also in case of error)

            Profile perfilBuscado = ProfileByUser(username);

            if (perfilBuscado != null)
            {
                return perfilBuscado.IsGrantedPrivilege(table, privilege);
            }
            else
            {
                return false;
            }

        }

        public void AddProfile(Profile profile)
        {
            //TODO DEADLINE 5: Add this profile
            if (!IsUserAdmin()) { return; }
            Profiles.Add(profile);
            
        }

        public User UserByName(string username)
        {
            //TODO DEADLINE 5: Return the user by name. If it doesn't exist, return null

            foreach (Profile p in Profiles)
            {
                foreach (User u in p.Users)
                {
                    if (u.Username.Equals(username))
                    {
                        return u;
                    }
                }
            }
            return null;

        }

        public Profile ProfileByName(string profileName)
        {
            //TODO DEADLINE 5: Return the profile by name. If it doesn't exist, return null
            foreach(Profile p in Profiles)
            {
                if (p.Name.Equals(profileName))
                {
                    return p;
                }
            }
            
            return null;
            
        }

        public Profile ProfileByUser(string username)
        {
            //TODO DEADLINE 5: Return the profile by user. If the user doesn't exist, return null
            foreach (Profile p in Profiles)
            {
                foreach(User u in p.Users)
                {
                    if (u.Username.Equals(username))
                    {
                        return p;
                    }
                }
            }
            return null;
            
        }

        public bool RemoveProfile(string profileName)
        {
            //TODO DEADLINE 5: Remove this profile
            if (!IsUserAdmin()) { return false; }
            foreach ( Profile p in Profiles)
            {
                if (p.Name.Equals(profileName))
                {
                    Profiles.Remove(p);
                    return true;
                }
            }
            return false;
        }

        public static Manager Load(string databaseName, string username)
        {
            //TODO DEADLINE 5: Load all the profiles and users saved for this database. The Manager instance should be created with the given username
            try
            {
                if (databaseName != null && !databaseName.Equals(""))
                {
                    string[] files = Directory.GetFiles(databaseName, "*.txt");
                    Manager mg = new Manager(username);
                    String fileNoExtension;
                    foreach (string file in files)
                    {
                        fileNoExtension = System.IO.Path.GetFileNameWithoutExtension(file);
                        bool exists = System.IO.File.Exists(file); //checks that the file exists
                        if (!exists) { return null; }

                        TextReader reader = System.IO.File.OpenText(file); //opens an existing file
                        
                        Profile p = new Profile();
                        String line = reader.ReadLine();
                        p.Name = line;

                        String tName;

                        line=reader.ReadLine();
                        while (line != null && !line.Equals(""))
                        {
                            string[] splited = line.Split("%");
                            tName = splited[0];
                            string[] privs = splited[1].Split(",");
                            for(int i=0; i < privs.Length - 1; i++)
                            {
                                p.GrantPrivilege(tName, Enum.Parse<Privilege>(privs[i]));
                            }
                            line = reader.ReadLine();
                        }

                        line = reader.ReadLine();

                        List<User> usrs = new List<User>();
                        User u;
                        while (line != null && !line.Equals(""))
                        {
                            u = new User();
                            string[] a = line.Split(",");
                            u.Username = a[0];
                            u.EncryptedPassword = a[1];
                            usrs.Add(u);
                            line = reader.ReadLine();
                        }
                        p.Users = usrs;
                        mg.AddProfile(p);
                        reader.Close();
                            
                    }
                    return mg;
                }
                else { return null; }
            }
            catch (Exception e)
            {
                return null;
            }

            
        }

        public void Save(string databaseName)
        {
            //TODO DEADLINE 5: Save all the profiles and users/passwords created for this database.
            try
            {
                if (databaseName == null || databaseName.Equals("")) { return; }

                if (!Directory.Exists(databaseName))
                {
                    Directory.CreateDirectory(databaseName);
                }

                if (Profiles != null && Profiles.Count != 0)
                {
                    foreach (Profile p in Profiles)
                    {
                        TextWriter writer = System.IO.File.CreateText(databaseName + "\\" + p.Name + ".txt"); //creates a new text file
                        writer.WriteLine(p.Name);
                        var keys = p.PrivilegesOn.Keys;
                        foreach(String x in keys)
                        {
                            writer.Write(x + "%");
                            foreach(Privilege pr in p.PrivilegesOn[x])
                            {
                                writer.Write(pr+",");
                            }
                            writer.WriteLine();
                        }
                        writer.WriteLine();
                        foreach(User u in p.Users)
                        {
                            writer.WriteLine(u.Username + "," + u.EncryptedPassword);
                        }
                        writer.Close();
                    }
                }
            }
            catch (Exception e)
            {
                return;
            }
        }
    }
}
