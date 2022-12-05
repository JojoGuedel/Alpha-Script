using Xunit;
using Nyx.CodeAnalysis;

namespace Nyx.Tests;

public class LexicalAnalyzerTests
{
    static SyntaxDefinition _syntax = SyntaxDefinition.Default();

    [Theory]
    [MemberData(nameof(GetSingleTokenData))]
    [MemberData(nameof(GetDoubleTokenData))]
    [MemberData(nameof(GetKeywordData))]
    [MemberData(nameof(GetAdditionalTokenData))]
    void TestTokens(string input, List<SyntaxKind> expected)
    {
        var lexicalAnalyzer = new LexicalAnalyzer(_syntax, input);
        var result = lexicalAnalyzer.GetAll().ToList();

        for (int i = 0; i < expected.Count; i++)
            Assert.Equal(expected[i], result[i].kind);

        Assert.Equal(expected.Count + 1, result.Count);
        Assert.Equal(SyntaxKind.Token_End, result.Last().kind);
    }

    // ------------------------------ Basic Token Tests -----------------------------
    static object[]? FinalizeTokenData(string pattern, SyntaxKind kind)
    {
        var expected = new List<SyntaxKind>(1);

        switch (kind)
        {
            case SyntaxKind.Token_Space:
            case SyntaxKind.Token_End:
                return null;
            case SyntaxKind.Token_CommentMarker:
                pattern = "// test comment";
                kind = SyntaxKind.Token_Comment;
                break;
            case SyntaxKind.Token_StringMarker:
                pattern = "\"test string\"";
                kind = SyntaxKind.Token_String;
                break;
        }

        expected.Add(kind);
        return new object[] { pattern, expected };
    }


    static IEnumerable<object[]> GetSingleTokenData()
    {
        foreach (var pattern in _syntax.singleTokens.Keys)
        {
            var data = FinalizeTokenData(pattern.ToString(), _syntax.GetSingleTokenKind(pattern));
            
            if (data is null) continue;
            yield return data;
        }
    }

    static IEnumerable<object[]> GetDoubleTokenData()
    {
        foreach (var pattern in _syntax.doubleTokens.Keys)
        {
            var data = FinalizeTokenData("" + pattern.Item1 + pattern.Item2, _syntax.GetDoubleTokenKind(pattern));
            
            if (data is null) continue;
            yield return data;
        }
    }

    static IEnumerable<object[]> GetKeywordData()
    {
        foreach (var pattern in _syntax.keywords.Keys)
        {
            var data = FinalizeTokenData(pattern, _syntax.GetKeyword(pattern));
            
            if (data is null) continue;
            yield return data;
        }
    }

    static IEnumerable<object[]> GetAdditionalTokenData()
    {
        string[] numberData = 
        {
            "1",
            "0000000001",
            "9487463973",
            // "0.9487463973",
            // "0x5aBCdeF",
            // "0b1010010",
            // "1e99",
            // "1.582092e99",
        };

        var expected = new List<SyntaxKind>();
        expected.Add(SyntaxKind.Token_Number);
        
        foreach(var e in numberData)
            yield return new object[] { e, expected };
    }
    // ------------------------------ Basic Token Tests -----------------------------

    // ---------------------------- Combined Token Tests ----------------------------
    
    // ---------------------------- Combined Token Tests ----------------------------
}