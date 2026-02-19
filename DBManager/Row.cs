using DbManager.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DbManager
{
    public class Row
    {
        private List<ColumnDefinition> ColumnDefinitions = new List<ColumnDefinition>();
        public List<string> Values { get; set; }

        public Row(List<ColumnDefinition> columnDefinitions, List<string> values)
        {
            //TODO DEADLINE 1.A: Initialize member variables
             this.Values = new List<string>();

            if(columnDefinitions != null)
            {
              this.ColumnDefinitions=columnDefinitions;
            }
            if(values != null)
            {
                this.Values=values;
            }
            
        }
    
            
        
        public void SetValue(string columnName, string value)
        {
            //TODO DEADLINE 1.A: Given a column name and value, change the value in that column
            int contador = 0;
              foreach(ColumnDefinition item in ColumnDefinitions)
            {
                if (item.Name.Equals(columnName))
                {
                    while(Values.Count <= contador)
                    {
                        Values.Add(null);
                    }
                    Values[contador]=value;
                    return;
                }
                contador++;
            }
            
        }

        public string GetValue(string columnName)
        {
            //TODO DEADLINE 1.A: Given a column name, return the value in that column
            int contador = 0;
              foreach(ColumnDefinition item in ColumnDefinitions)
            {
                if (item.Name.Equals(columnName))
                {
                  if(Values.Count > contador)
                    {
                        return Values[contador];
                    }
                    else
                    {
                        return null;
                    }
                    
                }
                contador++;
            }
            
            return null;
            
        }

        public bool IsTrue(Condition condition)
        {
            //TODO DEADLINE 1.A: Given a condition (column name, operator and literal value, return whether it is true or not
            //for this row. Check Condition.IsTrue method
            

            int contador = 0;
                foreach(ColumnDefinition item in ColumnDefinitions)
                {
                    if (item.Name.Equals(condition.ColumnName))
                    {
                        if(contador < Values.Count)
                        {
                            return condition.IsTrue(Values[contador],item.Type);
                        }
                    }
                    contador++;
                }
            return false;
            
        }

        private const string Delimiter = ":";
        private const string DelimiterEncoded = "[SEPARATOR]";

        private static string Encode(string value)
        {
            //TODO DEADLINE 1.C: Encode the delimiter in value

            if (value.Equals(null)) { return value; }

            return value.Replace(Delimiter, DelimiterEncoded);

        }

        private static string Decode(string value)
        {
            //TODO DEADLINE 1.C: Decode the value doing the opposite of Encode()

            if (value.Equals(null)) { return value; }

            return value.Replace(DelimiterEncoded, Delimiter);

        }

        public string AsText()
        {
            //TODO DEADLINE 1.C: Return the row as string with all values separated by the delimiter

            String r = "";
            foreach (String s in Values)
            {
                if (s == null) { r += "" + Delimiter; }
                else
                {
                    r += Encode(s) + Delimiter;
                }

            }

            r.TrimEnd(':');

            return r;

        }

        public static Row Parse(List<ColumnDefinition> columns, string value)
        {
            //TODO DEADLINE 1.C: Parse a rowReturn the row as string with all values separated by the delimiter
            
            return null;
            
        }
    }
}
