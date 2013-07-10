﻿//-----------------------------------------------------------------------------
// <copyright file="ProcessMonitor.cs" company="ImaginaryRealities">
// Copyright 2013 ImaginaryRealities, LLC
// </copyright>
//-----------------------------------------------------------------------------

namespace ImaginaryRealities.Framework.Diagnostics
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Allows the program to write trace messages to the Process Monitor log.
    /// </summary>
    public class ProcessMonitor : IProcessMonitor
    {
        private static readonly IntPtr InvalidHandleValue = new IntPtr(-1);

        private readonly IntPtr handle;
        private readonly IWindowsApi windowsApi;

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessMonitor"/> class.
        /// </summary>
        public ProcessMonitor()
            : this(new WindowsApiWrapper())
        {
        }

        internal ProcessMonitor(IWindowsApi windowsApi)
        {
            Contract.Requires<ArgumentNullException>(null != windowsApi);

            this.windowsApi = windowsApi;
            this.handle = windowsApi.CreateFile(
                "\\\\.\\Global\\ProcmonDebugLogger", 0xC0000000U, 7U, IntPtr.Zero, 3U, 0x80U, IntPtr.Zero);
            if (InvalidHandleValue != this.handle)
            {
                return;
            }

            var errorMessage = string.Format(
                CultureInfo.CurrentCulture, "CreateFile returned {0}", windowsApi.GetLastError());
            throw new Exception(errorMessage);
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
        public void WriteMessage(string message)
        {
            var buffer = IntPtr.Zero;
            try
            {
                buffer = Marshal.StringToHGlobalUni(message);
                uint bytesWritten;
                var inBufferSize = Convert.ToUInt32(message.Length * 2);
                var succeeded = this.windowsApi.DeviceIoControl(
                    this.handle, 0x4D600204U, buffer, inBufferSize, IntPtr.Zero, 0, out bytesWritten, IntPtr.Zero);
                if (succeeded)
                {
                    return;
                }

                var errorMessage = string.Format(
                    CultureInfo.CurrentCulture, "DeviceIoControl returned {0}", this.windowsApi.GetLastError());
                throw new Exception(errorMessage);
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

            if (!this.windowsApi.CloseHandle(this.handle))
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture, "CloseHandle returned {0}", this.windowsApi.GetLastError());
                throw new Exception(message);
            }

            if (!disposing)
            {
                return;
            }

            this.disposed = true;
        }
    }
}