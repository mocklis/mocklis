// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockMissingException.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Properties;

    #endregion

#if NETSTANDARD2_0
    using System.Runtime.Serialization;

    [Serializable]
#endif
    public class MockMissingException : Exception
    {
        public MockType MemberType { get; }
        public string InterfaceName { get; }
        public string MemberName { get; }
        public string MockMemberName { get; }

        private static string CreateMessage(MockType memberType, string interfaceName, string memberName, string mockMemberName)
        {
            switch (memberType)
            {
                case MockType.Method:
                    return string.Format(Resources.MockMissingExceptionMessageForMethod, interfaceName, memberName, mockMemberName);
                case MockType.PropertyGet:
                    return string.Format(Resources.MockMissingExceptionMessageForPropertyGet, interfaceName, memberName, mockMemberName);
                case MockType.PropertySet:
                    return string.Format(Resources.MockMissingExceptionMessageForPropertySet, interfaceName, memberName, mockMemberName);
                case MockType.EventAdd:
                    return string.Format(Resources.MockMissingExceptionMessageForEventAdd, interfaceName, memberName, mockMemberName);
                case MockType.EventRemove:
                    return string.Format(Resources.MockMissingExceptionMessageForEventRemove, interfaceName, memberName, mockMemberName);
                case MockType.IndexerGet:
                    return string.Format(Resources.MockMissingExceptionMessageForIndexerGet, interfaceName, memberName, mockMemberName);
                case MockType.IndexerSet:
                    return string.Format(Resources.MockMissingExceptionMessageForIndexerGet, interfaceName, memberName, mockMemberName);
                default:
                    throw new ArgumentOutOfRangeException(nameof(memberType));
            }
        }

        public MockMissingException(MockType memberType, string interfaceName, string memberName, string mockMemberName) : base(
            CreateMessage(memberType, interfaceName, memberName, mockMemberName))
        {
            MemberType = memberType;
            InterfaceName = interfaceName;
            MemberName = memberName;
            MockMemberName = mockMemberName;
        }

        public MockMissingException(MockType memberType, string interfaceName, string memberName, string mockMemberName,
            Exception innerException) : base(CreateMessage(memberType, interfaceName, memberName, mockMemberName), innerException)
        {
            MemberType = memberType;
            InterfaceName = interfaceName;
            MemberName = memberName;
            MockMemberName = mockMemberName;
        }

#if NETSTANDARD2_0
        protected MockMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            MemberType = (MockType)info.GetInt32(nameof(MemberType));
            InterfaceName = info.GetString(nameof(InterfaceName));
            MemberName = info.GetString(nameof(MemberName));
            MockMemberName = info.GetString(nameof(MockMemberName));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(MemberType), (int)MemberType);
            info.AddValue(nameof(InterfaceName), InterfaceName);
            info.AddValue(nameof(MemberName), MemberName);
            info.AddValue(nameof(MockMemberName), MockMemberName);
        }
#endif
    }
}
