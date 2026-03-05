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

            ColumnDefinition columna4 = new ColumnDefinition(ColumnDefinition.DataType.String, null);
            Assert.Equal("->String", columna4.AsText());
        }

        [Fact]
        public void TestParse()
        {
            ColumnDefinition columna1 = ColumnDefinition.Parse("Edad->Int");
            Assert.NotNull(columna1);
            Assert.Equal("Edad", columna1.Name);
            Assert.Equal(ColumnDefinition.DataType.Int, columna1.Type);

            ColumnDefinition columna2 = ColumnDefinition.Parse("Nombre[ARROW]Completo->String");
            Assert.NotNull(columna2);
            Assert.Equal("Nombre->Completo", columna2.Name);
            Assert.Equal(ColumnDefinition.DataType.String, columna2.Type);

            ColumnDefinition columna3 = ColumnDefinition.Parse("->Double");
            Assert.NotNull(columna3);
            Assert.Equal("", columna3.Name);
            Assert.Equal(ColumnDefinition.DataType.Double, columna3.Type);

            ColumnDefinition columna4 = ColumnDefinition.Parse(null);
            Assert.Null(columna4);

            ColumnDefinition columna5 = ColumnDefinition.Parse("Estatura->Lentejas");
            Assert.Null(columna5);

            ColumnDefinition columna6 = ColumnDefinition.Parse("SinDelimitador");
            Assert.Null(columna6);
        }
    
    }
}