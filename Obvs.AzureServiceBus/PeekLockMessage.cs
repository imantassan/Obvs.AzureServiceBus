﻿using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Obvs.AzureServiceBus.Infrastructure;
using Obvs.Types;

namespace Obvs.AzureServiceBus
{
    [DataContract]
    [Serializable]
    public abstract class PeekLockMessage : IMessage
    {
        [NonSerialized]
        private IMessagePeekLockControl _messagePeekLockControl;

        [XmlIgnore]
        [IgnoreDataMember]
        internal IMessagePeekLockControl PeekLockControl
        {
            get
            {
                return _messagePeekLockControl;
            }

            set
            {
#if DEBUG

                if(value == null)
                {
                    throw new ArgumentNullException();
                }

#endif

                _messagePeekLockControl = value;
            }
        }
    }

    public static class PeekLockControlMessageExtensions
    {
        public static IMessagePeekLockControl GetPeekLockControl<TMessage>(this TMessage message) where TMessage : class
        {
            if(message == null) throw new ArgumentNullException("message");

            PeekLockMessage peekLockMessage = message as PeekLockMessage;

            if(peekLockMessage == null)
            {
                throw new InvalidOperationException("The message is not valid for peek lock control. You must inherit from the PeekLockMessage class to opt-in to peek lock semantics.");
            }

            return peekLockMessage.PeekLockControl;
        }
    }
}
