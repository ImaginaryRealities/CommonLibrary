//-----------------------------------------------------------------------------
// <copyright file="SemanticVersionAttributeTests.cs" 
//            company="ImaginaryRealities">
// Copyright 2013 ImaginaryRealities, LLC
// </copyright>
// <summary>
// This file unit tests the SemanticVersionAttribute class.
// </summary>
// <license>
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// right to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shell be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
// </license>
//-----------------------------------------------------------------------------

namespace ImaginaryRealities.Framework.UnitTests
{
    using Xunit;

    /// <summary>
    /// Unit tests the <see cref="SemanticVersionAttribute"/> class.
    /// </summary>
    public static class SemanticVersionAttributeTests
    {
        /// <summary>
        /// Tests that the <see cref="SemanticVersionAttribute"/> object
        /// contains the correct version number.
        /// </summary>
        [Fact]
        public static void AttributeContainsCorrectVersionNumber()
        {
            var attribute = new SemanticVersionAttribute("1.2.3-alpha+build.3");
            Assert.NotNull(attribute.VersionNumber);
            var version = attribute.VersionNumber;
            Assert.Equal(1, version.MajorVersion);
            Assert.Equal(2, version.MinorVersion);
            Assert.Equal(3, version.PatchVersion);
            Assert.Equal("alpha", version.PrereleaseVersion);
            Assert.Equal("build.3", version.BuildVersion);
        }
    }
}
