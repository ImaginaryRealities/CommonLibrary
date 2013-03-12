//-----------------------------------------------------------------------------
// <copyright file="SemanticVersionNumberAttribute.cs" 
//            company="ImaginaryRealities">
// Copyright 2013 ImaginaryRealities, LLC
// </copyright>
// <summary>
// This file implements the SemanticVersionNumberAttribute class. The
// SemanticVersionNumberAttribute class is used to annotate an assembly with 
// the semantic version for the assembly or product.
// </summary>
//-----------------------------------------------------------------------------

namespace ImaginaryRealities.Common
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Attribute that can be used to annotate an assembly with the semantic
    /// version number of the assembly or product.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments",
        Justification =
            "MFC3: The versionNumber parameter is converted to a SemanticVersionNumber object and exposed through the VersionNumber property.")]
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    [Serializable]
    public sealed class SemanticVersionNumberAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersionNumberAttribute"/> class.
        /// </summary>
        /// <param name="versionNumber">
        /// The semantic version number for the assembly or product.
        /// </param>
        public SemanticVersionNumberAttribute(string versionNumber)
        {
            Contract.Requires<ArgumentNullException>(null != versionNumber);
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(versionNumber));
            Contract.Ensures(null != this.VersionNumber);
            this.VersionNumber = new SemanticVersionNumber(versionNumber);
        }

        /// <summary>
        /// Gets the semantic version number for the assembly or product.
        /// </summary>
        /// <value>
        /// A <see cref="SemanticVersionNumber"/> objects.
        /// </value>
        public SemanticVersionNumber VersionNumber { get; private set; }
    }
}
