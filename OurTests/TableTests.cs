using DbManager;

namespace OurTests
{
    public class TableTests
    {
        //TODO DEADLINE 1A : Create your own tests for Table
        
        [Fact]
        public void toStringTest()
        {
            //Valid examples:
            //"['Name']{'Adolfo'}{'Jacinto'}" <- one column, two rows
            //"['Name','Age']{'Adolfo','23'}{'Jacinto','24'}" <- two columns, two rows
            //"" <- no columns, no rows
            //"['Name']" <- one column, no rows
            
            Assert.Equal("", (new Table("test", null)).ToString());

            List<ColumnDefinition> columns = new List<ColumnDefinition>();
            ColumnDefinition c = new ColumnDefinition(ColumnDefinition.DataType.String, "Name");
            columns.Add(c);
            Table table = (new Table("test", columns)), table2;
            Assert.Equal("['Name']", table.ToString());

            List<String> values = new List<String>(), values2 = new List<string>();
            values.Add("Albert");
            values2.Add("Maria");
            table.AddRow(new Row(columns, values));
            table.AddRow(new Row(columns, values2));
            Assert.Equal("['Name']{'Albert'}{'Maria'}", table.ToString());

            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.Int, "Age"));

            table2 = new Table("test2", columns);
            values.Add("45");
            values2.Add("32");
            table2.AddRow(new Row(columns, values));
            table2.AddRow(new Row(columns, values2));
            Assert.Equal("['Name','Age']{'Albert','45'}{'Maria','32'}", table2.ToString());

        }
    }
}