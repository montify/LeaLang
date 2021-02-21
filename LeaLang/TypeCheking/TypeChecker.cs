using System.Linq.Expressions;
using LeaLang.Parser;

namespace LeaLang.TypeCheking
{
    public class TypeChecker
    {
        public TypeChecker()
        {
            
        }

        public TypeNode GenerateTree(Expression exp)
        {
            
            return new LiteralTypeNode(LeaType.Bool);
        }
    }
}