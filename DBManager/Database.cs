using DbManager.Parser;
using DbManager.Security;
using System;
using System.Collections.Generic;
using System.IO;

namespace DbManager
{
    public class Database
    {
        private List<Table> Tables = new List<Table>();
        private string m_username;

        public string LastErrorMessage { get; private set; }

        public Manager SecurityManager { get; private set; }

        //This constructor should only be used from Load (without needing to set a password for the user). It cannot be used from any other class
        private Database()
        {
        }

        public Database(string adminUsername, string adminPassword)
        {
            //DEADLINE 1.B: Initalize the member variables
            m_username = adminUsername;

        }

        public bool AddTable(Table table)
        {
            //DEADLINE 1.B: Add a new table to the database
            if (table != null)
            {
                Tables.Add(table);
                return true;
            }
            else { return false; }

        }

        public Table TableByName(string tableName)
        {
            //DEADLINE 1.B: Find and return the table with the given name

            foreach (Table t in Tables)
            {
                if (t.Name.Equals(tableName))
                {
                    return t;
                }
            }
            return null;

        }

        public bool CreateTable(string tableName, List<ColumnDefinition> ColumnDefinition)
        {
            //DEADLINE 1.B: Create and new table with the given name and columns. If there is already a table with that name,
            //return false and set LastErrorMessage with the appropriate error (Check Constants.cs)
            //Do the same if no column is provided
            //If everything goes ok, set LastErrorMessage with the appropriate success message (Check Constants.cs)
            if (tableName != null && !(tableName.Equals("")))
            {
                foreach (Table t in Tables)
                {
                    if (t.Name.Equals(tableName))
                    {
                        LastErrorMessage = Constants.TableAlreadyExistsError;
                        return false;
                    }
                }

                if (ColumnDefinition == null || ColumnDefinition.Count <= 0)
                {
                    LastErrorMessage = Constants.DatabaseCreatedWithoutColumnsError;
                    return false;
                }

                Table newT = new Table(tableName, ColumnDefinition);
                Tables.Add(newT);
                LastErrorMessage = Constants.CreateTableSuccess;
                return true;
            }
            else { return false; }

        }

        public bool DropTable(string tableName)
        {
            //DEADLINE 1.B: Delete the table with the given name. If the table doesn't exist, return false and set LastErrorMessage
            //If everything goes ok, return true and set LastErrorMessage with the appropriate success message (Check Constants.cs)
            if (tableName != null)
            {
                foreach (Table t in Tables)
                {
                    if (t.Name.Equals(tableName))
                    {
                        Tables.Remove(t);
                        LastErrorMessage = Constants.DropTableSuccess;
                        return true;
                    }
                }
            }
            LastErrorMessage = Constants.TableDoesNotExistError;
            return false;
        }

        public bool Insert(string tableName, List<string> values)
        {
            //DEADLINE 1.B: Insert a new row to the table. If it doesn't exist return false and set LastErrorMessage appropriately
            //If everything goes ok, set LastErrorMessage with the appropriate success message (Check Constants.cs)

            Table tabla = TableByName(tableName);

            if (tabla == null)
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return false;
            }



            bool Insert = tabla.Insert(values);

            if (Insert == false)
            {
                LastErrorMessage = Constants.ColumnCountsDontMatch;
                return false;
            }

            LastErrorMessage = Constants.InsertSuccess;
            return true; ;


        }



        public Table Select(string tableName, List<string> columns, Condition condition)
        {
            //DEADLINE 1.B: Return the result of the select. If the table doesn't exist return null and set LastErrorMessage appropriately (Check Constants.cs)
            //If any of the requested columns doesn't exist, return null and set LastErrorMessage (Check Constants.cs)
            //If everything goes ok, return the table

            Table tabla = TableByName(tableName);

            if (tabla == null)
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return null;
            }

            if (columns != null)
            {
                foreach (string c in columns)
                {
                    if (tabla.ColumnByName(c) == null)
                    {
                        LastErrorMessage = Constants.ColumnDoesNotExistError;
                        return null;
                    }
                }
            }

            if (condition != null)
            {
                if (tabla.ColumnByName(condition.ColumnName) == null)
                {
                    LastErrorMessage = Constants.ColumnDoesNotExistError;
                    return null;
                }
            }

            Table TablaRes = tabla.Select(columns, condition);

            if (TablaRes == null)
            {
                LastErrorMessage = Constants.ColumnDoesNotExistError;
                return null;
            }


            return TablaRes;
        }

        public bool DeleteWhere(string tableName, Condition columnCondition)
        {
            //DEADLINE 1.B: Delete all the rows where the condition is true. 
            //If the table or the column in the condition don't exist, return null and set LastErrorMessage (Check Constants.cs)
            //If everything goes ok, return true
            Table tabla = TableByName(tableName);

            if (tabla == null)
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return false;
            }
            if (columnCondition == null)
            {
                return false;
            }

            if (columnCondition != null)
            {

                ColumnDefinition columna = tabla.ColumnByName(columnCondition.ColumnName);
                if (columna == null)
                {
                    LastErrorMessage = Constants.ColumnDoesNotExistError;
                    return false;
                }
            }

            tabla.DeleteWhere(columnCondition);
            LastErrorMessage = Constants.DeleteSuccess;
            return true;
        }

        public bool Update(string tableName, List<SetValue> columnNames, Condition columnCondition)
        {
            //DEADLINE 1.B: Update in the given table all the rows where the condition is true using the SetValues
            //If the table or the column in the condition don't exist, return null and set LastErrorMessage (Check Constants.cs)
            //If everything goes ok, return true

            Table tabla = TableByName(tableName);
            if (tabla == null)
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return false;
            }
            if (columnNames == null || columnNames.Count <= 0)
            {
                return false;
            }
            if (columnCondition != null)
            {
                ColumnDefinition columna = tabla.ColumnByName(columnCondition.ColumnName);
                if (columna == null)
                {
                    LastErrorMessage = Constants.ColumnDoesNotExistError;
                    return false;
                }
            }

            if (columnNames != null)
            {
                foreach (SetValue item in columnNames)
                {
                    if (tabla.ColumnByName(item.ColumnName) == null)
                    {
                        LastErrorMessage = Constants.ColumnDoesNotExistError;
                        return false;
                    }
                }
            }

            bool resultado = tabla.Update(columnNames, columnCondition);
            if (resultado == true)
            {
                LastErrorMessage = Constants.UpdateSuccess;
            }
            return resultado;
        }





        public bool Save(string databaseName)
        {
            //DEADLINE 1.C: Save this database to disk with the given name
            //If everything goes ok, return true, false otherwise.
            try
            {
                if (databaseName == null || databaseName.Equals("")) { return false; }
                
                if (!Directory.Exists(databaseName))
                {
                    Directory.CreateDirectory(databaseName);
                }
                
                if (Tables != null && Tables.Count != 0)
                {
                    String toSave;
                    List<ColumnDefinition> cd;
                    ColumnDefinition c;
                    Row r;

                    foreach (Table t in Tables)
                    {
                        TextWriter writer = System.IO.File.CreateText(databaseName + "\\" + t.Name + ".txt"); //creates a new text file
                        cd = new List<ColumnDefinition>();
                        for (int i = 0; i < t.NumColumns(); i++)
                        {
                            c = t.GetColumn(i);
                            writer.WriteLine(c.AsText());
                        }
                        writer.WriteLine();
                        for (int i = 0; i < t.NumRows(); i++)
                        {
                            r = t.GetRow(i);
                            writer.WriteLine(r.AsText());
                        }
                        writer.Close();
                    }
                }
                return true;            
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
                return false;
            }
            //DEADLINE 5: Save the SecurityManager so that it can be loaded with the database in Load()

            return false;

        }

        public static Database Load(string databaseName, string username, string password)
        {
            //DEADLINE 1.C: Load the (previously saved) database of name databaseName
            //If everything goes ok, return the loaded database (a new instance), null otherwise.
            try
            {
                if (databaseName != null && !databaseName.Equals(""))
                {
                    string[] files = Directory.GetFiles(databaseName, "*.txt");
                    Database db = new Database();
                    String fileNoExtension;
                    foreach (string file in files)
                    {
                        fileNoExtension = System.IO.Path.GetFileNameWithoutExtension(file);
                        bool exists = System.IO.File.Exists(file); //checks that the file exists
                        if (!exists) { return null; }
                        List<ColumnDefinition> cd = new List<ColumnDefinition>();

                        TextReader reader = System.IO.File.OpenText(file); //opens an existing file
                        String line = reader.ReadLine();
                        while (line != null && !line.Equals(""))
                        {
                            cd.Add(ColumnDefinition.Parse(line));
                            line = reader.ReadLine();
                        }
                        Table t = new Table(fileNoExtension, cd);
                        line = reader.ReadLine();

                        while (line != null && !line.Equals(""))
                        {
                            t.AddRow(Row.Parse(cd, line));
                            line = reader.ReadLine();
                        }

                        db.AddTable(t);
                        reader.Close();
                    }
                    return db;
                }
            }
            catch (Exception e)
            {
                return null;
            }
            //DEADLINE 5: When the Database object is created, set the username (create a new method if you must)
            //After loading the database, load the SecurityManager and check the password is correct. If it's not, return null. If it is return the database

            return null;
        }

        public string ExecuteMiniSQLQuery(string query)
        {
            //Parse the query
            MiniSqlQuery miniSQLQuery = MiniSQLParser.Parse(query);

            //If the parser returns null, there must be a syntax error (or the parser is failing)
            if (miniSQLQuery == null)
                return Constants.SyntaxError;

            //Once the query is parsed, we run it on this database
            return miniSQLQuery.Execute(this);
        }


        public bool IsUserAdmin()
        {
            return SecurityManager.IsUserAdmin();
        }





        //All these methods are ONLY FOR TESTING. Use them to simplify creating unit tests:
        public const string AdminUsername = "admin";
        public const string AdminPassword = "adminPassword";
        public static Database CreateTestDatabase()
        {
            Database database = new Database(AdminUsername, AdminPassword);

            database.Tables.Add(Table.CreateTestTable());

            return database;
        }

        public void AddTuplesForTesting(string tableName, List<List<string>> rows)
        {
            Table table = TableByName(tableName);
            foreach (List<string> row in rows)
            {
                table.Insert(row);
            }
        }

        public void CheckForTesting(string tableName, List<List<string>> rows)
        {
            Table table = TableByName(tableName);

            table.CheckForTesting(rows);
        }

        public static bool AreEqual(Database db1, Database db2)
        {
            if (db1 == null || db2 == null) { return false; }
            if (db1.Tables.Count != db2.Tables.Count) { return false; }
            for (int i = 0; i < db1.Tables.Count; i++) //IMPORTA EL ORDEN (para el save and load da igual, si se hace uso de este metodo comprobar ese matiz)
            {
                Table t1 = db1.Tables[i];
                Table t2 = db2.Tables[i];

                if (t1 == null || t2 == null) { return false; }
                if (t1.NumColumns() != t2.NumColumns()) { return false; }
                for (int j = 0; j < t1.NumColumns(); j++)
                {
                    if (t1.GetColumn(j).AsText() != t2.GetColumn(j).AsText())
                    { 
                        return false; 
                    }
                }

                if (t1.NumRows() != t2.NumRows()) { return false; }
                for (int z = 0; z < t1.NumRows(); z++)
                {
                    if (t1.GetRow(z).AsText() != t2.GetRow(z).AsText())
                    { return false; }
                }
            }
            return true;
        }
    }
}





