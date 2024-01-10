using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ScopeFunction.Utils;

public static class StringHelpers
{
    public static string RemoveSpecialCharacters(this string str, Action<IStringRemovalOptions>? options = null)
    {
        var ops = new StringRemovalOptions();
        options?.Invoke(ops);

        var normalizedString = str.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(capacity: normalizedString.Length);

        if (ops.CharactersMappings.Count > 0)
        {
            normalizedString = ops.CharactersMappings
                .Aggregate(normalizedString, (current, kv) =>
                {
                    var s = current.Replace(kv.Key, kv.Value);
                    return s;
                });
        }
        
        if (ops.ReplaceDiacritics)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            normalizedString = Encoding.UTF8.GetString(Encoding.GetEncoding(1251).GetBytes(str));
        }

        if (ops.RemovedStrings.Count > 0)
        {
            ops.RemovedStrings.ForEach(f => normalizedString = normalizedString.Replace(f, ""));
        }

        bool IsNumeric(char c) => c is >= '0' and <= '9';
        bool IsUppercaseAlphabetic(char c) => c is >= 'a' and <= 'z';
        bool IsLowercaseAlphabetic(char c) => c is >= 'A' and <= 'Z';
        bool IsSupportedSpecialCharacter(char c) => c is '.' or '_' or ':' or '?';
        bool IsRemovedCharacter(char c) => ops.RemovedCharacters.Contains(c);

        foreach (var c in normalizedString.Where(c => IsNumeric(c) && !ops.RemoveNumbers
                                                      || IsUppercaseAlphabetic(c)
                                                      || IsLowercaseAlphabetic(c)
                                                      || IsSupportedSpecialCharacter(c)))
        {
            if (IsRemovedCharacter(c))
            {
                continue;
            }

            sb.Append(c);
        }

        return sb.ToString();
    }
}

public interface IStringRemovalOptions
{
    void WithoutDiacritics();
    void WithoutNumbers();
    void WithoutCharacter(char ch);
    void WithoutString(string str);
    void WithCharacterMapping(char from, char to);
}

public class StringRemovalOptions : IStringRemovalOptions
{
    public Dictionary<char, char> CharactersMappings { get; set; } = new();
    public List<char> RemovedCharacters { get; set; } = new();
    public List<string> RemovedStrings { get; set; } = new();
    
    public bool RemoveNumbers { get; set; }
    public bool ReplaceDiacritics { get; set; }
    public bool UseUnicode { get; set; }
    
    public void WithoutDiacritics()
    {
        ReplaceDiacritics = true;
    }

    public void WithoutNumbers()
    {
        RemoveNumbers = true;
    }

    public void WithoutCharacter(char ch)
    {
        RemovedCharacters.Add(ch);
    }

    public void WithoutString(string str)
    {
        RemovedStrings.Add(str);
    }

    public void WithCharacterMapping(char from, char to)
    {
        CharactersMappings.TryAdd(from, to);
    }
}