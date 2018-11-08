// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoredGenericEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Stored
{
    #region Using Directives

    using System;

    #endregion

    public class StoredGenericEventStep<TArgs> : StoredEventStep<EventHandler<TArgs>>
    {
        public void Raise(object sender, TArgs e)
        {
            EventHandler?.Invoke(sender, e);
        }
    }
}
