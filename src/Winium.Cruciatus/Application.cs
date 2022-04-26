using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
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
        /// Return process id of application.
        /// </summary>
        /// <returns>Process id</returns>
        public int GetProcessId()
        {
            return this.process.Id;
        }

        /// <summary>
        /// Get exit state of launched application
        /// </summary>
        /// <returns>
        /// true if it's already exit, false if it's still running
        /// </returns>
        public bool HasExited()
        {
            return this.process.HasExited;
        }

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
        /// Close child process
        /// </summary>
        /// <param name="child">Input child process to close</param>
        /// <returns>
        /// true if successfully close, otherwise return fail.
        /// </returns>
        public bool Close(Process child)
        {
            child.CloseMainWindow();
            return child.WaitForExit(CruciatusFactory.Settings.WaitForExitTimeout);
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

        /// <summary>
        /// Kill child process
        /// </summary>
        /// <param name="child">Input child process to kill</param>
        /// <returns>
        /// true if successfully kill, otherwise return false
        /// </returns>
        public bool Kill(Process child)
        {
            child.Kill();
            return child.WaitForExit(CruciatusFactory.Settings.WaitForExitTimeout);
        }

        /// <summary>
        /// Get all children processes of parent one bases on its id.
        /// </summary>
        /// <param name="parentId">Input parent process id.</param>
        /// <returns>List of child processes.</returns>
        public List<Process> GetChildPrecesses(int parentId)
        {
            var query = "Select * From Win32_Process Where ParentProcessId = "
                    + parentId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();
            List<Process> result = new List<Process>();
            foreach (ManagementObject managedObject in processList)
            {
                result.Add(Process.GetProcessById(Convert.ToInt32(managedObject["ProcessID"])));
            }
            return result;
        }

        #endregion
    }
}
