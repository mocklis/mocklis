// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingMethodStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Missing
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public sealed class MissingMethodStep<TParam, TResult> : IMethodStep<TParam, TResult>
    {
        public static readonly MissingMethodStep<TParam, TResult> Instance = new MissingMethodStep<TParam, TResult>();

        private MissingMethodStep()
        {
        }

        public TResult Call(IMockInfo mockInfo, TParam param)
        {
            throw new MockMissingException(MockType.Method, mockInfo);
        }
    }
}
