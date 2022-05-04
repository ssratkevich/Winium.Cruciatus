using System;
using System.Runtime.Serialization;

namespace Winium.Cruciatus.Exceptions
{
    /// <summary>
    /// Element not found exception.
    /// </summary>
    [Serializable]
    public class NoSuchElementException : CruciatusException
    {
        #region Constructors and Destructors

        /// <summary>
        /// Creates new exception.
        /// </summary>
        public NoSuchElementException()
        {
        }

        /// <summary>
        /// Creates new exception with given message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public NoSuchElementException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates new exception with given message and inner exception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public NoSuchElementException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates new exception with given context.
        /// </summary>
        protected NoSuchElementException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}
