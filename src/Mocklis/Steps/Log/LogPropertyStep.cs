// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogPropertyStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Log
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Log' property step. This class cannot be inherited.
    ///     Implements the <see cref="PropertyStepWithNext{TValue}" />
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    /// <seealso cref="PropertyStepWithNext{TValue}" />
    public sealed class LogPropertyStep<TValue> : PropertyStepWithNext<TValue>
    {
        private readonly ILogContext _logContext;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LogPropertyStep{TValue}" /> class.
        /// </summary>
        /// <param name="logContext">The log context used to write log lines.</param>
        public LogPropertyStep(ILogContext logContext)
        {
            _logContext = logContext ?? throw new ArgumentNullException(nameof(logContext));
        }

        /// <summary>
        ///     Called when a value is read from the property.
        ///     This implementation logs before and after the value has been read, along with any exceptions thrown.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <returns>The value being read.</returns>
        public override TValue Get(IMockInfo mockInfo)
        {
            _logContext.LogBeforePropertyGet(mockInfo);
            TValue result;
            try
            {
                result = base.Get(mockInfo);
            }
            catch (Exception exception)
            {
                _logContext.LogPropertyGetException(mockInfo, exception);
                throw;
            }

            _logContext.LogAfterPropertyGet(mockInfo, result);
            return result;
        }

        /// <summary>
        ///     Called when a value is written to the property.
        ///     This implementation logs before and after the value has been written, along with any exceptions thrown.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="value">The value being written.</param>
        public override void Set(IMockInfo mockInfo, TValue value)
        {
            _logContext.LogBeforePropertySet(mockInfo, value);
            try
            {
                base.Set(mockInfo, value);
            }
            catch (Exception exception)
            {
                _logContext.LogPropertySetException(mockInfo, exception);
                throw;
            }

            _logContext.LogAfterPropertySet(mockInfo);
        }
    }
}
