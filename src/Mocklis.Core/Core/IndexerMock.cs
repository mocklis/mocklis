// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexerMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Steps.Missing;

    #endregion

    public sealed class IndexerMock<TKey, TValue> : MemberMock, ICanHaveNextIndexerStep<TKey, TValue>
    {
        private IIndexerStep<TKey, TValue> _nextStep = MissingIndexerStep<TKey, TValue>.Instance;

        public IndexerMock(object mockInstance, string mocklisClassName, string interfaceName, string memberName, string memberMockName)
            : base(mockInstance, mocklisClassName, interfaceName, memberName, memberMockName)
        {
        }

        TStep ICanHaveNextIndexerStep<TKey, TValue>.SetNextStep<TStep>(TStep step)
        {
            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            _nextStep = step;
            return step;
        }

        public TValue this[TKey key]
        {
            get => _nextStep.Get(this, key);
            set => _nextStep.Set(this, key, value);
        }
    }
}
