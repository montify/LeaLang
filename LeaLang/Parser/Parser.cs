using System;
using System.Collections.Generic;
using LeaLang.Lexer;

namespace LeaLang.Parser
{
    public enum LeaType
    {
        Int32,
        String,
        Bool,
        Nil
    }
    public class Parser
    {
        private Lexer.Lexer m_Lexer;
        private int m_Position;
        private List<Token> m_tokenList;
        public List<string> m_Diagnostics;
        
        public Parser()
        {
            m_Lexer = new Lexer.Lexer();
            m_tokenList = new List<Token>();
            m_Diagnostics = new List<string>();
        }

        private Token Consume()
        {
            if (m_Position >= m_tokenList.Count)
                return m_tokenList[m_Position - 1];

            return m_tokenList[m_Position++];
        }
        
        private Token Peek(int offset = 0)
        {
            if ((m_Position + offset) >= m_tokenList.Count)
                return m_tokenList[m_Position - 1];

            return m_tokenList[m_Position + offset];
        }

        private Token Match(Token current, TokenType against)
        {
            if (current.TokenType == against)
                return Consume();

            m_Diagnostics.Add($"Expect Token: {against}, found: {Peek().TokenType}");
            return new Token("SynthesizedToken", against, null);
            
        }
      
        private Token Match(Token current, TokenType[] against)
        {
            foreach (var a in against)
            {
                if (current.TokenType == a)
                    return Consume();
            }

            foreach (var a in against)
            {
                m_Diagnostics.Add($"Expect Token: {a}, found: {Peek().TokenType}");
            }
            
            return new Token("SynthesizedToken", against[0], null); //Just return a random possible Token
        }
        
        private Token PeekMatch(Token current, TokenType[] against)
        {
            foreach (var a in against)
            {
                if (current.TokenType == a)
                    return current;
            }

            foreach (var a in against)
            {
                m_Diagnostics.Add($"Expect Token: {a}, found: {Peek().TokenType}");
            }
            
            return new Token("SynthesizedToken", against[0], null); //Just return a random possible Token
        }
        
        private void ResetParserState()
        {
            m_Position = 0;
            m_tokenList.Clear();
            m_Diagnostics.Clear();
        }

        public Expressions Parse(string sourceText) //API entry point
        {  
            ResetParserState();
            sourceText += '\0';  //TOOD: figure out how newline is interpreted in a c# string
            m_Lexer.Lex(sourceText, Lexer.Lexer.IncludeWhiteSpaceTokens.No, Lexer.Lexer.IncludeEofToken.Yes);
            m_tokenList = m_Lexer.GetTokens();

            if (m_Lexer.m_Diagnostics.Count > 0)
            {
                foreach (var errorMessage in m_Lexer.m_Diagnostics)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Lexer: "  + errorMessage);
                    Console.ResetColor();
                }
                
                return null;
            }
            
            switch (Peek().TokenType)
            {
                //ParseStatement,....
                default:
                    return ParseExpression();
            }
        }

        private Expressions ParseExpression()
        { 
            var currentToken = Consume();
            return ParseTerm(currentToken);
        }

        private Expressions ParseTerm(Token currentToken)
        {
            var left = ParseFactor(currentToken);
            
            while (Peek().TokenType == TokenType.Plus ||
                   Peek().TokenType == TokenType.Minus)
            {
          
                var op = Match(Peek(),new []{ TokenType.Plus, TokenType.Minus});
                var n = Match(Peek(),new []{ TokenType.Number, TokenType.String, TokenType.OpenPharentices}); 
                var right = ParseFactor(n);

                left = new BinaryExpression(left, op, right);
            }
            return left;
        }

        private Expressions ParseFactor(Token currentToken)
        {
            var left = ParseUnary(currentToken);

            while (Peek().TokenType == TokenType.Star ||
                   Peek().TokenType == TokenType.ForwardSlash)
            {
                //var op = Consume();
                var op = Match(Peek(),new []{ TokenType.Star, TokenType.ForwardSlash});
                var right = ParseUnary(Consume());

                left = new BinaryExpression(left, op, right);
            }
            return left;
        }

        private Expressions ParseUnary(Token currentToken)
        {
            if (currentToken.TokenType == TokenType.Plus ||
                currentToken.TokenType == TokenType.Minus)
            {
                var op = PeekMatch(currentToken, new [] {TokenType.Plus, TokenType.Minus});
               
                var right = ParseUnary(Consume());

                return new UnaryExpression(op, right);
            }
            
            return ParsePrimary(currentToken);
        }
    
        private Expressions ParsePrimary(Token currentToken)
        {
            if (currentToken.TokenType == TokenType.OpenPharentices)
            {
                var openP = PeekMatch(currentToken, new [] {TokenType.OpenPharentices});
                var exp = ParseExpression();
                var closeP = Match(Peek(), TokenType.ClosePharentices);
                return new PharinticesExpression(openP, exp, closeP);
            }
            
            if (currentToken.Value == null) //Hm Nullcheck maybe in Eveluator. or where Typcheck occurs?
            {
               // m_Diagnostics.Add($"Value cant be null: {Peek().TokenType}");
                //return new SynthesizedExpression();
                return new LiteralExpression(currentToken.Value, LeaType.Nil);
            }

            var type = LeaType.Nil;
            
            switch (currentToken.TokenType)
            {
                case TokenType.Number:
                    type = LeaType.Int32;
                    break;
                case TokenType.String:
                    type = LeaType.String;
                    break;
                case TokenType.True:
                case TokenType.False:
                    type = LeaType.Bool;
                    break;
            }
            
            return new LiteralExpression( currentToken.Value, type);
        }
    }
}