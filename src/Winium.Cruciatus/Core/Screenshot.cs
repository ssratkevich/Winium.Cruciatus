using System;
using System.IO;

namespace Winium.Cruciatus.Core
{
    /// <summary>
    /// Screenshot.
    /// </summary>
    public class Screenshot
    {
        #region Fields

        private readonly string base64String = string.Empty;

        private readonly byte[] byteArray;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Creates new instance of sreenshot with given byte array.
        /// </summary>
        /// <param name="array">Screenshot image.</param>
        public Screenshot(byte[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("Cannot be null or empty", "array");
            }

            this.byteArray = array;
            this.base64String = Convert.ToBase64String(this.byteArray);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Screenshot as base64 string.
        /// </summary>
        /// <returns>
        /// Screenshot as base64 string.
        /// </returns>
        public string AsBase64String() =>
            this.base64String;

        /// <summary>
        /// Screenshot as byte array.
        /// </summary>
        /// <returns>
        /// Screenshot as byte array.
        /// </returns>
        public byte[] AsByteArray() =>
            this.byteArray;

        /// <summary>
        /// Saves sreenshot to given file.
        /// </summary>
        /// <param name="filePath">
        /// File path.
        /// </param>
        public void SaveAsFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Cannot be null or empty", "filePath");
            }

            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using (var stream = File.OpenWrite(filePath))
            {
                stream.Write(this.byteArray, 0, this.byteArray.Length);
            }
        }

        /// <summary>
        /// Screenshot as base64 string.
        /// </summary>
        /// <returns>
        /// Screenshot as base64 string.
        /// </returns>
        public override string ToString() =>
            this.base64String;

        #endregion
    }
}
