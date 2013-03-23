//-----------------------------------------------------------------------------
// <copyright file="SemanticVersionAttribute.cs" 
//            company="ImaginaryRealities">
// Copyright 2013 ImaginaryRealities, LLC
// </copyright>
// <summary>
// This file implements the SemanticVersionAttribute class. The
// SemanticVersionAttribute class is used to annotate an assembly with 
// the semantic version for the assembly or product.
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
    using System.Runtime.InteropServices;

    /// <summary>
    /// Attribute that can be used to annotate an assembly with the semantic
    /// version number of the assembly or product.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments",
        Justification =
            "MFC3: The versionNumber parameter is converted to a SemanticVersionNumber object and exposed through the VersionNumber property.")]
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [Serializable]
    public sealed class SemanticVersionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersionAttribute"/> class.
        /// </summary>
        /// <param name="versionNumber">
        /// The semantic version number for the assembly or product.
        /// </param>
        public SemanticVersionAttribute(string versionNumber)
        {
            Contract.Requires<ArgumentNullException>(!object.ReferenceEquals(null, versionNumber));
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(versionNumber));
            Contract.Ensures(!object.ReferenceEquals(null, this.VersionNumber));
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
