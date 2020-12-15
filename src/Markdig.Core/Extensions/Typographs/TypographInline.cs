// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Markdig.Helpers;
using Markdig.Syntax.Inlines;

namespace Markdig.Extensions.Typograph
{
    /// <summary>
    /// An Typograph inline.
    /// </summary>
    /// <seealso cref="Inline" />
    public class TypographInline : LiteralInline
    {
        // Inherit from LiteralInline so that rendering is already handled by default

        /// <summary>
        /// Initializes a new instance of the <see cref="TypographInline"/> class.
        /// </summary>
        public TypographInline()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypographInline"/> class.
        /// </summary>
        /// <param name="content">The content.</param>
        public TypographInline(string content)
        {
            Content = new StringSlice(content);
        }

        /// <summary>
        /// Gets or sets the original match string (either an Typograph shortcode or a text smiley)
        /// </summary>
        public string Match { get; set; }
    }
}