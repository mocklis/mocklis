// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnPropertyStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Return' property step that returns a given value every time it's read.
    ///     Inherits from the <see cref="PropertyStepWithNext{TValue}" /> class.
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    /// <seealso cref="PropertyStepWithNext{TValue}" />
    public class ReturnPropertyStep<TValue> : PropertyStepWithNext<TValue>
    {
        private readonly TValue _value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReturnPropertyStep{TValue}" /> class.
        /// </summary>
        /// <param name="value">The value to be returned.</param>
        public ReturnPropertyStep(TValue value)
        {
            _value = value;
        }

        /// <summary>
        ///     Called when a value is read from the property.
        ///     This implementation returns a given value every time.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <returns>The value being read.</returns>
        public override TValue Get(IMockInfo mockInfo)
        {
            return _value;
        }
    }
}
