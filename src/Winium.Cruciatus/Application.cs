using System;
using System.Diagnostics;
using System.IO;
using Winium.Cruciatus.Exceptions;

namespace Winium.Cruciatus
{
    /// <summary>
    /// Wrapper for external application.
    /// </summary>
    public class Application
    {
        #region Fields

        /// <summary>
        /// Full path to executable.
        /// </summary>
        private readonly string executableFilePath;

        /// <summary>
        /// Executable owner process.
        /// </summary>
        private Process process;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Creates instance of an Application.
        /// </summary>
        /// <param name="executableFilePath">
        /// Path to external application (absolute or relative).
        /// </param>
        public Application(string executableFilePath)
        {
            if (executableFilePath == null)
            {
                throw new ArgumentNullException("executableFilePath");
            }

            if (Path.IsPathRooted(executableFilePath))
            {
                this.executableFilePath = executableFilePath;
            }
            else
            {
                var absolutePath = Path.Combine(Environment.CurrentDirectory, executableFilePath);
                this.executableFilePath = Path.GetFullPath((new Uri(absolutePath)).LocalPath);
            }
        }

        #endregion

        #region Public Methods and Operators


        /// <summary>
        /// Starts executable without arguments.
        /// </summary>
        public void Start()
        {
            this.Start(string.Empty);
        }

        /// <summary>
        /// Starts executable with arguments.
        /// </summary>
        /// <param name="arguments">
        /// Arguments string.
        /// </param>
        public void Start(string arguments)
        {
            if (!File.Exists(this.executableFilePath))
            {
                throw new CruciatusException(string.Format(@"Path ""{0}"" doesn't exists", this.executableFilePath));
            }

            var directory = Path.GetDirectoryName(this.executableFilePath);

            // ReSharper disable once AssignNullToNotNullAttribute
            // directory не может быть null, в связи с проверкой выше наличия файла executableFilePath
            var info = new ProcessStartInfo
            {
                FileName = this.executableFilePath,
                WorkingDirectory = directory,
                Arguments = arguments
            };

            this.process = Process.Start(info);
        }


        /// <summary>
        /// Try to close main application window.
        /// </summary>
        /// <returns>
        /// true - successful close, false - otherwise.
        /// </returns>
        public bool Close()
        {
            this.process.CloseMainWindow();
            return this.process.WaitForExit(CruciatusFactory.Settings.WaitForExitTimeout);
        }

        /// <summary>
        /// Kills the application.
        /// </summary>
        /// <returns>
        /// true - successful kill, false - otherwise.
        /// </returns>
        public bool Kill()
        {
            this.process.Kill();
            return this.process.WaitForExit(CruciatusFactory.Settings.WaitForExitTimeout);
        }

        #endregion
    }
}
