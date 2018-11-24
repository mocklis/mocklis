// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Dummy
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public class DummyPropertyStep<TValue> : IPropertyStep<TValue>
    {
        public static readonly DummyPropertyStep<TValue> Instance = new DummyPropertyStep<TValue>();

        private DummyPropertyStep()
        {
        }

        public TValue Get(MemberMock memberMock)
        {
            return default;
        }

        public void Set(MemberMock memberMock, TValue value)
        {
        }
    }
}
