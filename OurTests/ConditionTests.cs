using DbManager;
using Xunit;

namespace OurTests
{
    public class ConditionTests
    {
        //TODO DEADLINE 1A : Create your own tests for Condition
        
        [Fact]

        public void TestisTrue()
        {
            List<Condition> conditions = new List<Condition>()
            {
                //int

                new Condition("age", "=", "45"),
                new Condition("age", ">", "10"),
                new Condition("age", "<", "35"),
                
                //string
                new Condition("name", ">", "Tijicius"),
                new Condition("name", "=", "Nigel"),
                new Condition("name", "<", "831"),
                //double
                new Condition("sueldo", "<", "1233.21"),
                new Condition("sueldo", "=", "9393.987"),
                new Condition("sueldo", ">", "6542.134")
            };
            //constructor
            Assert.Equal("age", conditions[0].ColumnName);
            Assert.Equal("=", conditions[0].Operator);
            Assert.Equal("45", conditions[0].LiteralValue);

            //ints

            Assert.True(conditions[0].IsTrue("45", ColumnDefinition.DataType.Int));
            Assert.False(conditions[0].IsTrue("22", ColumnDefinition.DataType.Int));
            
            Assert.True(conditions[1].IsTrue("11", ColumnDefinition.DataType.Int));
            Assert.False(conditions[1].IsTrue("8", ColumnDefinition.DataType.Int));

            Assert.True(conditions[2].IsTrue("20", ColumnDefinition.DataType.Int));
            Assert.False(conditions[2].IsTrue("53", ColumnDefinition.DataType.Int));


            //strings

            Assert.True(conditions[3].IsTrue("Zalando", ColumnDefinition.DataType.String));
           

            Assert.True(conditions[4].IsTrue("Nigel", ColumnDefinition.DataType.String));
            

            Assert.True(conditions[5].IsTrue("11", ColumnDefinition.DataType.String));
            

            //doubles  

            Assert.True(conditions[6].IsTrue("22.12", ColumnDefinition.DataType.Double));
            Assert.False(conditions[6].IsTrue("902092.22", ColumnDefinition.DataType.Double));

            Assert.True(conditions[7].IsTrue("9393.987", ColumnDefinition.DataType.Double));
            Assert.False(conditions[7].IsTrue("1.09", ColumnDefinition.DataType.Double));

            Assert.True(conditions[8].IsTrue("63533772.098", ColumnDefinition.DataType.Double));
            Assert.False(conditions[8].IsTrue("2.67", ColumnDefinition.DataType.Double));

        }


    } 

}