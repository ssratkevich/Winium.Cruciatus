using System;
using System.Threading;
using System.Windows.Forms;
using NLog;
using WindowsInput;

namespace Winium.Cruciatus.Core
{
    /// <summary>
    /// SendKeys keyboard simulator.
    /// </summary>
    public class SendKeysExt : IKeyboard
    {
        #region Constants

        /// <summary>
        /// Alt key.
        /// </summary>
        public const char Alt = '%';

        /// <summary>
        /// Backspace key.
        /// </summary>
        public const string Backspace = "{BACKSPACE}";

        /// <summary>
        /// Ctrl key.
        /// </summary>
        public const char Ctrl = '^';

        /// <summary>
        /// Enter key.
        /// </summary>
        public const string Enter = "{ENTER}";

        /// <summary>
        /// Escape key.
        /// </summary>
        public const string Escape = "{ESCAPE}";

        /// <summary>
        /// + key.
        /// </summary>
        public const char Shift = '+';

        #endregion

        #region Fields

        private readonly Logger logger;

        #endregion

        #region Constructors and Destructors

        internal SendKeysExt(Logger logger)
        {
            this.logger = logger;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc/>
        public IKeyboard KeyDown(VirtualKeyCode keyCode) =>
            throw new NotImplementedException();

        /// <inheritdoc/>
        public IKeyboard KeyUp(VirtualKeyCode keyCode) =>
            throw new NotImplementedException();

        /// <inheritdoc/>
        public IKeyboard KeyPress(VirtualKeyCode keyCode) =>
            throw new NotImplementedException();

        /// <inheritdoc/>
        public IKeyboard SendBackspace() =>
            this.SendKeysPrivate(Backspace);

        /// <inheritdoc/>
        public IKeyboard SendCtrlA() =>
            this.SendKeysPrivate(Ctrl + "a");

        /// <inheritdoc/>
        public IKeyboard SendCtrlC() =>
            this.SendKeysPrivate(Ctrl + "c");

        /// <inheritdoc/>
        public IKeyboard SendCtrlV() =>
            this.SendKeysPrivate(Ctrl + "v");

        /// <inheritdoc/>
        public IKeyboard SendEnter() =>
            this.SendKeysPrivate(Enter);

        /// <inheritdoc/>
        public IKeyboard SendEscape() =>
            this.SendKeysPrivate(Escape);

        /// <inheritdoc/>
        public IKeyboard SendText(string text)
        {
            this.logger.Info("Send text '{0}'", text);
            return this.SendWaitPrivate(text);
        }

        #endregion

        #region Methods

        private IKeyboard SendKeysPrivate(string keys)
        {
            this.logger.Info("Send keys '{0}'", keys);
            return this.SendWaitPrivate(keys);
        }

        private IKeyboard SendWaitPrivate(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                SendKeys.SendWait(text);
                Thread.Sleep(250);
            }

            return this;
        }

        #endregion
    }
}
