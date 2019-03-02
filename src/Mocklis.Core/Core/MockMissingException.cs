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
#if NETSTANDARD2_0
    using System.Runtime.Serialization;
#endif

    #endregion

    /// <summary>
    ///     Exception class that indicates that a mocked member was accessed but didn't have (enough) behaviour configured to
    ///     handle the access properly.
    ///     Inherits from the <see cref="Exception" /> class.
    /// </summary>
    /// <seealso cref="Exception" />
#if NETSTANDARD2_0
    [Serializable]
#endif
    public class MockMissingException : Exception
    {
        /// <summary>
        ///     Gets the 'mock type' of the mocked member.
        /// </summary>
        /// <value>The 'mock type' of the mocked member.</value>
        public MockType MemberType { get; }

        /// <summary>
        ///     Gets the name of the mocklis class.
        /// </summary>
        /// <value>The name of the mocklis class.</value>
        public string MocklisClassName { get; }

        /// <summary>
        ///     Gets the name of the interface on which the mocked member is defined.
        /// </summary>
        /// <value>The name of the interface on which the mocked member is defined.</value>
        public string InterfaceName { get; }

        /// <summary>
        ///     Gets the name of the mocked member.
        /// </summary>
        /// <value>The name of the mocked member.</value>
        public string MemberName { get; }

        /// <summary>
        ///     Gets the name of the property or method used to provide the mocked member with behaviour.
        /// </summary>
        /// <value>The name of the property or method used to provide the mocked member with behaviour.</value>
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
                    rawMessage = Resources.MockMissingExceptionMessageForIndexerSet;
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
                case MockType.VirtualIndexerSet:
                    rawMessage = Resources.MockMissingExceptionMessageForVirtualIndexerSet;
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

        /// <summary>
        ///     Initializes a new instance of the <see cref="MockMissingException" /> class with mock information.
        /// </summary>
        /// <param name="memberType">The 'mock type' of the mocked member.</param>
        /// <param name="memberMock">Information about the mocked member.</param>
        public MockMissingException(MockType memberType, IMockInfo memberMock) : this(
            memberType,
            (memberMock ?? throw new ArgumentNullException(nameof(memberMock))).MocklisClassName,
            memberMock.InterfaceName,
            memberMock.MemberName,
            memberMock.MemberMockName)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MockMissingException" /> class with explicit mock information.
        /// </summary>
        /// <param name="memberType">The 'mock type' of the mocked member.</param>
        /// <param name="mocklisClassName">The name of the mocklis class.</param>
        /// <param name="interfaceName">The name of the interface on which the mocked member is defined.</param>
        /// <param name="memberName">The name of the mocked member.</param>
        /// <param name="memberMockName">The name of the property or method used to provide the mocked member with behaviour.</param>
        public MockMissingException(MockType memberType, string mocklisClassName, string interfaceName, string memberName, string memberMockName)
            : base(CreateMessage(memberType, mocklisClassName, interfaceName, memberName, memberMockName))
        {
            MemberType = memberType;
            MocklisClassName = mocklisClassName;
            InterfaceName = interfaceName;
            MemberName = memberName;
            MemberMockName = memberMockName;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MockMissingException" /> class with mock information and a reference
        ///     to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="memberType">The 'mock type' of the mocked member.</param>
        /// <param name="memberMock">Information about the mocked member.</param>
        /// <param name="innerException">The inner exception.</param>
        public MockMissingException(MockType memberType, IMockInfo memberMock, Exception innerException) : this(
            memberType,
            (memberMock ?? throw new ArgumentNullException(nameof(memberMock))).MocklisClassName,
            memberMock.InterfaceName,
            memberMock.MemberName,
            memberMock.MemberMockName,
            innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MockMissingException" /> class with explicit mock information and a
        ///     reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="memberType">The 'mock type' of the mocked member.</param>
        /// <param name="mocklisClassName">The name of the mocklis class.</param>
        /// <param name="interfaceName">The name of the interface on which the mocked member is defined.</param>
        /// <param name="memberName">The name of the mocked member.</param>
        /// <param name="memberMockName">The name of the property or method used to provide the mocked member with behaviour.</param>
        /// <param name="innerException">The inner exception.</param>
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
        /// <summary>
        ///     Initializes a new instance of the <see cref="MockMissingException" /> class with serialized data.
        /// </summary>
        /// <param name="info">
        ///     The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being
        ///     thrown.
        /// </param>
        /// <param name="context">
        ///     The <see cref="StreamingContext" /> that contains contextual information about the source or
        ///     destination.
        /// </param>
        protected MockMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            MemberType = (MockType)info.GetInt32(nameof(MemberType));
            MocklisClassName = info.GetString(nameof(MocklisClassName));
            InterfaceName = info.GetString(nameof(InterfaceName));
            MemberName = info.GetString(nameof(MemberName));
            MemberMockName = info.GetString(nameof(MemberMockName));
        }

        /// <summary>
        ///     Sets the <see cref="SerializationInfo" /> with information abouet the exception.
        /// </summary>
        /// <param name="info">
        ///     The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being
        ///     thrown.
        /// </param>
        /// <param name="context">
        ///     The <see cref="StreamingContext" /> that contains contextual information about the source or
        ///     destination.
        /// </param>
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
