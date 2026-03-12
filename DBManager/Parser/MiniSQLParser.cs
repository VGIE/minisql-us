using DbManager.Parser;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DbManager
{
    public class MiniSQLParser
    {
        public static MiniSqlQuery Parse(string miniSQLQuery)
        {
            //TODO DEADLINE 2
            const string selectPattern = null; //mikel
            
            const string insertPattern = @"INSERT\s+INTO\s+(\w+)\s+VALUES\s+\(((?:\s*'([^']*)'\s*,)*(?:\s*'([^']*)'\s*))\)"; //kaiet
            
            const string dropTablePattern = null; //fabian
            
            //Note: The parsing of CREATE TABLE should accept empty columns "()"
            //And then, an execution error should be given if a CreateTable without columns is executed
            const string createTablePattern = null; //fabian
            
            const string updateTablePattern = @"UPDATE\s+(\w+)\s+SET\s+((?:\w+='[^']*')(?:,\s*\w+='[^']*')*)\s+WHERE\s+(\w+)(=|<|>)'([^']*)'";
            
            const string deletePattern = @"DELETE\s+FROM\s+(\w+)\s+WHERE\s+(\w+)\s*(<|>|=)\s*'([^']*)'"; //kaiet
            

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
            Match match;
            
            match = Regex.Match(miniSQLQuery, insertPattern);
            if (match.Success == true)
            {
                if(match.Length != miniSQLQuery.Length) 
                { 
                    return null; 
                }
                string toFilter, toSplit="";
                bool copying = false;
                toFilter = match.Groups[2].Value;
                for(int i = 0; i < toFilter.Length; i++)
                {
                    if (toFilter[i]=='\'')
                    {
                        copying = !copying;
                    }
                    else if (copying == true)
                    {
                        toSplit += toFilter[i];
                    }
                    else if (toFilter[i] == ',')
                    {
                        toSplit += ",";
                    }
                }
                List<string> values = new List<string>();
                values = CommaSeparatedNames(toSplit);
                return new Insert(match.Groups[1].Value, values);
            }

            match = Regex.Match(miniSQLQuery, deletePattern);
            if (match.Success == true)
            {
                if (match.Length != miniSQLQuery.Length) 
                { 
                    return null; 
                }
                return new Delete(match.Groups[1].Value, new Condition(match.Groups[2].Value, match.Groups[3].Value, match.Groups[4].Value));
            }

            // --- TU BLOQUE UPDATE INTEGRADO ---
            match = Regex.Match(miniSQLQuery, updateTablePattern);
            if (match.Success == true)
            {
                if (match.Length != miniSQLQuery.Length)
                {
                    return null;
                }

                string tableName = match.Groups[1].Value;
                string setString = match.Groups[2].Value;
                string condColumn = match.Groups[3].Value;
                string conditionOperator = match.Groups[4].Value;
                string conditionValue = match.Groups[5].Value;

                List<SetValue> setValues = new List<SetValue>();
                List<string> asignaciones = CommaSeparatedNames(setString);

                foreach(string asignacion in asignaciones)
                {
                    string[] partes = asignacion.Split("=");
                    
                    if(partes.Length == 2)
                    {
                        string columna = partes[0].Trim();
                        string valorConComillas = partes[1].Trim();

                        if (valorConComillas.StartsWith("'") == true && valorConComillas.EndsWith("'") == true)
                        {
                            // extraemos solo lo de dentro de las comillas
                            string valorLimpio = valorConComillas.Substring(1, valorConComillas.Length - 2);
                            
                            SetValue nuevo = new SetValue(columna, valorLimpio);
                            setValues.Add(nuevo);
                        }
                        else
                        {
                            return null; 
                        }
                    }
                    else
                    {
                        return null; 
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
