// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockMissingException_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#if !NETCOREAPP1_1

namespace Mocklis.Core.Tests.Core
{
    #region Using Directives

    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Xunit;

    #endregion

    public class MockMissingException_should
    {
        private readonly IMockInfo _mockInfo =
            new PropertyMock<int>(new object(), "MocklisClassName", "InterfaceName", "MemberName", "MemberMockName", Strictness.Lenient);

        private static T RoundTrip<T>(T item)
        {
            var formatter = new BinaryFormatter();

            using (var m = new MemoryStream())
            {
                formatter.Serialize(m, item);
                m.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(m);
            }
        }

        [Fact]
        public void be_serializable()
        {
            var exception = new MockMissingException(MockType.PropertyGet, _mockInfo);
            var roundtrippedException = RoundTrip(exception);

            Assert.NotSame(exception, roundtrippedException);

            Assert.Equal(exception.MemberType, roundtrippedException.MemberType);
            Assert.Equal(exception.MocklisClassName, roundtrippedException.MocklisClassName);
            Assert.Equal(exception.InterfaceName, roundtrippedException.InterfaceName);
            Assert.Equal(exception.MemberName, roundtrippedException.MemberName);
            Assert.Equal(exception.MemberMockName, roundtrippedException.MemberMockName);
            Assert.Equal(exception.Message, roundtrippedException.Message);
        }
    }
}

#endif
