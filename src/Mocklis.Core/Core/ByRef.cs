// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ByRef.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public sealed class ByRef<T>
    {
        public static ref T Wrap(T value)
        {
            return ref new ByRef<T>(value).Value;
        }

        private T _value;

        private ByRef(T value)
        {
            _value = value;
        }

        private ref T Value => ref _value;
    }
}
