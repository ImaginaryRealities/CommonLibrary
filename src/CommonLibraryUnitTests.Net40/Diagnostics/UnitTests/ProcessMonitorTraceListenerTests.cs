//-----------------------------------------------------------------------------
// <copyright file="ProcessMonitorTraceListenerTests.cs" 
//            company="ImaginaryRealities">
// Copyright 2013 ImaginaryRealities, LLC
// </copyright>
//-----------------------------------------------------------------------------

namespace ImaginaryRealities.Framework.Diagnostics.UnitTests
{
    using Moq;

    using Xunit;

    public class ProcessMonitorTraceListenerTests
    {
        [Fact]
        public void DisposeDisposesOfInternalProcessMonitor()
        {
            var mockProcessMonitor = new Mock<IProcessMonitor>(MockBehavior.Strict);
            mockProcessMonitor.Setup(x => x.Dispose()).Verifiable();
            using (new ProcessMonitorTraceListener(mockProcessMonitor.Object))
            {
            }

            mockProcessMonitor.VerifyAll();
        }

        [Fact]
        public void WriteWritesAMessageToProcessMonitor()
        {
            var mockProcessMonitor = new Mock<IProcessMonitor>(MockBehavior.Strict);
            mockProcessMonitor.Setup(x => x.Dispose());
            mockProcessMonitor.Setup(x => x.WriteMessage("Test message")).Verifiable();
            using (var traceListener = new ProcessMonitorTraceListener(mockProcessMonitor.Object))
            {
                traceListener.Write("Test message");
            }

            mockProcessMonitor.VerifyAll();
        }

        [Fact]
        public void WriteLineWritesAMessageToProcessMonitor()
        {
            var mockProcessMonitor = new Mock<IProcessMonitor>(MockBehavior.Strict);
            mockProcessMonitor.Setup(x => x.Dispose());
            mockProcessMonitor.Setup(x => x.WriteMessage("Test message")).Verifiable();
            using (var traceListener = new ProcessMonitorTraceListener(mockProcessMonitor.Object))
            {
                traceListener.WriteLine("Test message");
            }

            mockProcessMonitor.VerifyAll();
        }
    }
}
