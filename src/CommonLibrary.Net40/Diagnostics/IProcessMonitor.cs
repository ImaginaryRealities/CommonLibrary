//-----------------------------------------------------------------------------
// <copyright file="IProcessMonitor.cs" company="ImaginaryRealities">
// Copyright 2013 ImaginaryRealities, LLC
// </copyright>
//-----------------------------------------------------------------------------

namespace ImaginaryRealities.Framework.Diagnostics
{
    using System;

    /// <summary>
    /// Abstracts the <see cref="ProcessMonitor"/> API to support mocking and
    /// unit testing.
    /// </summary>
    public interface IProcessMonitor : IDisposable
    {
        /// <summary>
        /// Writes a message to Process Monitor.
        /// </summary>
        /// <param name="message">
        /// The message to write to the Process Monitor log.
        /// </param>
        void WriteMessage(string message);

        /// <summary>
        /// Formats and writes a message to the Process Monitor log.
        /// </summary>
        /// <param name="formatProvider">
        /// A <see cref="IFormatProvider"/> object to use to format the
        /// message.
        /// </param>
        /// <param name="format">
        /// The format of the message to write to the Process Monitor log.
        /// </param>
        /// <param name="args">
        /// An array containing the arguments for the formatted message.
        /// </param>
        void WriteMessage(IFormatProvider formatProvider, string format, params object[] args);
    }
}