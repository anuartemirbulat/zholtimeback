namespace Common.Extensions;

public static class StringExtensions
{
    public static (int ab, int cd, int ef, int hij, int hi, int j) ExtractValuesFromCode(this string code)
    {
        if (code == "0") return (0, 0, 0, 0, 0, 0);
        int ab = int.Parse(code.Substring(0, 2));
        int cd = int.Parse(code.Substring(2, 2));
        int ef = int.Parse(code.Substring(4, 2));
        int hij = int.Parse(code.Substring(6, 3));
        int hi = int.Parse(code.Substring(6, 2));
        int j = int.Parse(code.Substring(8, 1));

        return (ab, cd, ef, hij, hi, j);
    }
}