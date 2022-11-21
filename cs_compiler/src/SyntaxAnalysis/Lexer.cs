using Diagnostics;
using Utils;

namespace SyntaxAnalysis;

class Lexer : AAnalyzer<char, SyntaxNode>
{
    public DiagnosticCollection diagnostics;
    SyntaxDefinition _syntax;
    string _text;
    char _currentChar { get => _Peek(0); }
    char _nextChar { get => _Peek(1); }
    int _start;
    int _length { get => _pos - _start; }
    TextLocation _location { get => new TextLocation(_start, _length); }
    bool _newLine;


    public Lexer(SyntaxDefinition syntax, string text) : base(text.ToList(), syntax.endSymbol)
    {
        diagnostics = new DiagnosticCollection();
        _syntax = syntax;
        _text = text;
        _newLine = true;
    }

    public override SyntaxNode GetNext()
    {
        _start = _pos;

        // lex indents after a new line
        if (_newLine)
        {
            _SkipBlankLines();

            while (_length < _syntax.indentSize && char.Equals(_currentChar, _syntax.indentSymbol))_pos++;

            if (_length % _syntax.indentSize != 0)
                return new SyntaxNode(SyntaxKind.Token_Indent, _location, false);
            else if (_length > 0)
                return new SyntaxNode(SyntaxKind.Token_Indent, _location);

            _newLine = false;
        }

        // lex numbers
        if (char.IsDigit(_currentChar))
        {
            while (char.IsDigit(_currentChar)) _pos++;
            // TODO: Handle '_' and '.'
            return new SyntaxNode(SyntaxKind.Token_Number, _location);
        }
        // lex strings
        else if (_syntax.GetSingleTokenKind(_currentChar) == SyntaxKind._StringMarker)
        {
            var terminator = _currentChar;

            while (char.Equals(_currentChar, '\\') || !char.Equals(_nextChar, terminator))
            {
                _pos++;

                if (_syntax.IsLineTerminator(_currentChar))
                {
                    diagnostics.Add(new Error_StringNotClosed(_location, terminator));
                    return new SyntaxNode(SyntaxKind.Token_String, _location, false);
                }
            }
            _pos += 2;

            return new SyntaxNode(SyntaxKind.Token_String, _location);
        }
        // lex operators and names
        else
        {
            var kind = _syntax.GetDoubleTokenKind((_currentChar, _nextChar));

            if (kind == SyntaxKind._Error)
            {
                kind = _syntax.GetSingleTokenKind(_currentChar);

                if (kind == SyntaxKind._Error)
                {
                    // TODO: filter unused names ('_')
                    while (char.IsLetterOrDigit(_currentChar) || char.Equals(_currentChar, '_')) _pos++;

                    if (_length > 0)
                    {
                        kind = _syntax.GetKeyword(_text.Substring(_start, _length));

                        if (kind != SyntaxKind._Error)
                            return new SyntaxNode(kind, _location);

                        return new SyntaxNode(SyntaxKind.Token_Identifier, _location);
                    }

                    _pos += 1;
                    return new SyntaxNode(SyntaxKind.Token_InvalidChar, _location);
                }
                else if (kind == SyntaxKind.Token_NewLine)
                    _newLine = true;
            }
            else
                _pos++;
            _pos++;

            return new SyntaxNode(kind, _location);
        }
    }

    public override IEnumerable<SyntaxNode> GetAll()
    {
        SyntaxNode token;

        do
        {
            token = GetNext();

            if (token.kind != SyntaxKind._Discard)
                yield return token;
        }
        while (token.kind != SyntaxKind.Token_End);
    }

    private void _SkipBlankLines()
    {
        int offset = 0;
        int blankLineCount = 0;

        while (true)
        {
            int curOffset = 0;

            while (char.IsWhiteSpace(_Peek(offset + curOffset))) curOffset++;

            if (!char.Equals(_Peek(offset + curOffset), _syntax.newLineSymbol))
                break;
            // TODO: checks special case with endSymbol

            offset += curOffset;
            blankLineCount++;
        }

        _pos += offset;
    }
}