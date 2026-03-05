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
            
            const string insertPattern = @"INSERT\s+INTO\s+(\w+)\s+VALUES\s+\(((?:\s*'([^']*)'\s*,)*(?:\s*'([^']*)'\s*))\)\s*;";
            
            const string dropTablePattern = null;
            
            //Note: The parsing of CREATE TABLE should accept empty columns "()"
            //And then, an execution error should be given if a CreateTable without columns is executed
            const string createTablePattern = null;
            
            const string updateTablePattern = null;
            
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

            Match match = Regex.Match(miniSQLQuery, insertPattern);
            if (match.Success)
            {
                string toFilter, toSplit="";
                bool copying = false;
                toFilter = match.Groups[2].Value;
                for(int i = 0; i < toFilter.Length; i++)
                {
                    if (toFilter[i].Equals("'"))
                    {
                        copying = !copying;
                    }
                    else if (copying)
                    {
                        toSplit += toFilter[i];
                    }
                    else if (toFilter[i].Equals(","))
                    {
                        toSplit += ",";
                    }
                }
                List<string> values = new List<string>();
                values = CommaSeparatedNames(toSplit);
                return new Insert(match.Groups[1].Value, values);
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
