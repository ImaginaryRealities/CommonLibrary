//-----------------------------------------------------------------------------
// <copyright file="ProcessMonitorTraceListener.cs" 
//            company="ImaginaryRealities">
// Copyright 2013 ImaginaryRealities, LLC
// </copyright>
//-----------------------------------------------------------------------------

namespace ImaginaryRealities.Framework.Diagnostics
{
    using System.Diagnostics;

    /// <summary>
    /// Trace listener class that will write trace messages to the Process
    /// Monitor log.
    /// </summary>
    public class ProcessMonitorTraceListener : TraceListener
    {
        private readonly IProcessMonitor processMonitor;

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessMonitorTraceListener"/> class.
        /// </summary>
        public ProcessMonitorTraceListener()
        {
            this.processMonitor = new ProcessMonitor();
        }

        internal ProcessMonitorTraceListener(IProcessMonitor processMonitor)
        {
            this.processMonitor = processMonitor;
        }

        /// <summary>
        /// When overridden in a derived class, writes the specified message to the listener you create in the derived class.
        /// </summary>
        /// <param name="message">A message to write. </param><filterpriority>2</filterpriority>
        public override void Write(string message)
        {
            this.processMonitor.WriteMessage(message);
        }

        /// <summary>
        /// When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write. </param><filterpriority>2</filterpriority>
        public override void WriteLine(string message)
        {
            this.processMonitor.WriteMessage(message);
        }

        /// <summary>
        /// Releases the handle to the Process Monitor output device.
        /// </summary>
        /// <param name="disposing">
        /// True if the object is being disposed, or false if the object is
        /// being finalized.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            base.Dispose(disposing);
            this.processMonitor.Dispose();

            if (!disposing)
            {
                return;
            }

            this.disposed = true;
        }
    }
}
