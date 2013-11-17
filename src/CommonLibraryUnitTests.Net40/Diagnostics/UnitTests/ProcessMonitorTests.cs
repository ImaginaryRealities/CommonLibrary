//-----------------------------------------------------------------------------
// <copyright file="ProcessMonitorTests.cs" company="ImaginaryRealities">
// Copyright 2013 ImaginaryRealities, LLC
// </copyright>
//-----------------------------------------------------------------------------

namespace ImaginaryRealities.Framework.Diagnostics.UnitTests
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;

    using Microsoft.Win32.SafeHandles;

    using Moq;

    using Xunit;

    public class ProcessMonitorTests
    {
        [Fact]
        public void ConstructorGetsHandleToProcessMonitor()
        {
            var mockWindowsApi = new Mock<IWindowsApi>(MockBehavior.Strict);
            mockWindowsApi.Setup(
                x =>
                x.CreateFile("\\\\.\\Global\\ProcmonDebugLogger", 0xC0000000U, 7U, IntPtr.Zero, 3U, 0x80U, IntPtr.Zero))
                .Returns(new SafeFileHandle(new IntPtr(10), false))
                .Verifiable();
            new ProcessMonitor(mockWindowsApi.Object);
            mockWindowsApi.VerifyAll();
        }

        [Fact]
        public void ConstructorThrowsExceptionIfErrorOccurs()
        {
            var mockWindowsApi = new Mock<IWindowsApi>(MockBehavior.Strict);
            mockWindowsApi.Setup(
                x =>
                x.CreateFile("\\\\.\\Global\\ProcmonDebugLogger", 0xC0000000U, 7U, IntPtr.Zero, 3U, 0x80U, IntPtr.Zero))
                .Returns(new SafeFileHandle(new IntPtr(-1), false));
            mockWindowsApi.Setup(x => x.GetLastError()).Returns(5);
            var exception = Assert.Throws<Exception>(() => new ProcessMonitor(mockWindowsApi.Object));
            Assert.Equal("CreateFile returned 5", exception.Message);
        }

        [Fact]
        public void DisposeClosesTheDeviceHandle()
        {
            var mockWindowsApi = new Mock<IWindowsApi>(MockBehavior.Strict);
            mockWindowsApi.Setup(
                x =>
                x.CreateFile("\\\\.\\Global\\ProcmonDebugLogger", 0xC0000000U, 7U, IntPtr.Zero, 3U, 0x80U, IntPtr.Zero))
                .Returns(new SafeFileHandle(new IntPtr(5), false));
            using (new ProcessMonitor(mockWindowsApi.Object))
            {
            }

            mockWindowsApi.VerifyAll();
        }

        [Fact]
        public void WriteMessageSendsMessageToProcessMonitor()
        {
            var mockWindowsApi = new Mock<IWindowsApi>(MockBehavior.Strict);
            var deviceHandle = new SafeFileHandle(new IntPtr(5), false);
            mockWindowsApi.Setup(
                x =>
                x.CreateFile("\\\\.\\Global\\ProcmonDebugLogger", 0xC0000000U, 7U, IntPtr.Zero, 3U, 0x80U, IntPtr.Zero))
                .Returns(deviceHandle);
            uint bytesReturned;
            mockWindowsApi.Setup(
                x =>
                x.DeviceIoControl(
                    deviceHandle,
                    0x4D600204U,
                    It.Is<IntPtr>(ptr => "Test message" == Marshal.PtrToStringUni(ptr)),
                    24U,
                    IntPtr.Zero,
                    0,
                    out bytesReturned,
                    IntPtr.Zero)).Returns(true).Verifiable();
            using (var processMonitor = new ProcessMonitor(mockWindowsApi.Object))
            {
                processMonitor.WriteMessage("Test message");
            }

            mockWindowsApi.VerifyAll();
        }

        [Fact]
        public void WriteMessageThrowsExceptionIfDeviceIoControlFails()
        {
            var mockWindowsApi = new Mock<IWindowsApi>(MockBehavior.Strict);
            var deviceHandle = new SafeFileHandle(new IntPtr(5), false);
            mockWindowsApi.Setup(
                x =>
                x.CreateFile("\\\\.\\Global\\ProcmonDebugLogger", 0xC0000000U, 7U, IntPtr.Zero, 3U, 0x80U, IntPtr.Zero))
                .Returns(deviceHandle);
            uint bytesReturned;
            mockWindowsApi.Setup(
                x =>
                x.DeviceIoControl(
                    deviceHandle, 0x4D600204U, It.IsAny<IntPtr>(), 24U, IntPtr.Zero, 0, out bytesReturned, IntPtr.Zero))
                .Returns(false)
                .Verifiable();
            mockWindowsApi.Setup(x => x.GetLastError()).Returns(2);
            var exception = Assert.Throws<Exception>(
                () =>
                    {
                        using (var processMonitor = new ProcessMonitor(mockWindowsApi.Object))
                        {
                            processMonitor.WriteMessage("Test message");
                        }
                    });
            Assert.Equal("DeviceIoControl returned 2", exception.Message);
        }

        [Fact]
        public void WriteMessageFormatsMessageAndWritesMessageToDevice()
        {
            var mockWindowsApi = new Mock<IWindowsApi>(MockBehavior.Strict);
            var deviceHandle = new SafeFileHandle(new IntPtr(5), false);
            mockWindowsApi.Setup(
                x =>
                x.CreateFile("\\\\.\\Global\\ProcmonDebugLogger", 0xC0000000U, 7U, IntPtr.Zero, 3U, 0x80U, IntPtr.Zero))
                .Returns(deviceHandle);
            uint bytesReturned;
            mockWindowsApi.Setup(
                x =>
                x.DeviceIoControl(
                    deviceHandle,
                    0x4D600204U,
                    It.Is<IntPtr>(ptr => "Hello, World!" == Marshal.PtrToStringUni(ptr)),
                    26U,
                    IntPtr.Zero,
                    0,
                    out bytesReturned,
                    IntPtr.Zero)).Returns(true).Verifiable();
            using (var processMonitor = new ProcessMonitor(mockWindowsApi.Object))
            {
                processMonitor.WriteMessage(CultureInfo.CurrentCulture, "Hello, {0}!", "World");
            }

            mockWindowsApi.VerifyAll();
        }
    }
}
