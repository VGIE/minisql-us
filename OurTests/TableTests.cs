using DbManager;
using DbManager.Parser;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
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

        [Fact]
        public void testIssue42()
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
                "Luis","Rodriguez", "22"
            };
            List<String> values2 = new List<string>()
            {
                "Mikel", "Ortiz", "35"
            };

            Row r = new Row(columns, values);
            Row r2 = new Row(columns, values2);
            table.AddRow(r);
            table.AddRow(r2);


            Condition condicionTest = new Condition("Name", "=", "Luis");
            Condition condicionTest2 = new Condition("Surname", "=", "Rodriguez");
            Table table2 = (new Table("test", columns));
            Row rs = new Row(columns, values);
            table2.AddRow(rs);
            List<string> nombresc = new List<string> { c.Name, c2.Name, c3.Name };

            Assert.Equal(table2.ToString(), table.Select(nombresc, condicionTest).ToString());
            Assert.Equal(table2.ToString(), table.Select(nombresc, condicionTest2).ToString());

            Condition condicionTest3 = new Condition("Age", "<", "30");
            Table table3 = (new Table("test", columns));
            table3.AddRow(rs);
            Assert.Equal(table3.ToString(), table.Select(nombresc, condicionTest3).ToString());

            Condition condicionTest4 = new Condition("Age", ">", "30");
            Table table4 = (new Table("test", columns));
            Row rs3 = new Row(columns, values2);
            table4.AddRow(rs3);
            Assert.Equal(table4.ToString(), table.Select(nombresc, condicionTest4).ToString());

            Assert.Null(table.Select(null, condicionTest4));

            Assert.Equal(table.ToString(), table.Select(nombresc, null).ToString());


            List<ColumnDefinition> columns2 = new List<ColumnDefinition>();
            ColumnDefinition c4 = new ColumnDefinition(ColumnDefinition.DataType.String, "Name");
            ColumnDefinition c5 = new ColumnDefinition(ColumnDefinition.DataType.String, "Surname");
            ColumnDefinition c6 = new ColumnDefinition(ColumnDefinition.DataType.Int, "Age");
            columns2.Add(c4);
            columns2.Add(c5);
            columns2.Add(c6);

            Table table5 = (new Table("test", columns2));
           

            table5.Insert(values);
            table5.Insert(values2);

            Assert.Equal(table.ToString(), table5.Select(nombresc, null).ToString());

            Table table6 = (new Table("test", columns2));
            List<String> values3 = new List<string>()
            {
                "Luis","Ramirez", "22"
            };
            List<String> values4 = new List<string>()
            {
                "Carlos", "Ortiz", "35"
            };
            table6.Insert(values3);
            table6.Insert(values4);

            Table table7 = (new Table("test", columns2));
            table7.Insert(values);
            table7.Insert(values2);

            List<SetValue> actualizar = new List<SetValue> { new SetValue("Surname", "Ramirez")};
            List<SetValue> actualizar2 = new List<SetValue> { new SetValue("Name", "Carlos")};
            table7.Update(actualizar , condicionTest3);
            table7.Update(actualizar2, condicionTest4);

            Assert.Equal(table6.ToString(), table7.Select(nombresc, null).ToString());

        }

        [Fact]
        public void testIssue43()
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
                "Luis","Rodriguez", "22"
            };
            List<String> values2 = new List<string>()
            {
                "Mikel", "Ortiz", "35"
            };

            Row r = new Row(columns, values);
            Row r2 = new Row(columns, values2);
            table.AddRow(r);
            table.AddRow(r2);


            List<ColumnDefinition> columns2 = new List<ColumnDefinition>();
            columns2.Add(c3);
            columns2.Add(c);
            columns2.Add(c2);
            Condition condicionTest = new Condition("Name", "=", "Luis");
            Condition condicionTest2 = new Condition("Surname", "=", "Rodriguez");
            Table table2 = (new Table("test", columns2));
            List<String> valuesDes = new List<string>()
            {
                "22","Luis","Rodriguez" 
            };
            Row rs = new Row(columns2, valuesDes);
            table2.AddRow(rs);
            List<string> nombresc = new List<string> { c3.Name, c.Name, c2.Name };

            Assert.Equal(table2.ToString(), table.Select(nombresc, condicionTest).ToString());
            Assert.Equal(table2.ToString(), table.Select(nombresc, condicionTest2).ToString());



            List<ColumnDefinition> columns3 = new List<ColumnDefinition>();
            columns3.Add(c2);
            columns3.Add(c);
            Table table3 = (new Table("test", columns3));
            List<String> valuesDes2 = new List<string>()
            {
                "Rodriguez","Luis"
            };
            List<String> valuesDes3 = new List<string>()
            {
                "Ortiz","Mikel"
            };
            Row rs2 = new Row(columns3, valuesDes2);
            Row rs3 = new Row(columns3, valuesDes3);
            table3.AddRow(rs2);
            table3.AddRow(rs3);

            List<string> nombresc2 = new List<string> { c2.Name, c.Name };
            Assert.Equal(table3.ToString(), table.Select(nombresc2, null).ToString());

            //TESTEAR QUE CUANDO TE PASAN UNA COLUMNA QUE NO EXISTE, DEVUELVA UNA TABLA VACIA

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