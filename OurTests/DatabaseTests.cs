using DbManager;
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
    

    
}
}