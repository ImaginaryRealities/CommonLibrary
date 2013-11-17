//-----------------------------------------------------------------------------
// <copyright file="SemanticVersionNumberTests.cs" 
//            company="ImaginaryRealities">
// Copyright 2013 ImaginaryRealities, LLC
// </copyright>
// <summary>
// This file unit tests the SemanticVersionNumber class.
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
    using System;
    using System.Diagnostics;

    using Xunit;

    /// <summary>
    /// Unit tests the <see cref="SemanticVersionNumber"/> class.
    /// </summary>
    public static class SemanticVersionNumberTests
    {
        /// <summary>
        /// Tests that the <see cref="SemanticVersionNumber"/> constructor
        /// will parse a version number only containing the required
        /// components.
        /// </summary>
        [Fact]
        public static void ConstructorInitializesBaseVersionNumbers()
        {
            var version = new SemanticVersionNumber("1.2.3");
            Assert.Equal(1, version.MajorVersion);
            Assert.Equal(2, version.MinorVersion);
            Assert.Equal(3, version.PatchVersion);
            Assert.Null(version.PrereleaseVersion);
            Assert.Null(version.BuildVersion);
        }

        /// <summary>
        /// Tests that the <see cref="SemanticVersionNumber"/> constructor
        /// can parse a full version number.
        /// </summary>
        [Fact]
        public static void ConstructorParsesFullVersionNumber()
        {
            var version = new SemanticVersionNumber("1.2.3-alpha.1+build.123");
            Assert.Equal(1, version.MajorVersion);
            Assert.Equal(2, version.MinorVersion);
            Assert.Equal(3, version.PatchVersion);
            Assert.Equal("alpha.1", version.PrereleaseVersion);
            Assert.Equal("build.123", version.BuildVersion);
        }

        /// <summary>
        /// Tests that the <see cref="SemanticVersionNumber"/> constructor
        /// throws an exception if an invalid version number is passed as an
        /// argument.
        /// </summary>
        [Fact]
        public static void ConstructorThrowsAnExceptionForInvalidVersionNumber()
        {
            Assert.Throws<ArgumentException>(() => new SemanticVersionNumber("1.2.3.4"));
        }

        /// <summary>
        /// Test that the code contract throws an 
        /// <see cref="ArgumentException"/> if the major version number is less
        /// than zero.
        /// </summary>
        [Fact]
        public static void ContractFailsIfMajorVersionIsLessThanZero()
        {
            Assert.Throws<ArgumentException>(() => new SemanticVersionNumber(-1, 0, 0));
        }

        /// <summary>
        /// Tests that the code contract throws an
        /// <see cref="ArgumentException"/> if the minor version number is less
        /// than zero.
        /// </summary>
        [Fact]
        public static void ContractFailsIfMinorVersionIsLessThanZero()
        {
            Assert.Throws<ArgumentException>(() => new SemanticVersionNumber(0, -1, 0));
        }

        /// <summary>
        /// Tests that the code contract throws an
        /// <see cref="ArgumentException"/> if the patch version number is less
        /// than zero.
        /// </summary>
        [Fact]
        public static void ContractFailsIfPatchVersionIsLessThanZero()
        {
            Assert.Throws<ArgumentException>(() => new SemanticVersionNumber(0, 0, -1));
        }

        /// <summary>
        /// Tests that two <see cref="SemanticVersionNumber"/> objects that
        /// represent the same version number are equal.
        /// </summary>
        [Fact]
        public static void IdenticalVersionNumbersAreEqual()
        {
            var version1 = new SemanticVersionNumber(1, 0, 0);
            var version2 = new SemanticVersionNumber(1, 0, 0);
            Assert.Equal(0, version1.CompareTo(version2));
        }

        /// <summary>
        /// Tests that one <see cref="SemanticVersionNumber"/> is less than
        /// another if the major version number of the first is less than the
        /// major version number of the second.
        /// </summary>
        [Fact]
        public static void MajorVersionIsLessThanOther()
        {
            var version1 = new SemanticVersionNumber(1, 2, 3);
            var version2 = new SemanticVersionNumber(2, 0, 0);
            Assert.True(version1 < version2);
        }

        /// <summary>
        /// Tests that one <see cref="SemanticVersionNumber"/> object is
        /// greater than another if the minor version number of the first is
        /// greater than the minor version number of the second.
        /// </summary>
        [Fact]
        public static void MinorVersionIsGreaterThanOther()
        {
            var version1 = new SemanticVersionNumber(1, 2, 0);
            var version2 = new SemanticVersionNumber(1, 1, 0);
            Assert.True(version1 > version2);
        }

        /// <summary>
        /// Tests that one <see cref="SemanticVersionNumber"/> object is
        /// less than another if the patch version number of the first is
        /// less than the patch version number of the second.
        /// </summary>
        [Fact]
        public static void PatchVersionIsLessThanOther()
        {
            var version1 = new SemanticVersionNumber(1, 1, 3);
            var version2 = new SemanticVersionNumber(1, 1, 4);
            Assert.True(version1 < version2);
        }

        /// <summary>
        /// Tests that one <see cref="SemanticVersionNumber"/> object is
        /// greater than another if one does not have a prerelease version
        /// and the second one does.
        /// </summary>
        [Fact]
        public static void ReleaseVersionIsGreaterThanPrereleaseVersion()
        {
            var version1 = new SemanticVersionNumber("1.0.0-alpha");
            var version2 = new SemanticVersionNumber(1, 0, 0);
            Assert.True(version1 < version2);
            Assert.True(version2 > version1);
        }

        /// <summary>
        /// Tests that a <see cref="SemanticVersionNumber"/> object cannot be
        /// compared to a string.
        /// </summary>
        [Fact]
        public static void SemanticVersionCannotBeComparedToString()
        {
            var version = new SemanticVersionNumber(1, 0, 0);
            Assert.Throws<ArgumentException>(() => version.CompareTo("1.3.0"));
        }

        /// <summary>
        /// Tests that the <see cref="SemanticVersionNumber.ToString"/> method
        /// successfully returns the full semantic version number.
        /// </summary>
        [Fact]
        public static void ToStringReturnsFullVersionNumber()
        {
            var version = new SemanticVersionNumber("1.2.3-alpha+build.2");
            Assert.Equal("1.2.3-alpha+build.2", version.ToString());
        }

        /// <summary>
        /// Tests that a <see cref="SemanticVersionNumber"/> object is equal to
        /// itself.
        /// </summary>
        [Fact]
        public static void VersionIsEqualToItself()
        {
            var version = new SemanticVersionNumber(1, 0, 0);
            Assert.True(version.Equals(version));
        }

        /// <summary>
        /// Tests that a <see cref="SemanticVersionNumber"/> object is not
        /// equal to a null reference.
        /// </summary>
        [Fact]
        public static void VersionIsNotEqualToNull()
        {
            // ReSharper disable ExpressionIsAlwaysNull
            var version = new SemanticVersionNumber(1, 0, 0);
            SemanticVersionNumber nullVersion = null;
            Assert.False(version == nullVersion);
            Assert.False(nullVersion == version);
            Assert.True(nullVersion != version);
            Assert.True(version != nullVersion);
            object other = null;
            Assert.False(version.Equals(other));

            // ReSharper restore ExpressionIsAlwaysNull
        }

        /// <summary>
        /// Tests that a <see cref="SemanticVersionNumber"/> object cannot be
        /// compared to a string.
        /// </summary>
        [Fact]
        public static void VersionIsNotEqualToString()
        {
            var version = new SemanticVersionNumber(1, 0, 0);
            object versionNumber = "1.0.0";
            Assert.False(version.Equals(versionNumber));
        }

        /// <summary>
        /// Tests that a version is equal to itself.
        /// </summary>
        [Fact]
        public static void VersionIsTheSameAsItself()
        {
            var version = new SemanticVersionNumber(1, 0, 0);
            Assert.Equal(0, version.CompareTo(version));
            Assert.True(version.Equals(version));
        }

        /// <summary>
        /// Compares multiple version numbers together.
        /// </summary>
        [Fact]
        public static void VersionsAreComparedCorrectly()
        {
            var version1 = new SemanticVersionNumber("1.0.0-alpha");
            var version2 = new SemanticVersionNumber("1.0.0-alpha.1");
            var version3 = new SemanticVersionNumber("1.0.0-alpha.beta");
            var version4 = new SemanticVersionNumber("1.0.0-beta");
            var version5 = new SemanticVersionNumber("1.0.0-beta.2");
            var version6 = new SemanticVersionNumber("1.0.0-beta.11");
            var version7 = new SemanticVersionNumber("1.0.0-rc.1");
            var version8 = new SemanticVersionNumber("1.0.0");
            Assert.True(version1 < version2);
            Assert.True(version2 < version3);
            Assert.True(version3 < version4);
            Assert.True(version4 < version5);
            Assert.True(version5 < version6);
            Assert.True(version6 < version7);
            Assert.True(version7 < version8);
            Assert.True(version2 > version1);
            Assert.True(version3 > version2);
        }

        /// <summary>
        /// Tests that two <see cref="SemanticVersionNumber"/> objects are
        /// equal.
        /// </summary>
        [Fact]
        public static void VersionsAreEqual()
        {
            var version1 = new SemanticVersionNumber("1.0.0-alpha+build.1");
            var version2 = new SemanticVersionNumber("1.0.0-alpha+build.1");
            object version3 = version1;
            Assert.True(version1 == version2);
            Assert.True(version1.Equals(version3));
        }

        /// <summary>
        /// Tests that two <see cref="SemanticVersionNumber"/> objects are
        /// not equal.
        /// </summary>
        [Fact]
        public static void VersionsAreNotEqual()
        {
            var version1 = new SemanticVersionNumber("1.0.0");
            var version2 = new SemanticVersionNumber("1.0.0-alpha+build.1");
            object version3 = version2;
            Assert.True(version1 != version2);
            Assert.False(version1.Equals(version3));
        }

        /// <summary>
        /// Tests that a <see cref="SemanticVersionNumber"/> object cannot be
        /// compared to a null reference.
        /// </summary>
        [Fact]
        public static void VersionCannotBeComparedToNull()
        {
            // ReSharper disable ExpressionIsAlwaysNull
            var version1 = new SemanticVersionNumber(1, 0, 0);
            SemanticVersionNumber version2 = null;
            Assert.Throws<ArgumentNullException>(() => version1.CompareTo(version2));

            // ReSharper restore ExpressionIsAlwaysNull
        }

        [Fact]
        public static void CompareToCastsObjectsCorrectly()
        {
            var version1 = new SemanticVersionNumber(1, 0, 0);
            object version2 = new SemanticVersionNumber(1, 0, 0);
            Assert.Equal(0, version1.CompareTo(version2));
        }

        [Fact]
        public static void HashCodeIsSameForIdenticalVersionNumbers()
        {
            var version1 = new SemanticVersionNumber("1.0.0-alpha+build.23");
            var version2 = new SemanticVersionNumber("1.0.0-alpha+build.23");
            Assert.Equal(version1.GetHashCode(), version2.GetHashCode());
        }
    }
}
