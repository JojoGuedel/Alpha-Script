using System.Diagnostics;
using Nyx.Utils;

namespace Nyx.Analysis;

public class MemberAccess : Expression
{
    public Expression expression { get; }
    public Identifier identifier { get; }
    
    public MemberAccess(Expression expression, LexerNode dot, LexerNode name) : 
        base(TextLocation.Embrace(expression.location, name.location))
    {
        Debug.Assert(name.value != null);

        this.expression = expression;
        this.identifier = new Identifier(name);
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "MemberAccess");
        indent += _ChildIndent(isLast);
        expression.Write(writer, indent, false);
        identifier.Write(writer, indent, true);
    }
}
