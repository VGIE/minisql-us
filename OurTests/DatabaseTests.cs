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
    }
}