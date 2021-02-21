using LeaLang.Eval;
using System;
using Xunit;

namespace LeaLang.Tests
{
   public class TestsTest
    {
        Parser.Parser parser;
        Evaluator evaluator;


        public TestsTest()
        {
            parser = new Parser.Parser();
            evaluator = new Evaluator();
        }
        
      

        [Fact]
        public void TestBinaryExpression()
        {
            //Test
            var testSrc = "1+1";
            var compilation = parser.Parse(testSrc);
            var result = evaluator.Evaluate(compilation);
            Assert.Equal(2,result);
           
        }
       
    }
}
