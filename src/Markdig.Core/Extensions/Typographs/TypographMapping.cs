// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Markdig.Helpers;

namespace Markdig.Extensions.Typograph
{
    /// <summary>
    /// An Typograph shortcodes and smileys mapping, to be used by <see cref="TypographParser"/>.
    /// </summary>
    public class TypographMapping
    {
        /// <summary>
        /// The default Typograph shortcodes mapping.
        /// </summary>
        public static readonly TypographMapping DefaultTypographsMapping = new TypographMapping();

        #region Typographs and Smileys


        static string getText(StringSlice ss) => ss.Text.Substring(ss.Start, ss.Length);

        static Match match(string text, string pattern) => Regex.Match(text, pattern, RegexOptions.IgnoreCase);

        static TypographInline matchResult(Match m, Func<Match, string> converter)
        {
            if (!m.Success)
                return null;

            var final = converter(m);

            return new TypographInline(final)
            {
                Match = m.Value
            };
        }


        /// <summary>
        /// Returns a new instance of the default Typograph shortcode to Typograph unicode dictionary.
        /// It can be used to create a customized <see cref="TypographMapping"/>.
        /// </summary>
        public static Dictionary<char, Func<StringSlice, TypographInline>[]> GetDefaultTypoToString()
        => new Dictionary<char, Func<StringSlice, TypographInline>[]>
            {
                {'(', new Func<StringSlice, TypographInline>[]{
                    ss => matchResult(
                        match(getText(ss), @"^\(([cpr]|tm)\)"),
                        m => m.Groups[1].Value.ToLowerInvariant() switch
                        {
                            "c" =>"©",
                            "r"=>"®",
                            "p"=>"§",
                            "tm"=>"™",
                            _ => ""
                        })
                    }
                },
                {'+', new Func<StringSlice, TypographInline>[]{
                    ss => matchResult(match(getText(ss), @"^\+-"), m => "±") }
                },
                {'.', new Func<StringSlice, TypographInline>[]{
                    ss => matchResult(match(getText(ss), @"^\.{2,}"), m => "…") }
                },
                {',', new Func<StringSlice, TypographInline>[]{
                    ss => matchResult(match(getText(ss), @"^\,{2,}"), m => ",") }
                },
                {'?', new Func<StringSlice, TypographInline>[]{
                    ss =>
                    {
                        var text = getText(ss);
                        var m = match(text, @"^\?…");

                        if(!m.Success)
                            return matchResult(match(text, @"^\?{4,}"), m => "???");

                        return matchResult(m, m => "?..");
                    } }
                },
                {'!', new Func<StringSlice, TypographInline>[]{ss =>
                    {
                        var text = getText(ss);
                        var m = match(text, @"^\!…");

                        if(!m.Success)
                            return matchResult(match(text, @"^\!{4,}"), m => "!!!");

                        return matchResult(m, m => "!..");
                    } }
                },
                {'-', new Func<StringSlice, TypographInline>[]{
                    ss => matchResult(match(getText(ss), @"^-{2,3}"), m => m.Value.Length == 2 ? "&ndash;" : "&mdash;") }
                },
            };

        #endregion

        /// <summary>
        /// Constructs a mapping for the default Typograph shortcodes and smileys.
        /// </summary>
        public TypographMapping()
            : this(null)
        { }

        internal Dictionary<char, Func<StringSlice, TypographInline>[]> Mappings { get; }


        internal char[] OpeningCharacters { get; }

        /// <summary>
        /// Constructs a mapping from a dictionary of Typograph shortcodes to unicode, and a dictionary of smileys to Typograph shortcodes.
        /// </summary>
        public TypographMapping(IDictionary<string, string> typoToStrings)
        {
            Mappings = GetDefaultTypoToString();

            if (typoToStrings != null)
                foreach (var p in typoToStrings)
                {
                    var k = p.Key[0];
                    var f = new Func<StringSlice, TypographInline>(ss =>
                    {
                        var text = getText(ss);

                        if (!text.StartsWith(p.Key, StringComparison.InvariantCultureIgnoreCase))
                            return null;

                        return new TypographInline
                        {
                            Match = text.Substring(0, p.Key.Length)
                        };
                    });

                    if (Mappings.TryGetValue(k, out var map))
                        Mappings[k] = map.Append(f).ToArray();
                    else
                        Mappings[k] = new[] { f };
                }

            OpeningCharacters = Mappings.Keys.ToArray();
        }
    }
}