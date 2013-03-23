//-----------------------------------------------------------------------------
// <copyright file="SemanticVersionNumber.cs" company="ImaginaryRealities">
// Copyright 2013 ImaginaryRealities, LLC
// </copyright>
// <summary>
// This file implements the SemanticVersionNumber class. The 
// SemanticVersionNumber class represents a semantic version number and allows
// the application to obtain the values of the components for the semantic 
// version number.
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

namespace ImaginaryRealities.Framework
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Stores a semantic version number.
    /// </summary>
    [Serializable]
    public sealed class SemanticVersionNumber : IComparable, IComparable<SemanticVersionNumber>,
        IEquatable<SemanticVersionNumber>
    {
        /// <summary>
        /// The regular expression that will be used to match numbers.
        /// </summary>
        private static readonly Regex AllDigitsRegex = new Regex(
            @"^[0-9]+$", RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// The regular expression that will be used to parse the semantic
        /// version number for the project.
        /// </summary>
        private static readonly Regex SemanticVersionRegex =
            new Regex(
                @"^(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)(-(?<prerelease>[A-Za-z0-9\.\-]+))?(\+(?<build>[A-Za-z0-9\.\-]+))?$",
                RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersionNumber"/> class.
        /// </summary>
        /// <param name="major">
        /// The major version number component.
        /// </param>
        /// <param name="minor">
        /// The minor version number component.
        /// </param>
        /// <param name="patch">
        /// The patch version number component.
        /// </param>
        public SemanticVersionNumber(int major, int minor, int patch)
        {
            Contract.Requires<ArgumentException>(0 <= major, SemanticVersionNumberResources.InvalidMajorVersionNumber);
            Contract.Requires<ArgumentException>(0 <= minor, SemanticVersionNumberResources.InvalidMinorVersionNumber);
            Contract.Requires<ArgumentException>(0 <= patch, SemanticVersionNumberResources.InvalidPatchVersionNumber);
            Contract.Ensures(0 <= major);
            Contract.Ensures(0 <= minor);
            Contract.Ensures(0 <= patch);
            this.MajorVersion = major;
            this.MinorVersion = minor;
            this.PatchVersion = patch;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersionNumber"/> class.
        /// </summary>
        /// <param name="versionNumber">
        /// The semantic version number to parse.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="versionNumber"/> is not a semantic version number.
        /// </exception>
        public SemanticVersionNumber(string versionNumber)
        {
            Contract.Requires<ArgumentNullException>(null != versionNumber);
            Contract.Requires(!string.IsNullOrEmpty(versionNumber));
            Contract.Ensures(0 <= this.MajorVersion);
            Contract.Ensures(0 <= this.MinorVersion);
            Contract.Ensures(0 <= this.PatchVersion);
            var match = SemanticVersionRegex.Match(versionNumber);
            if (!match.Success)
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    SemanticVersionNumberResources.InvalidSemanticVersionNumber,
                    versionNumber);
                throw new ArgumentException(message);
            }

            this.MajorVersion = int.Parse(match.Groups["major"].Value, CultureInfo.InvariantCulture);
            this.MinorVersion = int.Parse(match.Groups["minor"].Value, CultureInfo.InvariantCulture);
            this.PatchVersion = int.Parse(match.Groups["patch"].Value, CultureInfo.InvariantCulture);
            this.PrereleaseVersion = match.Groups["prerelease"].Success ? match.Groups["prerelease"].Value : null;
            this.BuildVersion = match.Groups["build"].Success ? match.Groups["build"].Value : null;
        }

        /// <summary>
        /// Gets the build version number.
        /// </summary>
        /// <value>
        /// The optional build version number of the semantic version number,
        /// or <b>null</b> if a build version number was not specified in the
        /// semantic version number.
        /// </value>
        public string BuildVersion { get; private set; }

        /// <summary>
        /// Gets the major version number.
        /// </summary>
        /// <value>
        /// The major version number component of the semantic version number.
        /// </value>
        public int MajorVersion { get; private set; }

        /// <summary>
        /// Gets the minor version number.
        /// </summary>
        /// <value>
        /// The minor version number component of the semantic version number.
        /// </value>
        public int MinorVersion { get; private set; }

        /// <summary>
        /// Gets the patch version number.
        /// </summary>
        /// <value>
        /// The patch version number component of the semantic version number.
        /// </value>
        public int PatchVersion { get; private set; }

        /// <summary>
        /// Gets the prerelease version number.
        /// </summary>
        /// <value>
        /// The optional prerelease version number component of the semantic
        /// version number, or <b>null</b> if a prerelease version number was
        /// not specified in the semantic version number.
        /// </value>
        public string PrereleaseVersion { get; private set; }

        /// <summary>
        /// Compares two <see cref="SemanticVersionNumber"/> objects for
        /// equality.
        /// </summary>
        /// <param name="version">
        /// A <see cref="SemanticVersionNumber"/> object.
        /// </param>
        /// <param name="other">
        /// A second <see cref="SemanticVersionNumber"/> object.
        /// </param>
        /// <returns>
        /// <b>True</b> if the objects are equal, of <b>false</b> if the
        /// objects are not equal.
        /// </returns>
        public static bool operator ==(SemanticVersionNumber version, SemanticVersionNumber other)
        {
            return ReferenceEquals(null, version) ? ReferenceEquals(null, other) : version.Equals(other);
        }

        /// <summary>
        /// Compares two <see cref="SemanticVersionNumber"/> objects for
        /// inequality.
        /// </summary>
        /// <param name="version">
        /// A <see cref="SemanticVersionNumber"/> object.
        /// </param>
        /// <param name="other">
        /// A second <see cref="SemanticVersionNumber"/> object.
        /// </param>
        /// <returns>
        /// <b>True</b> if the two objects are not equal, or <b>false</b> if
        /// the objects are equal.
        /// </returns>
        public static bool operator !=(SemanticVersionNumber version, SemanticVersionNumber other)
        {
            return ReferenceEquals(null, version) ? !ReferenceEquals(null, other) : !version.Equals(other);
        }

        /// <summary>
        /// Compares two <see cref="SemanticVersionNumber"/> objects to
        /// determine if the first is less than the second.
        /// </summary>
        /// <param name="version">
        /// The first <see cref="SemanticVersionNumber"/> object.
        /// </param>
        /// <param name="other">
        /// The second <see cref="SemanticVersionNumber"/> object.
        /// </param>
        /// <returns>
        /// <b>True</b> if <paramref name="version"/> comes before
        /// <paramref name="other"/>; otherwise <b>false</b>.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
            Justification = "MFC3: The arguments are being validated using code contracts.")]
        public static bool operator <(SemanticVersionNumber version, SemanticVersionNumber other)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(null, version));
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(null, other));
            return 0 > version.CompareTo(other);
        }

        /// <summary>
        /// Compares two <see cref="SemanticVersionNumber"/> objects to
        /// determine if one is greater than the other.
        /// </summary>
        /// <param name="version">
        /// The first <see cref="SemanticVersionNumber"/> object.
        /// </param>
        /// <param name="other">
        /// The second <see cref="SemanticVersionNumber"/> object.
        /// </param>
        /// <returns>
        /// <b>True</b> if <paramref name="version"/> is greater than
        /// <paramref name="other"/>; otherwise <b>false</b>.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
            Justification = "MFC3: The arguments are being validated using code contracts.")]
        public static bool operator >(SemanticVersionNumber version, SemanticVersionNumber other)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(null, version));
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(null, other));
            return 0 < version.CompareTo(other);
        }

        /// <summary>
        /// Compares two objects.
        /// </summary>
        /// <param name="obj">
        /// The object to compare to this object.
        /// </param>
        /// <returns>
        /// <b>-1</b> if this object comes before the other object, <b>0</b>
        /// if both objects are identical, and <b>1</b> if this object comes
        /// after the other object.
        /// </returns>
        public int CompareTo(object obj)
        {
            var otherVersion = obj as SemanticVersionNumber;
            if (null == otherVersion)
            {
                throw new ArgumentException(SemanticVersionNumberResources.ObjectIsNotSemanticVersionNumber);
            }

            return this.CompareTo(otherVersion);
        }

        /// <summary>
        /// Compares two <see cref="SemanticVersionNumber"/> objects.
        /// </summary>
        /// <param name="other">
        /// The <see cref="SemanticVersionNumber"/> object to compare to this
        /// object.
        /// </param>
        /// <returns>
        /// <b>-1</b> if this object comes before the other object, <b>0</b>
        /// if both objects are identical, and <b>1</b> if this object comes
        /// after the other object.
        /// </returns>
        public int CompareTo(SemanticVersionNumber other)
        {
            if (null == other)
            {
                throw new ArgumentNullException(
                    "other", SemanticVersionNumberResources.ObjectIsNotSemanticVersionNumber);
            }

            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            var result = this.MajorVersion.CompareTo(other.MajorVersion);
            if (0 == result)
            {
                result = this.MinorVersion.CompareTo(other.MinorVersion);
                if (0 == result)
                {
                    result = this.PatchVersion.CompareTo(other.PatchVersion);
                    if (0 == result)
                    {
                        result = ComparePrereleaseVersions(this.PrereleaseVersion, other.PrereleaseVersion);
                        if (0 == result)
                        {
                            result = CompareBuildVersions(this.BuildVersion, other.BuildVersion);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Compares two objects for equality.
        /// </summary>
        /// <param name="obj">
        /// The object to compare to this object.
        /// </param>
        /// <returns>
        /// <b>True</b> if the objects are equal, or <b>false</b> if the
        /// objects are not equal.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            var other = obj as SemanticVersionNumber;
            return null != other && this.Equals(other);
        }

        /// <summary>
        /// Compares two <see cref="SemanticVersionNumber"/> objects for
        /// equality.
        /// </summary>
        /// <param name="other">
        /// The <see cref="SemanticVersionNumber"/> object to compare to this
        /// object.
        /// </param>
        /// <returns>
        /// <b>True</b> if the objects are equal, or <b>false</b> if the
        /// objects are not equal.
        /// </returns>
        public bool Equals(SemanticVersionNumber other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return this.MajorVersion == other.MajorVersion && this.MinorVersion == other.MinorVersion
                   && this.PatchVersion == other.PatchVersion && this.PrereleaseVersion == other.PrereleaseVersion
                   && this.BuildVersion == other.BuildVersion;
        }

        /// <summary>
        /// Calculates and returns the hash code for the object.
        /// </summary>
        /// <returns>
        /// The object's hash code.
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = 17;
            hashCode = (hashCode * 37) + this.MajorVersion;
            hashCode = (hashCode * 37) + this.MinorVersion;
            hashCode = (hashCode * 37) + this.PatchVersion;
            if (null != this.PrereleaseVersion)
            {
                hashCode = (hashCode * 37) + this.PrereleaseVersion.GetHashCode();
            }

            if (null != this.BuildVersion)
            {
                hashCode = (hashCode * 37) + this.BuildVersion.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Retrieves the semantic version number.
        /// </summary>
        /// <returns>
        /// The full semantic version number.
        /// </returns>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(
                CultureInfo.InvariantCulture, "{0}.{1}.{2}", this.MajorVersion, this.MinorVersion, this.PatchVersion);
            if (!string.IsNullOrEmpty(this.PrereleaseVersion))
            {
                stringBuilder.AppendFormat("-{0}", this.PrereleaseVersion);
            }

            if (!string.IsNullOrEmpty(this.BuildVersion))
            {
                stringBuilder.AppendFormat("+{0}", this.BuildVersion);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Compares two build version numbers.
        /// </summary>
        /// <param name="identifier1">
        /// The first build version number.
        /// </param>
        /// <param name="identifier2">
        /// The second build version number.
        /// </param>
        /// <returns>
        /// <b>-1</b> if <paramref name="identifier1"/> is less than
        /// <paramref name="identifier2"/>, <b>0</b> if the version numbers are
        /// equal, or <b>1</b> if <paramref name="identifier1"/> is greater
        /// than <paramref name="identifier2"/>.
        /// </returns>
        private static int CompareBuildVersions(string identifier1, string identifier2)
        {
            var result = 0;
            var hasIdentifier1 = !string.IsNullOrEmpty(identifier1);
            var hasIdentifier2 = !string.IsNullOrEmpty(identifier2);
            if (hasIdentifier1 && !hasIdentifier2)
            {
                return 1;
            }

            if (!hasIdentifier1 && hasIdentifier2)
            {
                return -1;
            }

            if (hasIdentifier1)
            {
                var dotDelimiter = new[] { '.' };
                var parts1 = identifier1.Split(dotDelimiter, StringSplitOptions.RemoveEmptyEntries);
                var parts2 = identifier2.Split(dotDelimiter, StringSplitOptions.RemoveEmptyEntries);
                var max = Math.Max(parts1.Length, parts2.Length);
                for (var i = 0; i < max; i++)
                {
                    if (i == parts1.Length && i != parts2.Length)
                    {
                        result = -1;
                        break;
                    }

                    if (i != parts1.Length && i == parts2.Length)
                    {
                        result = 1;
                        break;
                    }

                    var part1 = parts1[i];
                    var part2 = parts2[i];
                    if (AllDigitsRegex.IsMatch(part1) && AllDigitsRegex.IsMatch(part2))
                    {
                        var value1 = int.Parse(part1, CultureInfo.InvariantCulture);
                        var value2 = int.Parse(part2, CultureInfo.InvariantCulture);
                        result = value1.CompareTo(value2);
                    }
                    else
                    {
                        result = string.Compare(part1, part2, StringComparison.Ordinal);
                    }

                    if (0 != result)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Compares two prerelease version numbers.
        /// </summary>
        /// <param name="identifier1">
        /// The first prerelease version number.
        /// </param>
        /// <param name="identifier2">
        /// The second prerelease version number.
        /// </param>
        /// <returns>
        /// <b>-1</b> if <paramref name="identifier1"/> is less than
        /// <paramref name="identifier2"/>, <b>0</b> if the version numbers are
        /// equal, or <b>1</b> if <paramref name="identifier1"/> is greater
        /// than <paramref name="identifier2"/>.
        /// </returns>
        private static int ComparePrereleaseVersions(string identifier1, string identifier2)
        {
            var result = 0;
            var hasIdentifier1 = !string.IsNullOrEmpty(identifier1);
            var hasIdentifier2 = !string.IsNullOrEmpty(identifier2);
            if (hasIdentifier1 && !hasIdentifier2)
            {
                return -1;
            }

            if (!hasIdentifier1 && hasIdentifier2)
            {
                return 1;
            }

            if (hasIdentifier1)
            {
                var dotDelimiter = new[] { '.' };
                var parts1 = identifier1.Split(dotDelimiter, StringSplitOptions.RemoveEmptyEntries);
                var parts2 = identifier2.Split(dotDelimiter, StringSplitOptions.RemoveEmptyEntries);
                var max = Math.Max(parts1.Length, parts2.Length);
                for (var i = 0; i < max; i++)
                {
                    if (i == parts1.Length && i != parts2.Length)
                    {
                        result = -1;
                        break;
                    }

                    if (i != parts1.Length && i == parts2.Length)
                    {
                        result = 1;
                        break;
                    }

                    var part1 = parts1[i];
                    var part2 = parts2[i];
                    if (AllDigitsRegex.IsMatch(part1) && AllDigitsRegex.IsMatch(part2))
                    {
                        var value1 = int.Parse(part1, CultureInfo.InvariantCulture);
                        var value2 = int.Parse(part2, CultureInfo.InvariantCulture);
                        result = value1.CompareTo(value2);
                    }
                    else
                    {
                        result = string.Compare(part1, part2, StringComparison.Ordinal);
                    }

                    if (0 != result)
                    {
                        break;
                    }
                }
            }

            return result;
        }
    }
}
