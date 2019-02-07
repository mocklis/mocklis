// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockMissingException.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Properties;

    #endregion

#if NETSTANDARD2_0
    using System.Runtime.Serialization;

    [Serializable]
#endif
    public class MockMissingException : Exception
    {
        public MockType MemberType { get; }
        public string MocklisClassName { get; }
        public string InterfaceName { get; }
        public string MemberName { get; }
        public string MemberMockName { get; }

        private static string CreateMessage(MockType memberType, string mocklisClassName, string interfaceName, string memberName,
            string memberMockName)
        {
            string rawMessage;
            switch (memberType)
            {
                case MockType.Method:
                    rawMessage = Resources.MockMissingExceptionMessageForMethod;
                    break;
                case MockType.PropertyGet:
                    rawMessage = Resources.MockMissingExceptionMessageForPropertyGet;
                    break;
                case MockType.PropertySet:
                    rawMessage = Resources.MockMissingExceptionMessageForPropertySet;
                    break;
                case MockType.EventAdd:
                    rawMessage = Resources.MockMissingExceptionMessageForEventAdd;
                    break;
                case MockType.EventRemove:
                    rawMessage = Resources.MockMissingExceptionMessageForEventRemove;
                    break;
                case MockType.IndexerGet:
                    rawMessage = Resources.MockMissingExceptionMessageForIndexerGet;
                    break;
                case MockType.IndexerSet:
                    rawMessage = Resources.MockMissingExceptionMessageForIndexerGet;
                    break;
                case MockType.VirtualMethod:
                    rawMessage = Resources.MockMissingExceptionMessageForVirtualMethod;
                    break;
                case MockType.VirtualPropertyGet:
                    rawMessage = Resources.MockMissingExceptionMessageForVirtualPropertyGet;
                    break;
                case MockType.VirtualPropertySet:
                    rawMessage = Resources.MockMissingExceptionMessageForVirtualPropertySet;
                    break;
                case MockType.VirtualIndexerGet:
                    rawMessage = Resources.MockMissingExceptionMessageForVirtualIndexerGet;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(memberType));
            }

            return string.Format(
                rawMessage,
                mocklisClassName ?? throw new ArgumentNullException(nameof(mocklisClassName)),
                interfaceName ?? throw new ArgumentNullException(nameof(interfaceName)),
                memberName ?? throw new ArgumentNullException(nameof(memberName)),
                memberMockName ?? throw new ArgumentNullException(nameof(memberMockName)));
        }

        public MockMissingException(MockType memberType, IMockInfo memberMock) : this(
            memberType,
            (memberMock ?? throw new ArgumentNullException(nameof(memberMock))).MocklisClassName,
            memberMock.InterfaceName,
            memberMock.MemberName,
            memberMock.MemberMockName)
        {
        }

        public MockMissingException(MockType memberType, string mocklisClassName, string interfaceName, string memberName, string memberMockName)
            : base(CreateMessage(memberType, mocklisClassName, interfaceName, memberName, memberMockName))
        {
            MemberType = memberType;
            MocklisClassName = mocklisClassName;
            InterfaceName = interfaceName;
            MemberName = memberName;
            MemberMockName = memberMockName;
        }

        public MockMissingException(MockType memberType, IMockInfo memberMock, Exception innerException) : this(
            memberType,
            (memberMock ?? throw new ArgumentNullException(nameof(memberMock))).MocklisClassName,
            memberMock.InterfaceName,
            memberMock.MemberName,
            memberMock.MemberMockName,
            innerException)
        {
        }

        public MockMissingException(MockType memberType, string mocklisClassName, string interfaceName, string memberName, string memberMockName,
            Exception innerException)
            : base(CreateMessage(memberType, mocklisClassName, interfaceName, memberName, memberMockName), innerException)
        {
            MemberType = memberType;
            MocklisClassName = mocklisClassName;
            InterfaceName = interfaceName;
            MemberName = memberName;
            MemberMockName = memberMockName;
        }

#if NETSTANDARD2_0
        protected MockMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            MemberType = (MockType)info.GetInt32(nameof(MemberType));
            MocklisClassName = info.GetString(nameof(MocklisClassName));
            InterfaceName = info.GetString(nameof(InterfaceName));
            MemberName = info.GetString(nameof(MemberName));
            MemberMockName = info.GetString(nameof(MemberMockName));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(MemberType), (int)MemberType);
            info.AddValue(nameof(MocklisClassName), MocklisClassName);
            info.AddValue(nameof(InterfaceName), InterfaceName);
            info.AddValue(nameof(MemberName), MemberName);
            info.AddValue(nameof(MemberMockName), MemberMockName);
        }
#endif
    }
}
