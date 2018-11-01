// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FieldBackedGenericEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;

    #endregion

    public class FieldBackedGenericEventStep<TArgs> : FieldBackedEventStep<EventHandler<TArgs>>
    {
        public void Raise(object sender, TArgs e)
        {
            EventHandler?.Invoke(sender, e);
        }
    }
}
