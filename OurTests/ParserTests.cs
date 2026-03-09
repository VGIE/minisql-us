using DbManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurTests
{
    public class ParserTest
    {
        [Fact]
        public void InsertTest()
        {
            Object obj;

            obj = MiniSQLParser.Parse("INSERT INTO esnulo VALUES ('', '0');");
            Assert.True(obj is Insert);
            obj = MiniSQLParser.Parse("INSERT tabla VALUES (1, 2);");
            Assert.False(obj is Insert);
            obj = MiniSQLParser.Parse("INSERT INTO heyy    VALUES    ('hola','2','3')  ");
            Assert.True(obj is Insert);
            obj = MiniSQLParser.Parse("INSERT INTO table VALUES ('2', '19,)");
            Assert.False(obj is Insert);

            obj = MiniSQLParser.Parse("INSERT INTO Usuario VALUES ('Maik', 'Tower', '14', '-1.3')");
            Assert.True(obj is Insert);
            Insert insert = (Insert)obj;

            Assert.Equal("Usuario", insert.Table);
            List<String> values = insert.Values;
            Assert.Equal("Maik", values[0]);
            Assert.Equal("Tower", values[1]);
            Assert.Equal("14", values[2]);
            Assert.Equal("-1.3", values[3]);



        }


    }

}
