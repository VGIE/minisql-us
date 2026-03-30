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
            Assert.False(obj is Insert);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("INSERT INTO esnulo VALUES ('','0')");
            Assert.True(obj is Insert);
            obj = MiniSQLParser.Parse("INSERT tabla VALUES (1,2);");
            Assert.False(obj is Insert);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("INSERT INTO heyy    VALUES    ('hola','2','3')  ");
            Assert.False(obj is Insert);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("INSERT INTO heyy    VALUES    ('hola','2','3')");
            Assert.True(obj is Insert);
            obj = MiniSQLParser.Parse("INSERT INTO table VALUES ('2','19,)");
            Assert.False(obj is Insert);
            Assert.True(obj is null);

            obj = MiniSQLParser.Parse("INSERT INTO Usuario VALUES ('Maik','Tower','14','-1.3')");
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

            obj = MiniSQLParser.Parse("DELETE FROM esnulo WHERE nombre = ''");
            Assert.False(obj is Delete);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("DELETE FROM esnulo WHERE nombre=''");
            Assert.True(obj is Delete);
            obj = MiniSQLParser.Parse("DELETE FROM tabla WHERE anyo=2");
            Assert.False(obj is Delete);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("DELETE FROM heyy    WHERE    name<'Rodolfo'  ");
            Assert.False(obj is Delete);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("DELETE FROM heyy    WHERE    name<'Rodolfo'");
            Assert.True(obj is Delete);
            obj = MiniSQLParser.Parse("DELETE FROM table WHERE AGE>'19");
            Assert.False(obj is Delete);
            Assert.True(obj is null);

            obj = MiniSQLParser.Parse("DELETE FROM Usuario WHERE nombre='Maik'");
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

            // Caso NO VÁLIDO: Falta el SET
            obj = MiniSQLParser.Parse("UPDATE Personas Edad='30' WHERE Nombre='Morty'");
            Assert.False(obj is Update);
            Assert.True(obj is null);

            // Caso NO VÁLIDO: Valores sin comillas
            obj = MiniSQLParser.Parse("UPDATE Personas SET Edad=30 WHERE Nombre=Morty");
            Assert.False(obj is Update);
            Assert.True(obj is null);

            obj = MiniSQLParser.Parse("UPDATE Personas SET Edad='30' WHERE Nombre = 'Morty'");
            Assert.False(obj is Update);
            Assert.True(obj is null);

            obj = MiniSQLParser.Parse("UPDATE Tabla SET Columna='Valor' WHERE Id='1'");
            Assert.True(obj is Update);

            obj = MiniSQLParser.Parse("UPDATE Personas SET Edad='30',Pelo='Rubio' WHERE Nombre='Morty'");
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
            obj = MiniSQLParser.Parse("SELECT dni FROM profesor WHERE edad>'60'");
            Assert.True(obj is Select);
            obj = MiniSQLParser.Parse("SELECT FROM profesor WHERE edad>'60'");
            Assert.False(obj is Select);
            Assert.Null(obj);
            obj = MiniSQLParser.Parse("Select nombre where dni = '123456'");
            Assert.False(obj is Select);
            Assert.Null(obj);
            obj = MiniSQLParser.Parse("SELECT nombre WHERE dni='123456'");
            Assert.False(obj is Select);
            Assert.Null(obj);



        }
        [Fact]
        public void CreateTableTest()
        {
            Object obj;

            obj = MiniSQLParser.Parse("CREATE TABLE Coches (Marca TEXT,Modelo TEXT)");
            Assert.True(obj is CreateTable);
            obj = MiniSQLParser.Parse("CREATE TABLE Coches ()");
            Assert.True(obj is CreateTable);
            obj = MiniSQLParser.Parse("CREATE TABLE Coches (Marca ,Modelo TEXT)");
            Assert.False(obj is CreateTable);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("CREATE TABLE Coches (Marca TEXT,Modelo TEXT);");
            Assert.False(obj is CreateTable);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("CREATE  TABLE Coches (Marca TEXT  ,Modelo TEXT) ");
            Assert.False(obj is CreateTable);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("CREATE Table Coches (Marca TEXT  ,Modelo TEXT)");
            Assert.False(obj is CreateTable);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("CREATE TABLE Coches (,Modelo TEXT)");
            Assert.False(obj is CreateTable);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("CREATE TABLE Coches (Marca TEXT,)");
            Assert.False(obj is CreateTable);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("CREATE TABLE        Coches          (Marca             TEXT,Modelo TEXT)");
            Assert.True(obj is CreateTable);
            obj = MiniSQLParser.Parse("CREATE     TABLE       Coches        (Marca                     TEXT)");
            Assert.True(obj is CreateTable);

            obj = MiniSQLParser.Parse("CREATE TABLE Coches (Marca TEXT,Modelo TEXT,Ano INT)");
            Assert.True(obj is CreateTable);
            CreateTable create = (CreateTable)obj;

            Assert.Equal("Coches", create.Table);
            List<ColumnDefinition> cols = create.ColumnsParameters;
            List<String> colsNomb = new List<String>();
            foreach (ColumnDefinition c in cols)
            {
                colsNomb.Add(c.Name);
            }
            Assert.Equal("Marca", colsNomb[0]);
            Assert.Equal("Modelo", colsNomb[1]);
            Assert.Equal("Ano", colsNomb[2]);
        }


        [Fact]
        public void DropTableTest()
        {
            Object obj;

            obj = MiniSQLParser.Parse("DROP    TABLE     Coches");
            Assert.True(obj is DropTable);
            obj = MiniSQLParser.Parse("DROP TABLE Coches");
            Assert.True(obj is DropTable);
            obj = MiniSQLParser.Parse("DROP TABLE Coches;");
            Assert.False(obj is DropTable);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("DROP;TABLE coches");
            Assert.False(obj is DropTable);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("Drop TABLE coches");
            Assert.False(obj is DropTable);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("DROP Table Coches");
            Assert.False(obj is DropTable);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("DROP TABLE");
            Assert.False(obj is DropTable);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("TABLE Coches");
            Assert.False(obj is DropTable);
            Assert.True(obj is null);


            obj = MiniSQLParser.Parse("DROP TABLE Coches");
            Assert.True(obj is DropTable);
            DropTable drop = (DropTable)obj;

            Assert.Equal("Coches", drop.Table);
        }

        [Fact]
        public void addUserTest()
        {
            Object obj;

            obj = MiniSQLParser.Parse("ADD USER (A, A, A);");
            Assert.False(obj is AddUser);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("ADD USER (A, A, A)");
            Assert.False(obj is AddUser);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("ADD USER (A,A,A)");
            Assert.True(obj is AddUser);
            obj = MiniSQLParser.Parse("ADD USER (A2,23,1)");
            Assert.False(obj is AddUser);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("ADD      USER     (A,A,A)   ");
            Assert.False(obj is AddUser);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("ADD      USER     (A,A,A)");
            Assert.True(obj is AddUser);
            obj = MiniSQLParser.Parse("ADD USER (A,A,)");
            Assert.False(obj is AddUser);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("ADD USER ('A','A','a')");
            Assert.False(obj is AddUser);
            Assert.True(obj is null);

            //TODO DEADLINE 5


        }

        [Fact]
        public void deleteUserTest()
        {
            Object obj;

            obj = MiniSQLParser.Parse("DELETE USER a;");
            Assert.False(obj is DeleteUser);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("DELETE USER a");
            Assert.True(obj is DeleteUser);
            obj = MiniSQLParser.Parse("DELETE USER a2");
            Assert.False(obj is DeleteUser);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("DELETE     USER     a   ");
            Assert.False(obj is DeleteUser);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("DELETE     USER     a");
            Assert.True(obj is DeleteUser);
            obj = MiniSQLParser.Parse("DELETE USER ");
            Assert.False(obj is DeleteUser);
            Assert.True(obj is null);
            obj = MiniSQLParser.Parse("DELETE USER 'a'");
            Assert.False(obj is DeleteUser);
            Assert.True(obj is null);

            //TODO DEADLINE 5


        }
    }
}
