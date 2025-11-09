using Ametrin.Optional.Parsing;

namespace Ametrin.Optional.Test;

internal sealed partial class SpanParsableGeneratorTests
{
    void String()
    {
        TestType.Parse("");
        TestType.Parse("", null);
        bool a = TestType.TryParse("", out _);
        bool b = TestType.TryParse("", null, out _);
        Option<TestType> c = TestType.TryParse("");
        Option<TestType> d = TestType.TryParse("", null);
    }
    void Span()
    {
        ReadOnlySpan<char> s = "";
        TestType.Parse(s);
        TestType.Parse(s, null);
        bool a = TestType.TryParse(s, out _);
        bool b = TestType.TryParse(s, null, out _);
        Option<TestType> c = TestType.TryParse(s);
        Option<TestType> d = TestType.TryParse(s, null);
    }

    [GenerateISpanParsable]
    internal partial class TestType
    {
        public static Option<TestType> TryParse(ReadOnlySpan<char> span, IFormatProvider? provider = null)
        {
            throw new NotImplementedException();
        }
    }
}
