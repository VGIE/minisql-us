using DbManager;

namespace OurTests
{
    public class ColumnDefinitionsTests
    {
        //TODO DEADLINE 1A : Create your own tests for Table
        /*
        [Fact]
        public void Test1()
        {

        }
        */

        [Fact]
        public void TestInitialization()
        {
            ColumnDefinition columnaPrueba = new ColumnDefinition(ColumnDefinition.DataType.String,"Rickinillo");
            
            Assert.NotNull(columnaPrueba);
            Assert.Equal("Rickinillo", columnaPrueba.Name);
            Assert.Equal(ColumnDefinition.DataType.String, columnaPrueba.Type);
        }
    
    }
}