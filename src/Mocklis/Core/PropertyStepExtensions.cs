// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyStepExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    /// <summary>
    ///     Extension methods for the IPropertyStep interface.
    /// </summary>
    public static class PropertyStepExtensions
    {
        /// <summary>
        ///     Reads a value using a property step. If the step is <c>null</c>, uses the strictness of the mock
        ///     to decide whether to throw a <see cref="MockMissingException" /> (VeryStrict) or to return a
        ///     default value (Lenient or Strict).
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="propertyStep">The property step (can be null) through which the value is read.</param>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <returns>The value being read.</returns>
        /// <seealso cref="IPropertyStep{TValue}" />
        public static TValue GetWithStrictnessCheckIfNull<TValue>(this IPropertyStep<TValue>? propertyStep, IMockInfo mockInfo)
        {
            if (propertyStep == null)
            {
                if (mockInfo.Strictness != Strictness.VeryStrict)
                {
                    return default!;
                }

                throw new MockMissingException(MockType.PropertyGet, mockInfo);
            }

            return propertyStep.Get(mockInfo);
        }

        /// <summary>
        ///     Writes a value using a property step. If the step is <c>null</c>, uses the strictness of the mock
        ///     to decide whether to throw a <see cref="MockMissingException" /> (VeryStrict) or to do nothing
        ///     (Lenient or Strict).
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="propertyStep">The property step (can be null) through which the value is read.</param>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="value">The value being written.</param>
        public static void SetWithStrictnessCheckIfNull<TValue>(this IPropertyStep<TValue>? propertyStep, IMockInfo mockInfo, TValue value)
        {
            if (propertyStep == null)
            {
                if (mockInfo.Strictness != Strictness.VeryStrict)
                {
                    return;
                }

                throw new MockMissingException(MockType.PropertySet, mockInfo);
            }

            propertyStep.Set(mockInfo, value);
        }
    }
}
