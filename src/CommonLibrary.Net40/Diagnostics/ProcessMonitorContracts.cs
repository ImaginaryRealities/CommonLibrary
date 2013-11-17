//-----------------------------------------------------------------------------
// <copyright file="ProcessMonitorContracts.cs" company="ImaginaryRealities">
// Copyright 2013 ImaginaryRealities, LLC
// </copyright>
//-----------------------------------------------------------------------------

namespace ImaginaryRealities.Framework.Diagnostics
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IProcessMonitor))]
    internal abstract class ProcessMonitorContracts : IProcessMonitor
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void WriteMessage(string message)
        {
            Contract.Requires<ArgumentNullException>(null != message, "The message cannot be a null value.");
        }

        public void WriteMessage(IFormatProvider formatProvider, string format, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(
                null != formatProvider,
                "The formatProvider parameter cannot be a null value.");
            Contract.Requires<ArgumentNullException>(null != format, "The format parameter cannot be a null value.");
        }
    }
}
