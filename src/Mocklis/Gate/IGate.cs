// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGate.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Gate
{
    #region Using Directives

    using System.Threading.Tasks;

    #endregion

    public interface IGate
    {
        Task GatePassed { get; }
    }
}
