using LeaLang.Parser;

namespace LeaLang.TypeCheking
{
    public abstract class TypeNode
    {
        public  LeaType type { get; internal set; }
    }

    public  class LiteralTypeNode : TypeNode
    {
        public LiteralTypeNode(LeaType type)
        {
            this.type = type;
        }
        
        
    }
}