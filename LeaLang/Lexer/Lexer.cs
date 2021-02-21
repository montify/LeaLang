using System;
using System.Collections.Generic;
using System.Text;

namespace LeaLang.Lexer
{
    public class Lexer
    {
        private string m_SourceText;
        private int m_Position;
        private List<Token> m_TokenList ;
     
        public List<string> m_Diagnostics = new List<string>();
        public Lexer()
        {
            m_TokenList =  new List<Token>();
        }

        private char Consume()
        {
            if (m_Position >= m_SourceText.Length)
                return m_SourceText[m_Position - 1];

            return m_SourceText[m_Position++]; //advance m_Position
        }

        private char Peek(int offset = 0)
        {
            if ((m_Position + offset) >= m_SourceText.Length)
                return m_SourceText[m_Position - 1];

            return m_SourceText[m_Position + offset];
        }

        private bool IsEof()
        {
            return Peek() == '\0';
        }

        private void ResetLexerState()
        {
            m_Position = 0;
            m_SourceText = "";
            m_TokenList.Clear();
            m_Diagnostics.Clear();
        }

        public enum IncludeWhiteSpaceTokens { Yes, No }
        public enum IncludeEofToken { Yes, No }

        // Api entry point
        public void Lex(string sourceText, IncludeWhiteSpaceTokens includeWhiteSpaceTokens,
            IncludeEofToken includeEofToken)
        {
            ResetLexerState(); //Fresh Start
            m_SourceText = sourceText;

            while (true)
            {
                var currentChar = Consume();

                switch (currentChar)
                {
                    case '+':
                    { 
                        if (Peek() == '+')
                        {
                            var _ = Consume();
                            m_TokenList.Add(new Token("PlusPlus", TokenType.PlusPlus, null));
                            break;
                        }
                        m_TokenList.Add(new Token("Plus", TokenType.Plus, null));
                        break;
                    }
                    case '-':
                    {
                        if (Peek() == '-')
                        {
                            var _ = Consume();
                            m_TokenList.Add(new Token("MinusMinus", TokenType.MinusMinus, null));
                            break;
                        }
                        m_TokenList.Add(new Token("Minus", TokenType.Minus, null));
                        break;
                    }
                    case '*':
                    {
                        m_TokenList.Add(new Token("Minus", TokenType.Star, null));
                        break;
                    }
                    case '/':
                    {
                        m_TokenList.Add(new Token("Minus", TokenType.ForwardSlash, null));
                        break;
                    }
                    case '!':
                    {
                        m_TokenList.Add(new Token("Bang", TokenType.Bang, null));
                        break;
                    }
                    case '(':
                    {
                        m_TokenList.Add(new Token("OpenPharentices", TokenType.OpenPharentices, null));
                        break;
                    }
                    case ')':
                    {
                        m_TokenList.Add(new Token("ClosePharentices", TokenType.ClosePharentices, null));
                        break;
                    }
                    case ' ':
                    {
                        if (includeWhiteSpaceTokens == IncludeWhiteSpaceTokens.Yes)
                            m_TokenList.Add(new Token("WhiteSpace", TokenType.WhiteSpace, null));
                        break;
                    }
                    case char _ when char.IsDigit(currentChar):
                    {
                        var sb = new StringBuilder();

                        sb.Append(currentChar);

                        while (char.IsDigit(Peek()))
                        {
                            sb.Append(Consume());
                        }

                        m_TokenList.Add(new Token("Number", TokenType.Number, sb.ToString()));
                        break;
                    }
                    case char _ when char.IsLetter(currentChar):
                    {
                        var sb = new StringBuilder();

                        sb.Append(currentChar);

                        while (char.IsLetter(Peek()))
                        {
                            sb.Append(Consume());
                        }

                        var parsedString = sb.ToString();

                        if (parsedString == "true"|| parsedString == "True")
                        {
                           
                            m_TokenList.Add(new Token("true", TokenType.True, sb.ToString()));
                            break;
                        }  
                        if (parsedString == "false" || parsedString == "False")
                        {
                            m_TokenList.Add(new Token("false", TokenType.False, sb.ToString()));
                            break;
                        }
                        
                        m_TokenList.Add(new Token("Letter", TokenType.String, sb.ToString()));
                        break;
                    }
                    default:
                    {
                        m_TokenList.Add(new Token("Default", TokenType.Default, null));
                        m_Diagnostics.Add($"Unexpected Token: {currentChar}");
                        break;
                    }
                }

                if (IsEof())
                {
                    if (includeEofToken == IncludeEofToken.Yes)
                        m_TokenList.Add(new Token("EOF", TokenType.EOF, null));

                    break;
                }
            }
        }

    
        public List<Token> GetTokens()
        {
            return m_TokenList;
        }
    }
}
