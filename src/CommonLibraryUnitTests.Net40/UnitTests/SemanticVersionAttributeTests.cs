//-----------------------------------------------------------------------------
// <copyright file="SemanticVersionAttributeTests.cs" 
//            company="ImaginaryRealities">
// Copyright 2013 ImaginaryRealities, LLC
// </copyright>
// <summary>
// This file unit tests the SemanticVersionAttribute class.
// </summary>
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
