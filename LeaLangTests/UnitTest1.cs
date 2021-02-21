using System;
using System.Collections.Generic;
using LeaLang.Eval;
using LeaLang.Parser;
using Xunit;

namespace LeaLangTests
{
    public class UnitTest1
    {
        private Parser parser;
        private Evaluator evaluator;
        private List<int> resultValues = new List<int>();
        private int ptr;
        public UnitTest1()
        {
             parser = new Parser();
             evaluator = new Evaluator();
             
             resultValues.Add(2);
             resultValues.Add(4);
        }
      
        
        [Theory]
        [InlineData("1+1",2)]
        [InlineData(" 2+2",4)]
        [InlineData("100 - 99",1)]
        [InlineData("-1*100-5 + 10/2 * 5", -80)]
        [InlineData(" (1 + 1) - (2*9) ", -16)]
        [InlineData("(1+1) + (1+1) *5", 12)]
        [InlineData(" 5+(2*(3-2))*4 + 5+(2*(3-2))*4 *5+(2*(3-2))*4", 66)]
        [InlineData(" 1+ 2     +   5",8)]
        [InlineData("+12",12)]
        [InlineData("-2",-2)]
        [InlineData("+2",2)]
        [InlineData("+2/2*5",5)]
        [InlineData("10/2 * 5",25)]
        public void Test1(string input, int res)
        {
            var exp = parser.Parse(input);
            var result = evaluator.EvaluateExpression(exp, parser.m_Diagnostics);
           
            Assert.Equal(res, result);
        }
        
        [Theory]
        [InlineData("A","A")]
        [InlineData("Alex","Alex")]
        [InlineData("  alex", "alex")]
        [InlineData("A","A")]
        public void Test12(string input, string res)
        {
            var exp = parser.Parse(input);
            var result = evaluator.EvaluateExpression(exp, parser.m_Diagnostics);
           
            Assert.Equal(res, result);
        }
        
        [Theory]
      
        [InlineData("False", false)]
        [InlineData("false", false)]
        [InlineData("True", true)]
        [InlineData("true", true)]
        
        public void Test13(string input, bool res)
        {
            var exp = parser.Parse(input);
            var result = evaluator.EvaluateExpression(exp, parser.m_Diagnostics);
           
            Assert.Equal(res, result);
        }
        
    }
}