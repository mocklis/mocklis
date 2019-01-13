// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGate.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Gate
{
    #region Using Directives

    using System.Threading.Tasks;

    #endregion

    public interface IGate
    {
        Task GatePassed { get; }
    }
}
