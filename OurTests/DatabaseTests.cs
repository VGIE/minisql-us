using DbManager;
using DbManager.Parser;
using System.ComponentModel.DataAnnotations;

namespace OurTests
{
    public class UnitTest1
    {
        //TODO DEADLINE 1B : Create your own tests for Database
        
        [Fact]
        public void addTableANDSearchByName()
        {
            Database db = new Database("dbTest", "1234");

            List<ColumnDefinition> cd = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "age"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "city")
            };
            Table t = new Table("HOLA", cd);
            
            //addTable
            Assert.True(db.AddTable(new Table("testTable", cd)));
            Assert.True(db.AddTable(t));
            Assert.False(db.AddTable(null));

            //tableByName
            Assert.Null(db.TableByName("wowo"));
            Assert.Equal(t, db.TableByName("HOLA"));
        }


        
        [Fact]
        public void createOrDropTableTest()
        {
            Database db = new Database("dbTest", "1234");

            List<ColumnDefinition> cd = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "age"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "city")
            };

            //select:
            Assert.True(db.CreateTable("testTable", cd));
            Assert.Equal(Constants.CreateTableSuccess, db.LastErrorMessage);

            Assert.False(db.CreateTable("", cd));

            Assert.False(db.CreateTable("Hola", new List<ColumnDefinition>()));
            Assert.Equal(Constants.DatabaseCreatedWithoutColumnsError, db.LastErrorMessage);
            Assert.False(db.CreateTable("Hola", null));
            Assert.Equal(Constants.DatabaseCreatedWithoutColumnsError, db.LastErrorMessage);

            cd.Add(new ColumnDefinition(ColumnDefinition.DataType.Int, "alturaCM"));
            Assert.False(db.CreateTable("testTable", cd));
            Assert.Equal(Constants.TableAlreadyExistsError, db.LastErrorMessage);

            //drop:
            Assert.False(db.DropTable("queso"));
            Assert.Equal(Constants.TableDoesNotExistError, db.LastErrorMessage);

            Assert.True(db.DropTable("testTable"));
            Assert.Equal(Constants.DropTableSuccess, db.LastErrorMessage);
        }

        [Fact]
        public void deleteWhereTest()
        {
            Database db = Database.CreateTestDatabase();
            string tableName = Table.TestTableName;

            bool resultadoNull = db.DeleteWhere(tableName, null);
            Assert.False(resultadoNull);

            Condition condicion1 = new Condition("Age", ">", "10"); 
            bool resultado1 = db.DeleteWhere("Rickinillos", condicion1);
            Assert.False(resultado1);
            Assert.Equal(Constants.TableDoesNotExistError, db.LastErrorMessage);

            Condition condicion2 = new Condition("Queso Rellenito", "=", "10"); 
            bool resultado2 = db.DeleteWhere(tableName, condicion2);
            Assert.False(resultado2);
            Assert.Equal(Constants.ColumnDoesNotExistError, db.LastErrorMessage);

            Condition condicion3 = new Condition("Age", ">", "50");
            bool resultado3 = db.DeleteWhere(tableName, condicion3);
            Assert.True(resultado3);
            Assert.Equal(Constants.DeleteSuccess, db.LastErrorMessage);

            Table tabla = db.TableByName(tableName);
            String nombre = tabla.GetRow(0).GetValue(Table.TestColumn1Name);
            Assert.Equal("Rodolfo", nombre); Assert.Equal(1, tabla.NumRows());

            Condition condicion4 = new Condition("Age", ">", "100");
            bool resultado4 = db.DeleteWhere(tableName, condicion4);
            Assert.True(resultado4);
            Assert.Equal(Constants.DeleteSuccess, db.LastErrorMessage);
            Assert.Equal(1, tabla.NumRows());
          
    }

    [Fact]
    public void updateTest()
        {
            Database db = Database.CreateTestDatabase();
            string tableName = Table.TestTableName;

            bool resultadoNull = db.Update(tableName, null, null);
            Assert.False(resultadoNull);

            bool resultadoVacio = db.Update(tableName, new List<SetValue>(), null);
            Assert.False(resultadoVacio);

            List<SetValue> listaCambios = new List<SetValue>();
            SetValue cambio = new SetValue(Table.TestColumn3Name,"99");
            listaCambios.Add(cambio);
            
            bool resultado1 = db.Update("Rickinillos", listaCambios, null);
            Assert.False(resultado1);
            Assert.Equal(Constants.TableDoesNotExistError, db.LastErrorMessage);

            Condition condicion1 = new Condition("ColorPelo", "=", "Rubio");
            bool resultado2 = db.Update(tableName, listaCambios, condicion1);
            Assert.False(resultado2);
            Assert.Equal(Constants.ColumnDoesNotExistError, db.LastErrorMessage);

            List<SetValue> listaCambios2 = new List<SetValue>();
            SetValue cambio2 = new SetValue("Rickinillos", "Morty");
            listaCambios2.Add(cambio2);
            bool resultado3 = db.Update(tableName, listaCambios2, null);
            Assert.False(resultado3);
            Assert.Equal(Constants.ColumnDoesNotExistError, db.LastErrorMessage);

            Condition condicion2 = new Condition(Table.TestColumn1Name, "=", "Maider");
            bool resultado4 = db.Update(tableName, listaCambios, condicion2);
            Assert.True(resultado4);
            Assert.Equal(Constants.UpdateSuccess, db.LastErrorMessage);

            Table tabla = db.TableByName(tableName);
            Assert.Equal("25", tabla.GetRow(0).GetValue(Table.TestColumn3Name));
            Assert.Equal("99", tabla.GetRow(1).GetValue(Table.TestColumn3Name));
            Assert.Equal("51", tabla.GetRow(2).GetValue(Table.TestColumn3Name));
        
        }

        [Fact]
        public void InsertandSelectTests()
        {
            //INSERT

            Database db = Database.CreateTestDatabase();
            List<ColumnDefinition> columnas = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String,"Nombre"),
                new ColumnDefinition(ColumnDefinition.DataType.Int,"Edad")

            };

            Table tabla = new Table("TablaTest", columnas);
            db.AddTable(tabla);

            Assert.False(db.Insert("TablaInvent",null));
            Assert.Equal(Constants.TableDoesNotExistError, db.LastErrorMessage);

            Assert.False(db.Insert("TablaTest", null));
            Assert.Equal(Constants.ColumnCountsDontMatch, db.LastErrorMessage);

            List<string> valoresIncompletos = new List<string>()
            {
                "Farlopo"
            };

            Assert.False(db.Insert("TablaTest",valoresIncompletos));
            Assert.Equal(Constants.ColumnCountsDontMatch, db.LastErrorMessage);



            List<string> valoresSobra = new List<string>()
            {
                "Charles","92","Xtra"
            };

            Assert.False(db.Insert("TablaTest",valoresSobra));
            Assert.Equal(Constants.ColumnCountsDontMatch, db.LastErrorMessage);

            List<string> valoresCorrectos = new List<string>()
            {
                "Tijuano","90"
            };

            Assert.True(db.Insert("TablaTest",valoresCorrectos));
            Assert.Equal(1, tabla.NumRows());
            Assert.Equal("Tijuano", tabla.GetRow(0).GetValue("Nombre"));
            Assert.Equal("90", tabla.GetRow(0).GetValue("Edad"));
            Assert.Equal(Constants.InsertSuccess, db.LastErrorMessage);

            //SELECT

            db.Insert("TablaTest", new List<string> { "Carlos", "22" });

            //tabla no existe
            
            List<string> columnasString = new List<string>()
            {
                columnas[0].Name,
                columnas[1].Name
            };


            Table resNull = db.Select("TablaInvent",columnasString , null);
            Assert.Null(resNull);
            Assert.Equal(Constants.TableDoesNotExistError, db.LastErrorMessage);

            List<string> colMalas = new List<string>()
            {
                "Nombre",
                "columnaInvent"
            };

            Table resColMal = db.Select("TablaTest", colMalas, null);
            Assert.Null(resColMal);
            Assert.Equal(Constants.ColumnDoesNotExistError, db.LastErrorMessage);

            Condition c1 = new Condition("Edad", "=", "22");
            Table ResCorrecto = db.Select("TablaTest", columnasString, c1);

            Assert.NotNull(ResCorrecto);
            Assert.Equal(1, ResCorrecto.NumRows());
            Assert.Equal("Carlos", ResCorrecto.GetRow(0).GetValue("Nombre"));
            Assert.Equal("22", ResCorrecto.GetRow(0).GetValue("Edad"));


            Table resEntera = db.Select("TablaTest", columnasString, null);
            Assert.NotNull(resEntera);
            Assert.Equal(2, resEntera.NumRows());
            Assert.Equal("Tijuano", resEntera.GetRow(0).GetValue("Nombre"));
            Assert.Equal("Carlos", resEntera.GetRow(1).GetValue("Nombre"));
            Assert.Equal("90", resEntera.GetRow(0).GetValue("Edad"));
            Assert.Equal("22", resEntera.GetRow(1).GetValue("Edad"));

            Condition cvacia = new Condition("Edad", "=", "100");
            Table resVacio = db.Select("TablaTest", columnasString, cvacia);

            Assert.NotNull(resVacio);
            Assert.Equal(0, resVacio.NumRows());


        }

        [Fact]
        public void SaveAndLoadTest()
        {
            Database db = Database.CreateTestDatabase();

            Assert.False(db.Save(""));
            Assert.False(db.Save(null));

            db.Save("guardadoTest");
            Assert.True(Database.AreEqual(db, Database.Load("guardadoTest", Database.AdminUsername, Database.AdminPassword)));
        }
    }
}