using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using LeaLang.Lexer;
using LeaLang.Parser;

namespace LeaLang.Eval
{
    public class Evaluator
    {
        public List<string> m_Diagnostics = new List<string>();
        public Evaluator()
        {

        }

        public void ResetEvaluatorState()
        {
            m_Diagnostics.Clear();
        }
        
        public object EvaluateExpression(Expressions exp, List<String> diagnostics = null)
        {
            if (exp == null) 
                return null;

            if (diagnostics?.Count > 0)
            {
                foreach (var errorMessage in diagnostics)
                {
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Parser: " + errorMessage);
                        Console.ResetColor();
                    }
                    return null;
                }
            }

            if (exp is BinaryExpression b)
            {
                var l = EvaluateExpression(b.Left);
                var r = EvaluateExpression(b.Right);
                
                if (l is int ll && r is int rr)
                {
                    if (b.Op.TokenType == TokenType.Plus)
                        return ll + rr;
                    if (b.Op.TokenType == TokenType.Minus)
                        return ll - rr;
                    if (b.Op.TokenType == TokenType.Star)
                        return ll * rr;
                    if (b.Op.TokenType == TokenType.ForwardSlash)
                        return ll / rr;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Evaluator: cant do {b.Op.Name} on {l.GetType()} <-> { r.GetType()}");
                    Console.ResetColor();
                    return null;
                }
            }
        
            if (exp is UnaryExpression u)
            {
                if (u.UnaryOperator.TokenType == TokenType.Minus)
                    return -(int)EvaluateExpression(u.Exp);
                else
                    return (int)EvaluateExpression(u.Exp);
            }

            if (exp is PharinticesExpression p)
                return EvaluateExpression(p.Exp);

            
            if (exp is LiteralExpression lit)
            {
                switch (lit.Type)
                {
                    case LeaType.Int32:
                    {
                        if(int.TryParse(lit.Value.ToString(), out var val))
                        {
                            return val;
                        }
                        else
                        {
                            m_Diagnostics.Add($"Evaluator: cant parse {lit.Value.ToString()} to LeaType.I32");
                            return 0;
                        }
                    }
                   
                    case LeaType.String:
                        return lit?.Value.ToString();
                    case LeaType.Bool:
                        return bool.Parse(lit.Value.ToString());
                    case LeaType.Nil:
                        m_Diagnostics.Add($"Value cant be null");
                        return  -404;
                    default:
                        throw new Exception($"Unknown Type: {lit.Type}");
                }
            }
            
            throw new Exception($"Cant Evaluate { exp }");
        }
    }
}
