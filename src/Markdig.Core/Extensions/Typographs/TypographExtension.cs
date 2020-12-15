// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Markdig.Renderers;
using System.Collections.Generic;

namespace Markdig.Extensions.Typograph
{
    /// <summary>
    /// Extension to allow emoji shortcodes and smileys replacement.
    /// </summary>
    /// <seealso cref="IMarkdownExtension" />
    public class TypographExtension : IMarkdownExtension
    {
        public TypographExtension(TypographMapping extraTypoMappings = null)
        {
            ExtraTypoMappings = extraTypoMappings;
        }

        public TypographMapping ExtraTypoMappings { get; }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.InlineParsers.Contains<TypographParser>())
            {
                // Insert the parser before any other parsers
                pipeline.InlineParsers.Insert(0, new TypographParser(ExtraTypoMappings));
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
        }
    }
}