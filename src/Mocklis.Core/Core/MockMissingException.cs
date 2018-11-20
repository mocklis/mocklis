// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockMissingException.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
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

        private static string CreateMessage(MockType memberType, MemberMock memberMock)
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(memberType));
            }

            return string.Format(rawMessage, memberMock.MocklisClassName, memberMock.InterfaceName,
                memberMock.MemberName, memberMock.MemberMockName);
        }

        public MockMissingException(MockType memberType, MemberMock memberMock)
            : base(CreateMessage(memberType, memberMock))
        {
            MemberType = memberType;
            MocklisClassName = memberMock.MocklisClassName;
            InterfaceName = memberMock.InterfaceName;
            MemberName = memberMock.MemberName;
            MemberMockName = memberMock.MemberMockName;
        }

        public MockMissingException(MockType memberType, MemberMock memberMock, Exception innerException)
            : base(CreateMessage(memberType, memberMock), innerException)
        {
            MemberType = memberType;
            MocklisClassName = memberMock.MocklisClassName;
            InterfaceName = memberMock.InterfaceName;
            MemberName = memberMock.MemberName;
            MemberMockName = memberMock.MemberMockName;
        }

#if NETSTANDARD2_0
        protected MockMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            MemberType = (MockType)info.GetInt32(nameof(MemberType));
            MocklisClassName = info.GetString(nameof(MemberMockName));
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
