//-----------------------------------------------------------------------------
// <copyright file="IWindowsApi.cs" company="ImaginaryRealities">
// Copyright 2013 ImaginaryRealities, LLC
// </copyright>
//-----------------------------------------------------------------------------

namespace ImaginaryRealities.Framework.Diagnostics
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Abstracts the Win32 API functions used for supporting integration
    /// with Process Monitor for mocking and unit testing.
    /// </summary>
    [CLSCompliant(false)]
    public interface IWindowsApi
    {
        /// <summary>
        /// Creates or opens a file or I/O device.
        /// </summary>
        /// <param name="fileName">
        /// The name of the file or device to be created or opened.
        /// </param>
        /// <param name="desiredAccess">
        /// The requested access to the file or device.
        /// </param>
        /// <param name="shareMode">
        /// The requested sharing mode of the file or device.
        /// </param>
        /// <param name="securityAttributes">
        /// A pointer to a SECURITY_ATTRIBUTES structure.
        /// </param>
        /// <param name="creationDisposition">
        /// An action to take on a file or device that exists or does not
        /// exist.
        /// </param>
        /// <param name="flagsAndAttributes">
        /// The file or device attributes and flags.
        /// </param>
        /// <param name="templateFile">
        /// The handle of a template file.
        /// </param>
        /// <returns>
        /// The handle of the file or device, or -1 if an error occurred.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "flags",
            Justification = "MFC3: flagsAndAttributes matches the native API.")]
        SafeFileHandle CreateFile(
            string fileName,
            uint desiredAccess,
            uint shareMode,
            IntPtr securityAttributes,
            uint creationDisposition,
            uint flagsAndAttributes,
            IntPtr templateFile);

        /// <summary>
        /// Sends a control code directly to a specified device driver,
        /// causing the corresponding device to perform the corresponding
        /// operation.
        /// </summary>
        /// <param name="deviceHandle">
        /// A handle to the device on which the operation is to be performed.
        /// </param>
        /// <param name="controlCode">
        /// The control code for the operation.
        /// </param>
        /// <param name="inBuffer">
        /// A pointer to the input buffer that contains the data required to
        /// perform the operation.
        /// </param>
        /// <param name="inBufferSize">
        /// The size of <paramref name="inBuffer"/> in bytes.
        /// </param>
        /// <param name="outBuffer">
        /// A pointer to the output buffer that is to receive the data returned
        /// by the operation.
        /// </param>
        /// <param name="outBufferSize">
        /// The size of <paramref name="outBuffer"/> in bytes.
        /// </param>
        /// <param name="bytesReturned">
        /// A pointer to a variable that receives the size of the data stored
        /// in <paramref name="outBuffer"/> in bytes.
        /// </param>
        /// <param name="overlapped">
        /// A pointer to an OVERLAPPED structure.
        /// </param>
        /// <returns>
        /// True if the operation succeeded, or false if the operation failed.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "6#",
            Justification = "MFC3: The DeviceIoControl native API defines the out parameter.")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Io",
            Justification = "MFC3: DeviceIoControl matches the name of the native API.")]
        bool DeviceIoControl(
            SafeFileHandle deviceHandle,
            uint controlCode,
            IntPtr inBuffer,
            uint inBufferSize,
            IntPtr outBuffer,
            uint outBufferSize,
            out uint bytesReturned,
            IntPtr overlapped);

        /// <summary>
        /// Gets the last error returned by a Win32 API call.
        /// </summary>
        /// <returns>
        /// The error from the last Win32 API call.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "MFC3: GetLastError is the name of a native API.")]
        int GetLastError();
    }
}
