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
            Assert.Null(fila.GetValue("NoSe"));
            Assert.Null(fila.GetValue(null));
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
            fila.SetValue("NoSe", "NiIdea");
            fila.SetValue(null, "NiIdea");
            
            Assert.NotNull(fila);
            Assert.NotNull(fila.Values);
            Assert.Equal("Morty Malvado", fila.GetValue("Rickinillo")); 
            Assert.NotEqual("Presidente", fila.GetValue("Beth"));
            Assert.Equal("Meesecks", fila.GetValue("Summer"));
          
        }

        [Fact]
        public void TestIsTrue()
        {
          List<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();
        List<string> values = new List<string>();
        Condition condicion1 = new Condition("Rickinillo",">","Jerry");
        Condition condicion2 = new Condition("Beth","<","Jerry");
        Condition condicion3 = new Condition("Summer","=","PersonaPajaro");
        Condition condicionNada = new Condition("Inexistente","=","Cualquiera");

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

            Assert.True(fila.IsTrue(condicion1));
            Assert.False(fila.IsTrue(condicion2));
            Assert.True(fila.IsTrue(condicion3));
            Assert.False(fila.IsTrue(condicionNada));
            
        }

        [Fact]
        public void TestConstructor()
        {
            List<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();
            List<string> values = new List<string>();

            ColumnDefinition columnaPrueba1 = new ColumnDefinition(ColumnDefinition.DataType.String, "Rickinillo");
            columnDefinitions.Add(columnaPrueba1);
            values.Add("Morty");
            values.Add("Extra");

            Row fila = new Row(columnDefinitions, values);

            Assert.NotNull(fila);
            Assert.NotNull(fila.Values);
            Assert.Equal(2, fila.Values.Count);

            Row filaNula = new Row(null, null);
            Assert.NotNull(filaNula);
            Assert.NotNull(filaNula.Values);
            Assert.Empty(filaNula.Values);
        }

        [Fact]
        public void Test10Issue()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>();
            ColumnDefinition c = new ColumnDefinition(ColumnDefinition.DataType.String, "Marca");
            ColumnDefinition c2 = new ColumnDefinition(ColumnDefinition.DataType.String, "Modelo");
            columns.Add(c);
            columns.Add(c2);


            List<String> values = new List<string>()
            {
                "Nike","Air Max 1: Gucci"
            };
            List<String> values2 = new List<string>()
            {
                null, "Super Star: BAPE"
            };

            Row r = new Row(columns, values);
            Row r2 = new Row(columns, values2);

            String s = "Nike:Air Max 1[SEPARATOR] Gucci";
            Assert.Equal(s, r.AsText());

            String s2 = ":Super Star[SEPARATOR] BAPE";
            Assert.Equal(s2, r2.AsText());

            String p= r.AsText();
            Assert.Equal(p, Row.Parse(columns, p).AsText());

        }
    }
}