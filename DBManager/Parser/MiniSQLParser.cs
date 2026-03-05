using DbManager.Parser;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DbManager
{
    public class MiniSQLParser
    {
        public static MiniSqlQuery Parse(string miniSQLQuery)
        {
            //TODO DEADLINE 2
            const string selectPattern = null;
            
            const string insertPattern = null;
            
            const string dropTablePattern = null;
            
            //Note: The parsing of CREATE TABLE should accept empty columns "()"
            //And then, an execution error should be given if a CreateTable without columns is executed
            const string createTablePattern = null;
            
            const string updateTablePattern = @"UPDATE\s+(\w+)\s+SET\s+(.*)\s+WHERE\s+(\w+)\s*(=|<|>)\s*(.*)";
            
            const string deletePattern = null;
            

            //TODO DEADLINE 4
            const string createSecurityProfilePattern = null;
            
            const string dropSecurityProfilePattern = null;
            
            const string grantPattern = null;
            
            const string revokePattern = null;
            
            const string addUserPattern = null;
            
            const string deleteUserPattern = null;
            

            //TODO DEADLINE 2
            //Parse query using the regular expressions above one by one. If there is a match, create an instance of the query with the parsed parameters
            //For example, if the query is a "SELECT ...", there should be a match with selectPattern. We would create and return an instance of Select
            //initialized with the table name, the columns, and (possibly) an instance of Condition.
            //If there is no match, it means there is a syntax error. We will return null.
            Match matchUpdate = Regex.Match(miniSQLQuery, updateTablePattern);
            if (matchUpdate.Success)
            {
                string tableName = matchUpdate.Groups[1].Value;
                string setString = matchUpdate.Groups[2].Value;
                string condColumn = matchUpdate.Groups[3].Value;
                string conditionOperator = matchUpdate.Groups[4].Value;
                string conditionValue = matchUpdate.Groups[5].Value.Trim('\'');

                List<SetValue> setValues = new List<SetValue>();
                List<string> asignaciones = CommaSeparatedNames(setString);

                foreach(string asignacion in asignaciones)
                {
                    string[] partes = asignacion.Split("=");
                    if(partes.Length == 2)
                    {
                        string columna = partes[0].Trim();
                        string valor = partes[1].Trim().Trim('\'');
                        SetValue nuevo = new SetValue(columna, valor);
                        setValues.Add(nuevo);
                    }
                }
                Condition condition = new Condition(condColumn, conditionOperator, conditionValue);
                Update consultaUpdate = new Update(tableName, setValues, condition);

                return consultaUpdate;
            }


            //TODO DEADLINE 4
            //Do the same for the security queries (CREATE SECURITY PROFILE, ...)
            
            return null;
           
        }

        static List<string> CommaSeparatedNames(string text)
        {
            string[] textParts = text.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
            List<string> commaSeparator = new List<string>();
            for(int i=0; i < textParts.Length; i++)
            {
                commaSeparator.Add(textParts[i]);
            }
            return commaSeparator;
        }
        
    }
}
