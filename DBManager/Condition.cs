using System;
using System.Collections.Generic;
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

        private string mayorQue = ">";
        private string menorQue = "<";  
        private string igualQue = "=";

        public Condition(string column, string op, string literalValue)
        {
            //TODO DEADLINE 1A: Initialize member variables
             this.ColumnName = column ;
             this.Operator = op ;
             this.LiteralValue = literalValue;
        }


        public bool IsTrue(string value, ColumnDefinition.DataType type)
        {
            //TODO DEADLINE 1A: return true if the condition is true for this value
            //Depending on the type of the column, the comparison should be different:
            //"ab" < "cd
            //"9" > "10"
            //9 < 10
            //Convert first the strings to the appropriate type and then compare (depending on the operator of the condition)

            int resultado = 0;

            if (value == null)
            {
                return false;
            }

            if (type == ColumnDefinition.DataType.String)
            {
                resultado = value.CompareTo(LiteralValue);
            }
            else if (type == ColumnDefinition.DataType.Int)
            {
                int valor = Int32.Parse(value);
                int valor2 = Int32.Parse(LiteralValue);

                resultado = valor.CompareTo(valor2);
            }
            else if (type == ColumnDefinition.DataType.Double)
            {
                double valor = Double.Parse(value);
                double valor2 = Double.Parse(LiteralValue);

                resultado = valor.CompareTo(valor2);

            }
            if (Operator == mayorQue && resultado > 0)
            {
                return true;
            }else if (Operator == igualQue && resultado == 0)
            {
                return true;
            }else if((Operator == menorQue && resultado < 0))
            {
                return true;
            }
            else
            {
                return false;
            }
                
        }
    }
}



