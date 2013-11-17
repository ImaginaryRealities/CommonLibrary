//-----------------------------------------------------------------------------
// <copyright file="ProcessMonitor.cs" company="ImaginaryRealities">
// Copyright 2013 ImaginaryRealities, LLC
// </copyright>
//-----------------------------------------------------------------------------

namespace ImaginaryRealities.Framework.Diagnostics
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Runtime.InteropServices;

    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Allows the program to write trace messages to the Process Monitor log.
    /// </summary>
    public class ProcessMonitor : IProcessMonitor
    {
        private readonly SafeFileHandle handle;
        private readonly IWindowsApi windowsApi;

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessMonitor"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public ProcessMonitor()
            : this(new WindowsApiWrapper())
        {
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "CreateFile",
            Justification = "MFC3: CreateFile is spelled correctly.")]
        internal ProcessMonitor(IWindowsApi windowsApi)
        {
            Contract.Requires<ArgumentNullException>(null != windowsApi);

            this.windowsApi = windowsApi;
            this.handle = windowsApi.CreateFile(
                "\\\\.\\Global\\ProcmonDebugLogger",
                0xC0000000U,
                7U,
                IntPtr.Zero,
                3U,
                0x80U,
                IntPtr.Zero);
            if (!this.handle.IsInvalid)
            {
                return;
            }

            var errorMessage = string.Format(
                CultureInfo.CurrentCulture,
                "CreateFile returned {0}",
                windowsApi.GetLastError());
            throw new ProcessMonitorException(errorMessage);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ProcessMonitor"/> class. 
        /// </summary>
        ~ProcessMonitor()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Disposes of the object when it is no longer necessary.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Writes a message to Process Monitor.
        /// </summary>
        /// <param name="message">
        /// The message to write to the Process Monitor log.
        /// </param>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly",
            MessageId = "DeviceIoControl", Justification = "MFC3: DeviceIoControl is spelled correctly.")]
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
            Justification = "MFC3: The message parameter is being validated using a code contract.")]
        public void WriteMessage(string message)
        {
            var buffer = IntPtr.Zero;
            try
            {
                buffer = Marshal.StringToHGlobalUni(message);
                uint bytesWritten;
                var inBufferSize = Convert.ToUInt32(message.Length * 2);
                var succeeded = this.windowsApi.DeviceIoControl(
                    this.handle,
                    0x4D600204U,
                    buffer,
                    inBufferSize,
                    IntPtr.Zero,
                    0,
                    out bytesWritten,
                    IntPtr.Zero);
                if (succeeded)
                {
                    return;
                }

                var errorMessage = string.Format(
                    CultureInfo.CurrentCulture,
                    "DeviceIoControl returned {0}",
                    this.windowsApi.GetLastError());
                throw new ProcessMonitorException(errorMessage);
            }
            finally
            {
                if (IntPtr.Zero != buffer)
                {
                    Marshal.FreeHGlobal(buffer);
                }
            }
        }

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
        public void WriteMessage(IFormatProvider formatProvider, string format, params object[] args)
        {
            var message = string.Format(formatProvider, format, args);
            this.WriteMessage(message);
        }

        /// <summary>
        /// Releases the handle to the Process Monitor output device.
        /// </summary>
        /// <param name="disposing">
        /// True if the object is being disposed, or false if the object is
        /// being garbage collected.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            this.handle.Dispose();
            if (!disposing)
            {
                return;
            }

            this.disposed = true;
        }
    }
}
