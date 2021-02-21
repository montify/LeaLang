using LeaLang.Eval;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LeaLang
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser.Parser();
            var evaluator = new Evaluator();
            Stopwatch sw = new Stopwatch();
          
            
           while (true)
            {   
                Console.Write(">");
                var input = Console.ReadLine();
               
                if (input == "exit") break;
                sw.Start();
             
                var exp = parser.Parse(input);
                if(exp == null) continue;
                
                evaluator.ResetEvaluatorState();
                var result = evaluator.EvaluateExpression(exp, parser.m_Diagnostics);

                if (evaluator.m_Diagnostics.Count > 0)
                {
                    foreach (var msg in evaluator.m_Diagnostics)
                    {
                        Console.WriteLine($"Evaluator: {msg}");
                      
                    }
                    continue;
                }
              
                
                if(result == null)continue;
                    
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Result: " + result);
                Console.ResetColor();
                sw.Stop();
                Console.WriteLine("ms: " +   sw.Elapsed.Milliseconds);
                sw.Reset();
            }
         
         
        }
    }
}
