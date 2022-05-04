using WindowsInput;

namespace Winium.Cruciatus.Core
{
    /// <summary>
    /// Keyboard simulator interface.
    /// </summary>
    public interface IKeyboard
    {
        /// <summary>
        /// Emulates pressing and holding button.
        /// </summary>
        /// <param name="keyCode">
        /// Button code.
        /// </param>
        /// <returns>
        /// Keyboard for chaining actions.
        /// </returns>
        IKeyboard KeyDown(VirtualKeyCode keyCode);

        /// <summary>
        /// Emulates button release.
        /// </summary>
        /// <param name="keyCode">
        /// Button code.
        /// </param>
        /// <returns>
        /// Keyboard for chaining actions.
        /// </returns>
        IKeyboard KeyUp(VirtualKeyCode keyCode);

        /// <summary>
         /// Emulates press and release button action.
        /// </summary>
        /// <param name="keyCode">
        /// Button code.
        /// </param>
        /// <returns>
        /// Keyboard for chaining actions.
        /// </returns>
        IKeyboard KeyPress(VirtualKeyCode keyCode);

        /// <summary>
        /// Emulates press and release Backspace button.
        /// </summary>
        /// <returns>
        /// Keyboard for chaining actions.
        /// </returns>
        IKeyboard SendBackspace();

        /// <summary>
        /// Emulates Ctrl + A command.
        /// </summary>
        /// <returns>
        /// Keyboard for chaining actions.
        /// </returns> 
        IKeyboard SendCtrlA();

        /// <summary>
        /// Emulates Ctrl + C command.
        /// </summary>
        /// <returns>
        /// Keyboard for chaining actions.
        /// </returns> 
        IKeyboard SendCtrlC();

        /// <summary>
        /// Emulates Ctrl + V command.
        /// </summary>
        /// <returns>
        /// Keyboard for chaining actions.
        /// </returns> 
        IKeyboard SendCtrlV();

        /// <summary>
        /// Emulates press and release Enter button.
        /// </summary>
        IKeyboard SendEnter();

        /// <summary>
        /// Emulates press and release Escape button.
        /// </summary>
        /// <returns>
        /// Keyboard for chaining actions.
        /// </returns> 
        IKeyboard SendEscape();

        /// <summary>
        /// Emulates text entering.
        /// </summary>
        /// <param name="text">
        /// Text to enter.
        /// </param>
        /// <returns>
        /// Keyboard for chaining actions.
        /// </returns>
        IKeyboard SendText(string text);
    }
}
