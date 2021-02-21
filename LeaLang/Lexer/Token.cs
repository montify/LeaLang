namespace LeaLang.Lexer
{
    public class Token
    {
        public string Name { get; }
        public TokenType TokenType { get; }
        public object Value { get; }
        
        public Token(string name, TokenType tokenType, object value)
        {
            Name = name;
            TokenType = tokenType;
            Value = value;
        }
    }
}
