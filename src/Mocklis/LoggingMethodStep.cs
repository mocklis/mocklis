// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggingMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.StepCallerBaseClasses;

    #endregion

    public sealed class LoggingMethodStep<TParam, TResult> : MethodStepCaller<TParam, TResult>, IMethodStep<TParam, TResult>
    {
        public TResult Call(object instance, MemberMock memberMock, TParam param)
        {
            Console.WriteLine(FormattableString.Invariant($"Calling '{memberMock.InterfaceName}.{memberMock.MemberName}' with parameter: {param}"));
            var returnValue = NextStep.Call(instance, memberMock, param);
            Console.WriteLine(
                FormattableString.Invariant($"Returned from '{memberMock.InterfaceName}.{memberMock.MemberName}' with result: {returnValue}"));
            return returnValue;
        }
    }
}
