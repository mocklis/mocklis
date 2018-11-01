// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyChangedEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.ChangeNotification
{
    #region Using Directives

    using System.ComponentModel;

    #endregion

    public class PropertyChangedEventStep : FieldBackedEventStep<PropertyChangedEventHandler>
    {
        public void Raise(object sender, string propertyName)
        {
            EventHandler?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}
