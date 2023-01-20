namespace Nyx.Analysis;

public enum SyntaxKind
{
    Token_Error,
    Token_Discard,
    Token_End,
    Token_InvalidChar,
    Token_Space,
    Token_CommentMarker,
    Token_Comment,

    Token_Indent,
    Token_BeginBlock,
    Token_EndBlock,
    Token_NewLine,
    Token_Number,
    Token_StringMarker,
    Token_String,
    Token_Identifier,

    Token_LParen,
    Token_RParen,
    Token_LSquare,
    Token_RSquare,

    Token_Dot,
    Token_Comma,
    Token_Colon,
    Token_Semicolon,
    Token_RArrow,

    Token_Plus,
    Token_Minus,
    Token_Star,
    Token_Slash,

    Token_Less,
    Token_Greater,
    Token_Equal,
    Token_Percent,

    // Token_LBrace,
    // Token_RBrace,
    Token_EqualEqual,
    Token_NotEqual,
    Token_LessEqual,
    Token_GreaterEqual,

    Token_PlusPlus,
    Token_PlusEqual,
    Token_MinusMinus,
    Token_MinusEqual,
    Token_StarEqual,
    Token_SlashEqual,
    Token_PercentEqual,

    Keyword_Static,
    Keyword_Mutable,
    Keyword_Var,
    Keyword_Function,
    Keyword_Return,
    Keyword_If,
    Keyword_Else,
    Keyword_Not,
    Keyword_And,
    Keyword_Or,
    // Keyword_Switch,
    // Keyword_Case,
    // Keyword_Default,
    // Keyword_For,
    // Keyword_In,
    // Keyword_Do,
    Keyword_While,
    Keyword_Continue,
    Keyword_Break,
}
