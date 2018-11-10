// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MedialPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public class MedialPropertyStep<TValue> : IPropertyStep<TValue>, IPropertyStepCaller<TValue>
    {
        protected IPropertyStep<TValue> NextStep { get; private set; } = MissingPropertyStep<TValue>.Instance;

        public TStep SetNextStep<TStep>(TStep step) where TStep : IPropertyStep<TValue>
        {
            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            NextStep = step;
            return step;
        }

        public virtual TValue Get(object instance, MemberMock memberMock)
        {
            return NextStep.Get(instance, memberMock);
        }

        public virtual void Set(object instance, MemberMock memberMock, TValue value)
        {
            NextStep.Set(instance, memberMock, value);
        }
    }
}
