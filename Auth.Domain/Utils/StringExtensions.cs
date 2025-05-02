using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Auth.Domain.Utils;

[ExcludeFromCodeCoverage]
public static class StringExtensions
{
    public static string ApenasNumeros(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        return Regex.Replace(input, @"\D", string.Empty);
    }
}