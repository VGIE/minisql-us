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

        [Fact]
        public void TestAsText()
        {
            ColumnDefinition columna1 = new ColumnDefinition(ColumnDefinition.DataType.Int, "Edad");
            Assert.Equal("Edad->Int", columna1.AsText());

            ColumnDefinition columna2 = new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre->Completo");
            Assert.Equal("Nombre[ARROW]Completo->String", columna2.AsText());

            ColumnDefinition columna3 = new ColumnDefinition(ColumnDefinition.DataType.Double, "");
            Assert.Equal("->Double", columna3.AsText());
        }
    
    }
}