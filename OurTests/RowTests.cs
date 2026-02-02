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
            ColumnDefinition columnaPrueba1 = new ColumnDefinition(ColumnDefinition.DataType.String,"Rickinillo");
            columnDefinitions.Add(columnaPrueba1);
            values.Add("Morty");
            ColumnDefinition columnaPrueba2 = new ColumnDefinition(ColumnDefinition.DataType.String,"Beth");
            columnDefinitions.Add(columnaPrueba2);
            values.Add("Jerry");
            ColumnDefinition columnaPrueba3 = new ColumnDefinition(ColumnDefinition.DataType.String,"Summer");
            columnDefinitions.Add(columnaPrueba3);
            values.Add("PersonaPajaro");

           
            Row fila = new Row(columnDefinitions,values);

            Assert.NotNull(fila);
            Assert.NotNull(fila.Values);
            Assert.Equal(values , fila.Values);
            Assert.Equal("Morty", fila.GetValue("Rickinillo")); 
            Assert.Equal("PersonaPajaro", fila.GetValue("Summer"));
            Assert.NotEqual("Jeremy", fila.GetValue("Beth")); 
            Assert.Null(fila.GetValue("RickBot"));
        }

        [Fact]
        public void TestSetValue(){
        List<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();
        List<string> values = new List<string>();

        ColumnDefinition columnaPrueba1 = new ColumnDefinition(ColumnDefinition.DataType.String,"Rickinillo");
            columnDefinitions.Add(columnaPrueba1);
            values.Add("Morty");
            ColumnDefinition columnaPrueba2 = new ColumnDefinition(ColumnDefinition.DataType.String,"Beth");
            columnDefinitions.Add(columnaPrueba2);
            values.Add("Jerry");
            ColumnDefinition columnaPrueba3 = new ColumnDefinition(ColumnDefinition.DataType.String,"Summer");
            columnDefinitions.Add(columnaPrueba3);
            values.Add("PersonaPajaro");

            Row fila = new Row(columnDefinitions,values);

            fila.SetValue("Rickinillo","Morty Malvado");
            fila.SetValue("Beth","Cirujana");
            fila.SetValue("Summer","Meesecks");
            
            Assert.NotNull(fila);
            Assert.NotNull(fila.Values);
            Assert.Equal("Morty Malvado", fila.GetValue("Rickinillo")); 
            Assert.NotEqual("Presidente", fila.GetValue("Beth"));
            Assert.Equal("Meesecks", fila.GetValue("Summer"));

           
          
        }
    }
}