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
//-----------------------------------------------------------------------------

namespace ImaginaryRealities.Framework
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Stores a semantic version number.
    /// </summary>
    [Serializable]
    public sealed class SemanticVersionNumber
    {
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
            this.BuildVersion = match.Groups["build"].Success ? match.Groups["success"].Value : null;
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
    }
}
