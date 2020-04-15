// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEventListSink.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Helpers
{
    #region Using Directives

    using System.Collections.Generic;
    using Serilog.Core;
    using Serilog.Events;

    #endregion

    public class LogEventListSink : ILogEventSink
    {
        private readonly List<LogEvent> _logEvents = new List<LogEvent>();

        public int Count
        {
            get
            {
                lock (_logEvents)
                {
                    return _logEvents.Count;
                }
            }
        }

        public LogEvent this[int index]
        {
            get
            {
                lock (_logEvents)
                {
                    return _logEvents[index];
                }
            }
        }

        public void Emit(LogEvent logEvent)
        {
            lock (_logEvents)
            {
                _logEvents.Add(logEvent);
            }
        }
    }
}
