//-----------------------------------------------------------------------------
// <copyright file="WindowsApiWrapper.cs" company="ImaginaryRealities">
// Copyright 2013 ImaginaryRealities, LLC
// </copyright>
//-----------------------------------------------------------------------------

namespace ImaginaryRealities.Framework.Diagnostics
{
    using System;
    using System.Runtime.InteropServices;

    using Microsoft.Win32.SafeHandles;

    internal class WindowsApiWrapper : IWindowsApi
    {
        public SafeFileHandle CreateFile(
            string fileName,
            uint desiredAccess,
            uint shareMode,
            IntPtr securityAttributes,
            uint creationDisposition,
            uint flagsAndAttributes,
            IntPtr templateFile)
        {
            return NativeMethods.CreateFile(
                fileName,
                desiredAccess,
                shareMode,
                securityAttributes,
                creationDisposition,
                flagsAndAttributes,
                templateFile);
        }

        public bool DeviceIoControl(
            SafeFileHandle deviceHandle,
            uint controlCode,
            IntPtr inBuffer,
            uint inBufferSize,
            IntPtr outBuffer,
            uint outBufferSize,
            out uint bytesReturned,
            IntPtr overlapped)
        {
            return NativeMethods.DeviceIoControl(
                deviceHandle,
                controlCode,
                inBuffer,
                inBufferSize,
                outBuffer,
                outBufferSize,
                out bytesReturned,
                overlapped);
        }

        public int GetLastError()
        {
            return Marshal.GetLastWin32Error();
        }
    }
}
