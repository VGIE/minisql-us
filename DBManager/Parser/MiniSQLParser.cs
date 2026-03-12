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
            const string selectPattern = @"SELECT\s+(.+)\s+FROM\s+(\w+)\s*(WHERE\s+(\w+)\s*([=<>])\s*(.+))*";
    
            
           
            
            const string insertPattern = @"INSERT\s+INTO\s+(\w+)\s+VALUES\s+\(((?:\s*'([^']*)'\s*,)*(?:\s*'([^']*)'\s*))\)"; //kaiet
            
            const string dropTablePattern = null; //fabian
            
            //Note: The parsing of CREATE TABLE should accept empty columns "()"
            //And then, an execution error should be given if a CreateTable without columns is executed
            const string createTablePattern = @"CREATE TABLE (\w+) \((\w+\s(?:String|Int|Double)(?:,\w+\s(?:String|Int|Double))*)?\)";//fabian
            
            const string updateTablePattern = null; //julen
            
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
            Match match = Regex.Match(miniSQLQuery, createTablePattern);
            if (match.Success) //Has there been a match?
            {
                if (match.Groups[2].Value == null)
                {
                    //PREGUNTAR QUÉ HACER SI LAS COLUMNAS ESTÁN NULL
                    return null;
                }
                String[] cols = match.Groups[2].Value.Split(',');
                List<ColumnDefinition> columnas = new List<ColumnDefinition>();
                foreach (String s in cols)
                {
                    String[] separados = s.Split(' ');
                    String nombre = separados[0];
                    String tipo = separados[1];
                    ColumnDefinition rcol = null;
                    if (tipo.Equals("String"))
                    {
                        rcol = new ColumnDefinition(ColumnDefinition.DataType.String, nombre);
                    }
                    if (tipo.Equals("Int"))
                    {
                        rcol = new ColumnDefinition(ColumnDefinition.DataType.Int, nombre);
                    }
                    if (tipo.Equals("Double"))
                    {
                        rcol = new ColumnDefinition(ColumnDefinition.DataType.Double, nombre);
                    }
                    columnas.Add(rcol);

                }
                return new CreateTable(match.Groups[1].Value, columnas);
            }
            else
            {
                Console.WriteLine("No matches found");
            }

           Match matchSelect = Regex.Match(miniSQLQuery, selectPattern);

           
                if (matchSelect.Success)
                {
                string columns = matchSelect.Groups[1].Value;
                string tableName = matchSelect.Groups[2].Value;
                List<string> columnList = CommaSeparatedNames(columns);
                Condition condition = null;
               
                if (matchSelect.Groups[4].Success)
                {
                    string conditionColumn = matchSelect.Groups[4].Value;
                    string conditionOperator = matchSelect.Groups[5].Value;
                    string conditionValue = matchSelect.Groups[6].Value;

                    condition = new Condition(conditionColumn, conditionOperator, conditionValue);
                }

                    return new Select(tableName, columnList, condition);
                }

            Match match = Regex.Match(miniSQLQuery, insertPattern);
            
            
            match = Regex.Match(miniSQLQuery, insertPattern);
            if (match.Success)
            {
                if(match.Length != miniSQLQuery.Length) { return null; }
                string toFilter, toSplit="";
                bool copying = false;
                toFilter = match.Groups[2].Value;
                for (int i = 0; i < toFilter.Length; i++)
                {
                    if (toFilter[i] == '\'')
                    {
                        copying = !copying;
                    }
                    else if (copying)
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
            if (match.Success)
            {
                if (match.Length != miniSQLQuery.Length) { return null; }
                return new Delete(match.Groups[1].Value, new Condition(match.Groups[2].Value, match.Groups[3].Value, match.Groups[4].Value));
            }
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
