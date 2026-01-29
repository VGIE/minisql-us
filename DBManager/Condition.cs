using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Text;
using DbManager;

namespace DbManager
{
    public class Condition
    {
        public string ColumnName { get; private set; }
        public string Operator { get; private set; }
        public string LiteralValue { get; private set; }

        public Condition(string column, string op, string literalValue)
        {
            //TODO DEADLINE 1A: Initialize member variables
            column = this.ColumnName;
            op = this.Operator;
            literalValue = this.LiteralValue;
        }


        public bool IsTrue(string value, ColumnDefinition.DataType type)
        {
            //TODO DEADLINE 1A: return true if the condition is true for this value
            //Depending on the type of the column, the comparison should be different:
            //"ab" < "cd
            //"9" > "10"
            //9 < 10
            //Convert first the strings to the appropriate type and then compare (depending on the operator of the condition)
           
            
            if(type is string)
            {
                int comparacion = value.CompareTo(LiteralValue);
                if(comparacion == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }else if (type is double)
            {
                int valor = Int32.Parse(value);
                int valor2 = Int32.Parse(LiteralValue);
                int comparacion = valor.CompareTo(valor2);
                if (comparacion == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            else if(type is int)
            {
                 double valor = Double.Parse(value);
                double valor2 = Int32.Parse(LiteralValue);
                int comparacion = valor.CompareTo(valor2);
                if (comparacion == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
                 return false;

        }

    }

}



