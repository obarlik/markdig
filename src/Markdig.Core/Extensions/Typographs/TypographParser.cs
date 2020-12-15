// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Markdig.Helpers;
using Markdig.Parsers;

namespace Markdig.Extensions.Typograph
{
    /// <summary>
    /// The inline parser used for typographs.
    /// </summary>
    /// <seealso cref="InlineParser" />
    public class TypographParser : InlineParser
    {
        private readonly TypographMapping _typographMapping;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypographParser"/> class.
        /// </summary>
        public TypographParser(TypographMapping typographMapping)
        {
            _typographMapping = typographMapping;
            OpeningCharacters = _typographMapping.OpeningCharacters;
        }

        public override bool Match(InlineProcessor processor, ref StringSlice slice)
        {
            foreach (var p in _typographMapping.Mappings)
            {
                foreach (var f in p.Value)
                {
                    var m = f(slice);

                    if (m == null)
                        continue;

                    m.Span.Start = processor.GetSourcePosition(slice.Start, out int line, out int column);
                    m.Line = line;
                    m.Column = column;
                    m.Span.End = m.Span.Start + m.Match.Length - 1;

                    slice.Start += m.Match.Length;
                    return true;
                }
            }

            return false;
        }
    }
}