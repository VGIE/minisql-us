using DbManager;
using System.Data.Common;
using System.Xml.Linq;

namespace OurTests
{

    
    public class TableTests
    {
        [Fact]
        public void testIssue4()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>();
            ColumnDefinition c = new ColumnDefinition(ColumnDefinition.DataType.String, "Name");
            ColumnDefinition c2 = new ColumnDefinition(ColumnDefinition.DataType.String, "Surname");
            columns.Add(c);
            columns.Add(c2);
            Table table = (new Table("test", columns));
            List<String> values = new List<string>()
            {
                "Luis","Rodriguez"
            };
            List<String> values2 = new List<string>()
            {
                "Mikel", "Ortiz"
            };

            Row r = new Row(columns, values);
            Row r2 = new Row(columns, values2);
            table.AddRow(r);
            table.AddRow(r2);

            Assert.Equal(r, table.GetRow(0));
            Assert.Equal(r2, table.GetRow(1));
            Assert.Null(table.GetRow(20));
            Assert.Null(table.GetRow(-1));
            Assert.Equal(2, table.NumRows());

            Assert.Equal(c, table.GetColumn(0));
            Assert.Equal(c2, table.GetColumn(1));
            Assert.Null(table.GetColumn(5));
            Assert.Null(table.GetColumn(-1));
            Assert.Equal(2, table.NumColumns());

            Assert.Equal(c, table.ColumnByName("Name"));
            Assert.Equal(c2, table.ColumnByName("Surname"));
            Assert.Null(table.ColumnByName("Nombre"));
            Assert.Null(table.ColumnByName(null));

            Assert.Equal(0, table.ColumnIndexByName("Name"));
            Assert.Equal(1, table.ColumnIndexByName("Surname"));
            Assert.Equal(-1, table.ColumnIndexByName("Nombre"));
            Assert.Equal(-1, table.ColumnIndexByName(null));
        }

        //TODO DEADLINE 1A : Create your own tests for Table

        [Fact]
        public void toString()
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