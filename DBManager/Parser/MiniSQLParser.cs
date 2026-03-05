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
            const string selectPattern = null;
            
            const string insertPattern = null;
            
            const string dropTablePattern = null;

            //Note: The parsing of CREATE TABLE should accept empty columns "()"
            //And then, an execution error should be given if a CreateTable without columns is executed
            const string createTablePattern = @"CREATE TABLE(\w +) \((\w +\s(?:String | Int | Double)(?:,\w +\s(?:String | Int | Double)) *)*\)";
            
            const string updateTablePattern = null;
            
            const string deletePattern = null;
            Match match = Regex.Match(miniSQLQuery, createTablePattern);
            if (match.Success) //Has there been a match?
            {
                String[] cols = match.Groups[2].Value.Split(',');
                List<ColumnDefinition> columnas = new List<ColumnDefinition>();
                foreach(String s in cols)
                {
                    String[] separados= s.Split(' ');
                    String nombre = separados[0];
                    String tipo = separados[1];
                    //ColumnDefinition rcol= new ColumnDefinition()

                }
                //return new CreateTable(match.Groups[1].Value, );
            }
            else
            {
                Console.WriteLine("No matches found");
            }


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
