using DbManager;

namespace OurTests
{
    public class RowTests
    {
        //TODO DEADLINE 1A : Create your own tests for Row
        /*
        [Fact]
        public void Test1()
        {

        }
        */
        [Fact]
        public void TestInitializeyGetValue()
        {
            List<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();
            List<string> values = new List<string>();
            ColumnDefinition columnaPrueba = new ColumnDefinition(ColumnDefinition.DataType.String,"Rickinillo");
            columnDefinitions.Add(columnaPrueba);
            values.Add("Morty");

           
            Row fila = new Row(columnDefinitions,values);

            Assert.NotNull(fila.Values);
            Assert.Equal(values , fila.Values);
            Assert.Equal("Morty", fila.GetValue("Rickinillo")); 
        }
    }
}