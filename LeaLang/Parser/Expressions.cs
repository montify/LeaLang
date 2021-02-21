using System;
using LeaLang.Lexer;

namespace LeaLang.Parser
{
    public abstract class Node
    {
       
    }
    public abstract class Statement : Node
    {
        public object Value { get; set; }
    }

    public abstract class Expressions : Node
    {
        public object Value { get; set; }
    }
    public  class SynthesizedExpression : Expressions
    {
        public object Value { get; set; }
    }
    public class NumberExpression : Expressions
    {
        public NumberExpression(int value)
        {
            Value = value;
        }
    }
    public class LiteralExpression : Expressions
    {
        public LeaType Type { get; }

        public LiteralExpression(object value, LeaType type)
        {
            Type = type;
            Value = value;
        }
    }
   
    public class PharinticesExpression : Expressions
    {
        public Token OpenPharentices { get; }
        public Expressions Exp { get; }
        public Token ClosePharentices { get; }
 
        public PharinticesExpression(Token openPharentices, Expressions exp, Token closePharentices)
        {
            OpenPharentices = openPharentices;
            Exp = exp;
            ClosePharentices = closePharentices;
        }
    }
    
    public class BinaryExpression : Expressions
    {
        public Expressions Left { get; }
        public Token Op { get; }
        public Expressions Right { get; }

        public BinaryExpression(Expressions left, Token op, Expressions right)
        {
            Left = left;
            Op = op;
            Right = right;
        }
    }

    public class UnaryExpression : Expressions
    {
        public Token UnaryOperator { get; }
        public Expressions Exp { get; }

        public UnaryExpression(Token unaryOperator, Expressions exp)
        {
            UnaryOperator = unaryOperator;
            Exp = exp;
        }
    }
}
