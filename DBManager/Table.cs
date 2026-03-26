using DbManager.Parser;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Security.Cryptography;

namespace DbManager
{
    public class Table
    {
        private List<ColumnDefinition> ColumnDefinitions = new List<ColumnDefinition>();
        private List<Row> Rows = new List<Row>();

        public string Name { get; private set; } = null;

        public Table(string name, List<ColumnDefinition> columns)
        {
            //TODO DEADLINE 1.A: Initialize member variables
            ColumnDefinitions = columns;
            Name = name;
        }

        public Row GetRow(int i)
        {
            //TODO DEADLINE 1.A: Return the i-th row
            if (i > NumRows() - 1 || i < 0) { return null; }
            return Rows[i];

        }

        public void AddRow(Row row)
        {
            //TODO DEADLINE 1.A: Add a new row

            Rows.Add(row);

        }

        public int NumRows()
        {
            //TODO DEADLINE 1.A: Return the number of rows

            return Rows.Count;

        }

        public ColumnDefinition GetColumn(int i)
        {
            //TODO DEADLINE 1.A: Return the i-th column
            if (i > NumColumns() - 1 || i < 0) { return null; }
            return ColumnDefinitions[i];

        }

        public int NumColumns()
        {
            //TODO DEADLINE 1.A: Return the number of columns

            return ColumnDefinitions.Count;

        }

        public ColumnDefinition ColumnByName(string column)
        {
            //TODO DEADLINE 1.A: Return the number of columns
            if (ColumnDefinitions == null || column == null)
            {
                return null;
            }


            for (int i = 0; i < ColumnDefinitions.Count; i++)
            {
                if (ColumnDefinitions[i].Name.Equals(column))
                {
                    return ColumnDefinitions[i];
                }
            }

            return null;

        }
        public int ColumnIndexByName(string columnName)
        {
            //TODO DEADLINE 1.A: Return the zero-based index of the column named columnName
            if (ColumnDefinitions == null || columnName == null)
            {
                return -1;
            }
            for (int i = 0; i < ColumnDefinitions.Count; i++)
            {
                if (ColumnDefinitions[i].Name.Equals(columnName))
                {
                    return i;
                }
            }

            return -1;

        }


        public override string ToString()
        {
            //TODO DEADLINE 1.A: Return the table as a string. The format is specified in the documentation
            //Valid examples:
            //"['Name']{'Adolfo'}{'Jacinto'}" <- one column, two rows
            //"['Name','Age']{'Adolfo','23'}{'Jacinto','24'}" <- two columns, two rows
            //"" <- no columns, no rows
            //"['Name']" <- one column, no rows


            if (ColumnDefinitions != null && ColumnDefinitions.Count > 0)
            {
                string result = "[";
                ColumnDefinition last = ColumnDefinitions[ColumnDefinitions.Count - 1];

                for (int i = 0; i < ColumnDefinitions.Count; i++)
                {
                    ColumnDefinition c = ColumnDefinitions[i];
                    result = result + "'" + c.Name + "'";

                    if (i < ColumnDefinitions.Count - 1)
                    {
                        result = result + ",";
                    }
                }
                result = result + "]";
                if (Rows != null)
                {
                    foreach (Row row in Rows)
                    {
                        result = result + "{";
                        for (int i = 0; i < row.Values.Count; i++)
                        {
                            result = result + "'" + row.Values[i] + "'";
                            if (i < row.Values.Count - 1)
                            {
                                result = result + ",";
                            }
                        }
                        result = result + "}";
                    }
                }
                return result;
            }
            else
            {
                return "";
            }
        }

        public void DeleteIthRow(int row)
        {
            //TODO DEADLINE 1.A: Delete the i-th row. If there is no i-th row, do nothing
            if (!(row >= Rows.Count || row < 0))
            {
                Row toDelete = Rows[row];
                if (toDelete != null)
                {
                    Rows.RemoveAt(row);
                }
            }
        }

        private List<int> RowIndicesWhereConditionIsTrue(Condition condition)
        {
            //TODO DEADLINE 1.A: Returns the indices of all the rows where the condition is true. Check Row.IsTrue()

            List<int> result = new List<int>();

            if (Rows != null)
            {
                for (int i = 0; i < Rows.Count; i++)
                {
                    if (Rows[i].IsTrue(condition))
                    {
                        result.Add(i);
                    }
                }
            }
            return result;

        }

        public void DeleteWhere(Condition condition)
        {
            //TODO DEADLINE 1.A: Delete all rows where the condition is true. Check RowIndicesWhereConditionIsTrue()
            List<int> toDelete = this.RowIndicesWhereConditionIsTrue(condition);
            toDelete.Sort();
            int num = toDelete.Count - 1;
            for (int i = num; i >= 0; i--)
            {
                Rows.RemoveAt(toDelete[i]);
            }

        }

        public Table Select(List<string> columnNames, Condition condition)
        {
            //TODO DEADLINE 1.A: Return a new table (with name 'Result') that contains the result of the select. The condition
            //may be null (if no condition, all rows should be returned). This is the most difficult method in this class

            if (columnNames == null) { return null; }

            List<ColumnDefinition> columns = new List<ColumnDefinition>();
            List<int> indices = new List<int>();
            foreach (String col in columnNames)
            {
                for (int i = 0; i < ColumnDefinitions.Count; i++)
                {
                    if (ColumnDefinitions[i].Name.Equals(col))
                    {
                        columns.Add(ColumnDefinitions[i]);
                        indices.Add(i);
                        break;
                    }
                }
            }
            if (columns == null || columns.Count == 0)
            {
                Table tablaError = new Table("Result", columns);
                return tablaError;
            }
            Table Result = new Table("Result", columns);

            List<int> resultRowsInd = new List<int>();
            if (condition != null)
            {
                resultRowsInd = RowIndicesWhereConditionIsTrue(condition);
            }
            else
            {
                for (int i = 0; i < Rows.Count; i++)
                {
                    resultRowsInd.Add(i);
                }
            }

            foreach (int indice in resultRowsInd)
            {
                List<String> valuesOrd = new List<String>();
                Row r = Rows[indice];
                foreach (int indiceCol in indices)
                {
                    valuesOrd.Add(r.Values[indiceCol]);
                }
                Result.AddRow(new Row(columns, valuesOrd));
            }
            return Result;

        }

        public bool Insert(List<string> values)
        {
            //TODO DEADLINE 1.A: Insert a new row with the values given. If the number of values is not correct, return false. True otherwise

            if (values != null && NumColumns() == values.Count)
            {
                Row inserting = new Row(ColumnDefinitions, values);
                this.AddRow(inserting);
                return true;
            }
            return false;

        }

        public bool Update(List<SetValue> setValues, Condition condition)
        {
            //TODO DEADLINE 1.A: Update all the rows where the condition is true using all the SetValues (ColumnName-Value). If condition is null,
            //return false, otherwise return true

            List<String> columnas = new List<String>();

            for (int i = 0; i < setValues.Count; i++)
            {
                columnas.Add(setValues[i].ColumnName);
            }

            if (condition != null)
            {
                List<int> resultRows = new List<int>();
                resultRows = RowIndicesWhereConditionIsTrue(condition);
                foreach (int i in resultRows)
                {
                    for (int j = 0; j < columnas.Count; j++)
                    {
                        Rows[i].SetValue(columnas[j], setValues[j].Value);
                    }

                }
                return true;

            }

            return false;


        }



        //Only for testing purposes
        public const string TestTableName = "TestTable";
        public const string TestColumn1Name = "Name";
        public const string TestColumn2Name = "Height";
        public const string TestColumn3Name = "Age";
        public const string TestColumn1Row1 = "Rodolfo";
        public const string TestColumn1Row2 = "Maider";
        public const string TestColumn1Row3 = "Pepe";
        public const string TestColumn2Row1 = "1.62";
        public const string TestColumn2Row2 = "1.67";
        public const string TestColumn2Row3 = "1.55";
        public const string TestColumn3Row1 = "25";
        public const string TestColumn3Row2 = "67";
        public const string TestColumn3Row3 = "51";
        public const ColumnDefinition.DataType TestColumn1Type = ColumnDefinition.DataType.String;
        public const ColumnDefinition.DataType TestColumn2Type = ColumnDefinition.DataType.Double;
        public const ColumnDefinition.DataType TestColumn3Type = ColumnDefinition.DataType.Int;
        public static Table CreateTestTable(string tableName = TestTableName)
        {
            Table table = new Table(tableName, new List<ColumnDefinition>()
            {
                new ColumnDefinition(TestColumn1Type, TestColumn1Name),
                new ColumnDefinition(TestColumn2Type, TestColumn2Name),
                new ColumnDefinition(TestColumn3Type, TestColumn3Name)
            });
            table.Insert(new List<string>() { TestColumn1Row1, TestColumn2Row1, TestColumn3Row1 });
            table.Insert(new List<string>() { TestColumn1Row2, TestColumn2Row2, TestColumn3Row2 });
            table.Insert(new List<string>() { TestColumn1Row3, TestColumn2Row3, TestColumn3Row3 });
            return table;
        }

        public void CheckForTesting(List<List<string>> rows)
        {
            if (rows.Count != NumRows())
                throw new Exception($"The table has {NumRows()} rows and {rows.Count} were expected");
            int rowIndex = 0;
            foreach (List<string> row in rows)
            {
                if (GetRow(rowIndex).Values.Count != row.Count)
                    if (rows.Count != NumRows())
                        throw new Exception($"The {rowIndex}-th row has {GetRow(rowIndex).Values.Count} values and {row.Count} were expected");

                for (int columnIndex = 0; columnIndex < row.Count; columnIndex++)
                {
                    if (GetRow(rowIndex).Values[columnIndex] != row[columnIndex])
                        if (rows.Count != NumRows())
                            throw new Exception($"The [{rowIndex},{columnIndex}] element is {GetRow(rowIndex).Values[columnIndex]} instead of {row[columnIndex]}");
                }

                rowIndex++;
            }
        }
    }
}