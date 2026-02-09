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
            ColumnDefinition c3 = new ColumnDefinition(ColumnDefinition.DataType.Int, "Age");
            columns.Add(c);
            columns.Add(c2);
            columns.Add(c3);
            Table table = (new Table("test", columns));
            List<String> values = new List<string>()
            {
                "Luis","Rodriguez", "33"
            };
            List<String> values2 = new List<string>()
            {
                "Mikel", "Ortiz", "22"
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
            Assert.Equal(c3, table.GetColumn(2));
            Assert.Null(table.GetColumn(5));
            Assert.Null(table.GetColumn(-1));
            Assert.Equal(3, table.NumColumns());

            Assert.Equal(c, table.ColumnByName("Name"));
            Assert.Equal(c2, table.ColumnByName("Surname"));
            Assert.Equal(c3, table.ColumnByName("Age"));
            Assert.Null(table.ColumnByName("Nombre"));
            Assert.Null(table.ColumnByName(null));

            Assert.Equal(0, table.ColumnIndexByName("Name"));
            Assert.Equal(1, table.ColumnIndexByName("Surname"));
            Assert.Equal(2, table.ColumnIndexByName("Age"));
            Assert.Equal(-1, table.ColumnIndexByName("Nombre"));
            Assert.Equal(-1, table.ColumnIndexByName(null));



            Condition condicionTest = new Condition("Name", "=", "Luis");
            Condition condicionTest2 = new Condition("Surname", "=", "Rodriguez");
            Table table2 = (new Table("test", columns));
            Row rs = new Row(columns, values);
            table2.AddRow(rs);
            List<string> nombresc = new List<string>();
            for(int i=0; i<columns.Count; i++)
            {
                nombresc.Add(columns[i].Name);
            }
            Assert.Equal(table2, table.Select(nombresc, condicionTest));
            Assert.Equal(table2, table.Select(nombresc, condicionTest2));

            Condition condicionTest3 = new Condition("Age", "<", "30");
            Table table3 = (new Table("test", columns));
            Row rs2 = new Row(columns, values2);
            table2.AddRow(rs2);
            Assert.Equal(table3, table.Select(nombresc, condicionTest3));

            Condition condicionTest4 = new Condition("Age", ">", "30");
            Table table4 = (new Table("test", columns));
            Row rs3 = new Row(columns, values);
            table2.AddRow(rs3);
            Assert.Equal(table3, table.Select(nombresc, condicionTest4));

            Assert.Null(table.Select(null, condicionTest4));

            Assert.Equal(table, table.Select(nombresc, null));





        }

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

        [Fact]
        public void deleteithrowTest()
        {
            List<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();
            columnDefinitions.Add(new ColumnDefinition(ColumnDefinition.DataType.String, "name"));
            columnDefinitions.Add(new ColumnDefinition(ColumnDefinition.DataType.Int, "age"));
            Table t = new Table("test", columnDefinitions);
            
            List<String> values = new List<String>();
            values.Add("Mario");
            values.Add("26");
            t.AddRow(new Row(columnDefinitions, values));

            values.Clear();
            values.Add("Carla");
            values.Add("26");
            t.AddRow(new Row(columnDefinitions, values));

            values.Clear();
            values.Add("Ramon");
            values.Add("19");
            Row toDelete = new Row(columnDefinitions, values);
            t.AddRow(toDelete);

            values.Clear();
            values.Add("Juan");
            values.Add("81");
            Row fila = new Row(columnDefinitions, values);
            t.AddRow(fila);

            Assert.Equal(toDelete, t.GetRow(2));
            t.DeleteIthRow(2);
            Assert.Equal(3, t.NumRows());
            Assert.Equal(fila, t.GetRow(2));
        }


        [Fact]
        public void DeleteWhereTest()
        {
            List<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();
            columnDefinitions.Add(new ColumnDefinition(ColumnDefinition.DataType.String, "name"));
            columnDefinitions.Add(new ColumnDefinition(ColumnDefinition.DataType.Int, "age"));
            Table t = new Table("test", columnDefinitions);

            List<String> values = new List<String>(),
                values2 = new List<String>(),
                values3 = new List<String>(),
                values4 = new List<String>();
            values.Add("Mario");
            values.Add("26");
            t.AddRow(new Row(columnDefinitions, values));

            values2.Add("Carla");
            values2.Add("19");
            t.AddRow(new Row(columnDefinitions, values2));

            values3.Add("Ramon");
            values3.Add("19");
            Row toDelete = new Row(columnDefinitions, values3);
            t.AddRow(toDelete);

            values4.Add("Juan");
            values4.Add("81");
            Row fila = new Row(columnDefinitions, values4);
            t.AddRow(fila);

            t.DeleteWhere(new Condition("age", "<", "20"));
            Assert.Equal(2, t.NumRows());
            Assert.Equal("Mario", t.GetRow(0).GetValue("name"));
            Assert.Equal("Juan", t.GetRow(1).GetValue("name"));
            t.DeleteWhere(new Condition("name", "=", "Juan"));
            Assert.Equal(1, t.NumRows());
            Assert.Equal("Mario", t.GetRow(0).GetValue("name"));

            t.DeleteWhere(new Condition("name", "=", "null"));
            Assert.Equal(1, t.NumRows());
            Assert.Equal("Mario", t.GetRow(0).GetValue("name"));
        }
    }
}