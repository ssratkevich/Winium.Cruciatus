using System.Threading;
using NLog;
using WindowsInput;

namespace Winium.Cruciatus.Core
{
    /// <summary>
    /// WindowsInput.KeyboardSimulator keyboard simulator.
    /// </summary>
    public class KeyboardSimulatorExt : IKeyboard
    {
        #region Fields

        private readonly IKeyboardSimulator keyboardSimulator;

        private readonly Logger logger;

        #endregion

        #region Constructors and Destructors

        internal KeyboardSimulatorExt(IKeyboardSimulator keyboardSimulator, Logger logger)
        {
            this.keyboardSimulator = keyboardSimulator;
            this.logger = logger;
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc/>
        public IKeyboard KeyDown(VirtualKeyCode keyCode)
        {
            this.logger.Info("Key down '{0}'", keyCode.ToString());
            this.keyboardSimulator.KeyDown(keyCode);
            Thread.Sleep(250);
            return this;
        }

        /// <inheritdoc/>
        public IKeyboard KeyPress(VirtualKeyCode keyCode)
        {
            this.logger.Info("Key press '{0}'", keyCode.ToString());
            this.keyboardSimulator.KeyPress(keyCode);
            Thread.Sleep(250);
            return this;
        }

        /// <inheritdoc/>
        public void KeyPressSimultaneous(VirtualKeyCode keyCode1, VirtualKeyCode keyCode2)
        {
            this.logger.Info("Press key combo '{0} + {1}'", keyCode1, keyCode2);
            this.keyboardSimulator.ModifiedKeyStroke(keyCode1, keyCode2);
            Thread.Sleep(250);
        }

        /// <summary>
        /// Emulates simultaneous three keys press command.
        /// </summary>
        /// <param name="keyCode1">
        /// First key code.
        /// </param>
        /// <param name="keyCode2">
        /// Second key code.
        /// </param>
        /// <param name="keyCode3">
        /// Third key code.
        /// </param>
        public void KeyPressSimultaneous(VirtualKeyCode keyCode1, VirtualKeyCode keyCode2, VirtualKeyCode keyCode3)
        {
            this.logger.Info("Press key combo '{0} + {1} + {2}'", keyCode1, keyCode2, keyCode3);
            this.keyboardSimulator.ModifiedKeyStroke(new[] { keyCode1, keyCode2 }, keyCode3);
            Thread.Sleep(250);
        }

        /// <inheritdoc/>
        public IKeyboard KeyUp(VirtualKeyCode keyCode)
        {
            this.logger.Info("Key up '{0}'", keyCode.ToString());
            this.keyboardSimulator.KeyUp(keyCode);
            Thread.Sleep(250);
            return this;
        }

        /// <inheritdoc/>
        public IKeyboard SendBackspace()
        {
            this.KeyPress(VirtualKeyCode.BACK);
            return this;
        }

        /// <inheritdoc/>
        public IKeyboard SendCtrlA()
        {
            this.KeyPressSimultaneous(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_A);
            return this;
        }

        /// <inheritdoc/>
        public IKeyboard SendCtrlC()
        {
            this.KeyPressSimultaneous(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
            return this;
        }

        /// <inheritdoc/>
        public IKeyboard SendCtrlV()
        {
            this.KeyPressSimultaneous(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
            return this;
        }

        /// <inheritdoc/>
        public IKeyboard SendEnter()
        {
            this.KeyPress(VirtualKeyCode.RETURN);
            return this;
        }

        /// <inheritdoc/>
        public IKeyboard SendEscape()
        {
            this.KeyPress(VirtualKeyCode.ESCAPE);
            return this;
        }

        /// <inheritdoc/>
        public IKeyboard SendText(string text)
        {
            this.logger.Info("Send text '{0}'", text);
            if (!string.IsNullOrEmpty(text))
            {
                this.keyboardSimulator.TextEntry(text);
                Thread.Sleep(250);
            }

            return this;
        }

        #endregion
    }
}
