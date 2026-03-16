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

        public void SelectCase()
        {
            Object obj;

            obj = MiniSQLParser.Parse("SELECT dni FROM alumnos WHERE nombre='Charlicius'");
            Assert.True(obj is Select);

            Select select = (Select)obj;
            Assert.Equal("alumnos", select.Table);
            Assert.Equal(1, select.Columns.Count);
            Assert.Equal("dni", select.Columns[0]);


            obj = MiniSQLParser.Parse("SELECT dni,sexo,apellido FROM alumnos WHERE nombre='Charlicius'");
            Assert.True(obj is Select);
            obj = MiniSQLParser.Parse("SELECT dni FROM profesor");
            Assert.True(obj is Select);
            obj = MiniSQLParser.Parse("SELECT dni FROM profesor WHERE edad>60");
            Assert.True(obj is Select);
            obj = MiniSQLParser.Parse("SELECT FROM profesor WHERE edad>60");
            Assert.False(obj is Select);
            Assert.Null(obj);
            obj = MiniSQLParser.Parse("Select nombre where dni = '123456'");
            Assert.False(obj is Select);
            Assert.Null(obj);
            obj = MiniSQLParser.Parse("SELECT nombre WHERE dni = '123456'");
            Assert.False(obj is Select);
            Assert.Null(obj);



        }
        [Fact]
        public void CreateTableTest()
        {
            Object obj;

            obj = MiniSQLParser.Parse("CREATE TABLE Coches (Marca String,Modelo String)");
            Assert.True(obj is CreateTable);
            obj = MiniSQLParser.Parse("CREATE TABLE Coches ()");
            Assert.True(obj is CreateTable);
            obj = MiniSQLParser.Parse("CREATE TABLE Coches (Marca ,Modelo String)");
            Assert.False(obj is CreateTable);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("CREATE TABLE Coches (Marca String,Modelo String);");
            Assert.False(obj is CreateTable);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("CREATE  TABLE Coches (Marca String  ,Modelo String) ");
            Assert.False(obj is CreateTable);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("CREATE Table Coches (Marca String  ,Modelo String)");
            Assert.False(obj is CreateTable);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("CREATE TABLE Coches (,Modelo String)");
            Assert.False(obj is CreateTable);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("CREATE TABLE Coches (Marca String,)");
            Assert.False(obj is CreateTable);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("CREATE TABLE        Coches          (Marca             String,Modelo String)");
            Assert.True(obj is CreateTable);

            obj = MiniSQLParser.Parse("CREATE TABLE Coches (Marca String,Modelo String,Ano Int)");
            Assert.True(obj is CreateTable);
            CreateTable create = (CreateTable)obj;

            Assert.Equal("Coches", create.Table);
            List<ColumnDefinition> cols = create.ColumnsParameters;
            List<String> colsNomb = new List<String>();
            foreach(ColumnDefinition c in cols)
            {
                colsNomb.Add(c.Name);
            }
            Assert.Equal("Marca", colsNomb[0]);
            Assert.Equal("Modelo", colsNomb[1]);
            Assert.Equal("Ano", colsNomb[2]);
        }



    }

}
