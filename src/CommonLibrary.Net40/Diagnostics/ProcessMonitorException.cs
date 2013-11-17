//-----------------------------------------------------------------------------
// <copyright file="ProcessMonitorException.cs" company="ImaginaryRealities">
// Copyright 2013 ImaginaryRealities, LLC
// </copyright>
//-----------------------------------------------------------------------------

namespace ImaginaryRealities.Framework.Diagnostics
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    /// <summary>
    /// Reports an exception with integration with the Process Monitor program.
    /// </summary>
    [Serializable]
    public class ProcessMonitorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessMonitorException"/> class.
        /// </summary>
        public ProcessMonitorException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessMonitorException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message for the exception.
        /// </param>
        public ProcessMonitorException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessMonitorException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message for the exception.
        /// </param>
        /// <param name="innerException">
        /// The inner exception that is being wrapped by this exception.
        /// </param>
        public ProcessMonitorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessMonitorException"/> class.
        /// </summary>
        /// <param name="info">
        /// A <see cref="SerializationInfo"/> object.
        /// </param>
        /// <param name="context">
        /// A <see cref="StreamingContext"/> object.
        /// </param>
        protected ProcessMonitorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
