using System;
using System.Runtime.Serialization;

namespace Winium.Cruciatus.Exceptions
{
    /// <summary>
    /// Cruciatus exception base class.
    /// </summary>
    [Serializable]
    public class CruciatusException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Creates new exception.
        /// </summary>
        public CruciatusException()
        {
        }

        /// <summary>
        /// Creates new exception with given message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public CruciatusException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates new exception with given message and inner exception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public CruciatusException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates new exception with given context.
        /// </summary>
        protected CruciatusException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}
