using DbManager;
using DbManager.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurTests
{
    public class ParserTest
    {
        [Fact]
        public void InsertTest()
        {
            Object obj;

            obj = MiniSQLParser.Parse("INSERT INTO esnulo VALUES ('', '0');");
            Assert.False(obj is Insert);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("INSERT INTO esnulo VALUES ('', '0')");
            Assert.True(obj is Insert);
            obj = MiniSQLParser.Parse("INSERT tabla VALUES (1, 2);");
            Assert.False(obj is Insert);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("INSERT INTO heyy    VALUES    ('hola','2','3')  ");
            Assert.False(obj is Insert);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("INSERT INTO heyy    VALUES    ('hola','2','3')");
            Assert.True(obj is Insert);
            obj = MiniSQLParser.Parse("INSERT INTO table VALUES ('2', '19,)");
            Assert.False(obj is Insert);
            Assert.True(obj is null);

            obj = MiniSQLParser.Parse("INSERT INTO Usuario VALUES ('Maik', 'Tower', '14', '-1.3')");
            Assert.True(obj is Insert);
            Insert insert = (Insert)obj;

            Assert.Equal("Usuario", insert.Table);
            List<String> values = insert.Values;
            Assert.Equal("Maik", values[0]);
            Assert.Equal("Tower", values[1]);
            Assert.Equal("14", values[2]);
            Assert.Equal("-1.3", values[3]);
        }

        [Fact]
        public void DeleteTest()
        {
            Object obj;

            obj = MiniSQLParser.Parse("DELETE FROM esnulo WHERE nombre = '';");
            Assert.False(obj is Delete);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("DELETE FROM esnulo WHERE nombre = ''");
            Assert.True(obj is Delete);
            obj = MiniSQLParser.Parse("DELETE FROM tabla WHERE anyo = 2");
            Assert.False(obj is Delete);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("DELETE FROM heyy    WHERE    name < 'Rodolfo'  ");
            Assert.False(obj is Delete);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("DELETE FROM heyy    WHERE    name < 'Rodolfo'");
            Assert.True(obj is Delete);
            obj = MiniSQLParser.Parse("DELETE FROM table WHERE AGE > '19");
            Assert.False(obj is Delete);
            Assert.True(obj is null);

            obj = MiniSQLParser.Parse("DELETE FROM Usuario WHERE nombre = 'Maik'");
            Assert.True(obj is Delete);
            Delete delete = (Delete)obj;

            Assert.Equal("Usuario", delete.Table);
            Assert.Equal("nombre", delete.Where.ColumnName);
            Assert.Equal("=", delete.Where.Operator);
            Assert.Equal("Maik", delete.Where.LiteralValue);
        }


        [Fact]
        public void UpdateTest()
        {
            Object obj;

            obj = MiniSQLParser.Parse("UPDATE Personas Edad='30' WHERE Nombre='Morty'");
            Assert.False(obj is Update);

            obj = MiniSQLParser.Parse("UPDATE Personas SET Edad=30 WHERE Nombre=Morty");
            Assert.False(obj is Update);

            obj = MiniSQLParser.Parse("UPDATE Personas SET Edad='30' WHERE Nombre = 'Morty'");
            Assert.False(obj is Update);

            obj = MiniSQLParser.Parse("UPDATE Tabla SET Columna='Valor' WHERE Id='1'");
            Assert.True(obj is Update);

            obj = MiniSQLParser.Parse("UPDATE Personas SET Edad='30', Pelo='Rubio' WHERE Nombre='Morty'");
            Assert.True(obj is Update);
            
            Update updateObj = (Update)obj;

            Assert.Equal("Personas", updateObj.Table);

            List<SetValue> columnasActualizar = updateObj.Columns;
            Assert.Equal(2, columnasActualizar.Count);
            
            Assert.Equal("Edad", columnasActualizar[0].ColumnName);
            Assert.Equal("30", columnasActualizar[0].Value);

            Assert.Equal("Pelo", columnasActualizar[1].ColumnName);
            Assert.Equal("Rubio", columnasActualizar[1].Value);

            Condition condicionWhere = updateObj.Where;
            Assert.Equal("Nombre", condicionWhere.ColumnName);
            Assert.Equal("=", condicionWhere.Operator);
            Assert.Equal("Morty", condicionWhere.LiteralValue);
        }

    }

}
