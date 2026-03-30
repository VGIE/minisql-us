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
            const string selectPattern = @"SELECT\s+(\w+(?:,\w+)*)\s+FROM\s+(\w+)(\s+WHERE\s+(\w+)([=<>])('([^']*)'))?"; //Mikel

            const string insertPattern = @"INSERT\s+INTO\s+(\w+)\s+VALUES\s+\(((?:'([^']*)',)*(?:'([^']*)'))\)"; //kaiet

            const string dropTablePattern = @"DROP\s+TABLE\s+(\w+)"; //fabian

            //Note: The parsing of CREATE TABLE should accept empty columns "()"
            //And then, an execution error should be given if a CreateTable without columns is executed

            const string updateTablePattern = @"UPDATE\s+(\w+)\s+SET\s+((?:\w+='[^']*')(?:,\w+='[^']*')*)\s+WHERE\s+(\w+)(=|<|>)'([^']*)'"; //Julen

            const string createTablePattern = @"CREATE\s+TABLE\s+(\w+)\s+\((\w+\s+(?:TEXT|INT|DOUBLE)(?:,\w+\s+(?:TEXT|INT|DOUBLE))*)?\)"; //fabian

            const string deletePattern = @"DELETE\s+FROM\s+(\w+)\s+WHERE\s+(\w+)(<|>|=)'([^']*)'"; //kaiet



            //TODO DEADLINE 4
            const string createSecurityProfilePattern = null; //mikel

            const string dropSecurityProfilePattern = null; //mikel

            const string grantPattern = null; //julen

            const string revokePattern = @"REVOKE\s+(DELETE|INSERT|SELECT|UPDATE)\s+ON\s+(\w+)\s+TO\s+([A-Za-z]+)"; //fabian

            const string addUserPattern = @"ADD\s+USER\s+\(([A-Za-z]+),([A-Za-z]+),([A-Za-z]+)\)"; //kaiet

            const string deleteUserPattern = @"DELETE\s+USER\s+([A-Za-z]+)"; //kaiet


            //TODO DEADLINE 2
            //Parse query using the regular expressions above one by one. If there is a match, create an instance of the query with the parsed parameters
            //For example, if the query is a "SELECT ...", there should be a match with selectPattern. We would create and return an instance of Select
            //initialized with the table name, the columns, and (possibly) an instance of Condition.
            //If there is no match, it means there is a syntax error. We will return null.

            Match match;

            match = Regex.Match(miniSQLQuery, createTablePattern);
            if (match.Success)
            {
                if (match.Length != miniSQLQuery.Length) { return null; }

                List<ColumnDefinition> columnas = new List<ColumnDefinition>();
                if (match.Groups[2].Value == null || match.Groups[2].Value.Length == 0 || match.Groups[2].Value.Length == 1)
                {
                    //PREGUNTAR QUÉ HACER SI LAS COLUMNAS ESTÁN NULL
                    return new CreateTable(match.Groups[1].Value, columnas);
                }
                else
                {
                    String[] cols = match.Groups[2].Value.Split(',');
                    foreach (String s in cols)
                    {
                        String[] separados = s.Split(' ');
                        String nombre = separados[0];
                        String tipo = separados[separados.Length-1];
                        ColumnDefinition rcol = null;
                        if (tipo.Equals("TEXT"))
                        {
                            rcol = new ColumnDefinition(ColumnDefinition.DataType.String, nombre);
                        }
                        if (tipo.Equals("INT"))
                        {
                            rcol = new ColumnDefinition(ColumnDefinition.DataType.Int, nombre);
                        }
                        if (tipo.Equals("DOUBLE"))
                        {
                            rcol = new ColumnDefinition(ColumnDefinition.DataType.Double, nombre);
                        }
                        columnas.Add(rcol);
                    }
                    return new CreateTable(match.Groups[1].Value, columnas);
                }
            }
           

            match = Regex.Match(miniSQLQuery, dropTablePattern);
            if (match.Success)
            {
                if (match.Length != miniSQLQuery.Length) { return null; }

                if (match.Groups[1].Value.Length == 0 || match.Groups[1].Value.Length == 1)
                {
                    return null;
                }
                else
                {
                    String nombreTabla = match.Groups[1].Value;
                    return new DropTable(match.Groups[1].Value);
                }
            }
            


            match = Regex.Match(miniSQLQuery, selectPattern);
            if (match.Success)
            {
                if (match.Length != miniSQLQuery.Length) { return null; }
                string columns = match.Groups[1].Value;
                string tableName = match.Groups[2].Value;
                List<string> columnList = CommaSeparatedNames(columns);
                Condition condition = null;

                if (match.Groups[3].Success)
                {
                    string conditionColumn = match.Groups[3].Value;
                    string conditionOperator = match.Groups[4].Value;
                    string conditionValue = match.Groups[5].Value;

                    condition = new Condition(conditionColumn, conditionOperator, conditionValue);
                }

                return new Select(tableName, columnList, condition);
            }

            match = Regex.Match(miniSQLQuery, insertPattern);
            if (match.Success == true)
            {
                if (match.Length != miniSQLQuery.Length) { return null; }
                string toFilter, toSplit = "";
                bool copying = false;
                toFilter = match.Groups[2].Value;
                for (int i = 0; i < toFilter.Length; i++)
                {
                    if (toFilter[i] == '\'')
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

                string toFilter = setString;
                string toSplit = "";
                bool copying = false;

                for (int i = 0; i < toFilter.Length; i++)
                {
                    if (toFilter[i] == '\'')
                    {
                        toSplit += toFilter[i];
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
                    else if (toFilter[i] == '=')
                    {
                        toSplit += "=";
                    }
                    else if (toFilter[i] != ' ')
                    {
                        toSplit += toFilter[i];
                    }
                }

                List<string> asignaciones = CommaSeparatedNames(toSplit);
                List<SetValue> setValues = new List<SetValue>();

                foreach (string asignacion in asignaciones)
                {
                    string[] partes = asignacion.Split("=");

                    if (partes.Length == 2)
                    {
                        string columna = partes[0];
                        string valor = partes[1];

                        if (valor.StartsWith("'") && valor.EndsWith("'"))
                        {
                            string valorLimpio = valor.Substring(1, valor.Length - 2);
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

            match = Regex.Match(miniSQLQuery, addUserPattern);
            if (match.Success == true)
            {
                if (match.Length != miniSQLQuery.Length)
                {
                    return null;
                }

                return (new AddUser(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value));
            }

            match = Regex.Match(miniSQLQuery, deleteUserPattern);
            if (match.Success == true)
            {
                if (match.Length != miniSQLQuery.Length)
                {
                    return null;
                }

                return (new DeleteUser(match.Groups[1].Value));
            }

            match = Regex.Match(miniSQLQuery, revokePattern);
            if (match.Success)
            {
                if (match.Length != miniSQLQuery.Length) { return null; }

                return (new Revoke(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value));
            }

            return null;
        }


        static List<string> CommaSeparatedNames(string text)
        {
            string[] textParts = text.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
            List<string> commaSeparator = new List<string>();
            for (int i = 0; i < textParts.Length; i++)
            {
                commaSeparator.Add(textParts[i]);
            }
            return commaSeparator;
        }

    }
}
