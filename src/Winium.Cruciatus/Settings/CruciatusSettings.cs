using Winium.Cruciatus.Core;
using Winium.Cruciatus.Settings.MessageBoxSettings;

namespace Winium.Cruciatus.Settings
{
    /// <summary>
    /// Cruciatus settings class.
    /// </summary>
    public class CruciatusSettings
    {
        #region Constants

        private const MouseButton DefaultClickButton = MouseButton.Left;

        private const int DefaultScrollBarHeight = 18;

        private const int DefaultScrollBarWidth = 18;

        private const int DefaultSearchTimeout = 10000;

        private const int DefaultWaitForExitTimeout = 10000;

        #endregion

        #region Static Fields

        private static readonly MessageBoxButtonUid DefaultMessageBoxButtonUid = new MessageBoxButtonUid();

        private static readonly OpenFileDialogUid DefaultOpenFileDialogUid = new OpenFileDialogUid();

        private static readonly SaveFileDialogUid DefaultSaveFileDialogUid = new SaveFileDialogUid();

        private static CruciatusSettings instance;

        #endregion

        #region Constructors and Destructors

        private CruciatusSettings()
        {
            DefaultMessageBoxButtonUid.CloseButton = "Close";
            DefaultMessageBoxButtonUid.OkType = new OkType { Ok = "2" };
            DefaultMessageBoxButtonUid.OkCancelType = new OkCancelType { Ok = "1", Cancel = "2" };
            DefaultMessageBoxButtonUid.YesNoType = new YesNoType { Yes = "6", No = "7" };
            DefaultMessageBoxButtonUid.YesNoCancelType = new YesNoCancelType { Yes = "6", No = "7", Cancel = "2" };

            DefaultOpenFileDialogUid.OpenButton = "1";
            DefaultOpenFileDialogUid.CancelButton = "2";
            DefaultOpenFileDialogUid.FileNameEditableComboBox = "1148";

            DefaultSaveFileDialogUid.SaveButton = "1";
            DefaultSaveFileDialogUid.CancelButton = "2";
            DefaultSaveFileDialogUid.FileNameEditableComboBox = "FileNameControlHost";
            DefaultSaveFileDialogUid.FileTypeComboBox = "FileTypeControlHost";

            this.ResetToDefault();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Automatic srceenshot capture (default is false).
        /// </summary>
        public bool AutomaticScreenshotCapture { get; set; }

        /// <summary>
        /// Default mouse click button.
        /// </summary>
        public MouseButton ClickButton { get; set; }

        /// <summary>
        /// Keyboard simulator type.
        /// </summary>
        public KeyboardSimulatorType KeyboardSimulatorType { get; set; }

        /// <summary>
        /// MessageBox buttons ids.
        /// </summary>
        public MessageBoxButtonUid MessageBoxButtonUid { get; set; }

        /// <summary>
        /// OpenFileDialog elements ids.
        /// </summary>
        public OpenFileDialogUid OpenFileDialogUid { get; set; }

        /// <summary>
        /// SaveFileDialog elements ids.
        /// </summary>
        public SaveFileDialogUid SaveFileDialogUid { get; set; }

        /// <summary>
        /// Srceenshots save directory (default is './Screenshots').
        /// </summary>
        public string ScreenshotsPath { get; set; }

        /// <summary>
        /// ScrollBar height.
        /// </summary>
        public int ScrollBarHeight { get; set; }

        /// <summary>
        /// ScrollBar width.
        /// </summary>
        public int ScrollBarWidth { get; set; }

        /// <summary>
        /// Element search time threshold (milliseconds).
        /// </summary>
        public int SearchTimeout { get; set; }

        /// <summary>
        /// Application exit time threshold (milliseconds).
        /// </summary>
        public int WaitForExitTimeout { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Globally available settings instance.
        /// </summary>
        internal static CruciatusSettings Instance => 
            instance ??= new CruciatusSettings();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Сбрасывает значения настроек на исходные.
        /// </summary>
        public void ResetToDefault()
        {
            this.SearchTimeout = DefaultSearchTimeout;
            this.WaitForExitTimeout = DefaultWaitForExitTimeout;
            this.ScrollBarWidth = DefaultScrollBarWidth;
            this.ScrollBarHeight = DefaultScrollBarHeight;
            this.ClickButton = DefaultClickButton;
            this.KeyboardSimulatorType = KeyboardSimulatorType.BasedOnWindowsFormsSendKeysClass;
            this.ScreenshotsPath = "Screenshots";
            this.AutomaticScreenshotCapture = false;

            this.MessageBoxButtonUid = (MessageBoxButtonUid)DefaultMessageBoxButtonUid.Clone();
            this.OpenFileDialogUid = (OpenFileDialogUid)DefaultOpenFileDialogUid.Clone();
            this.SaveFileDialogUid = (SaveFileDialogUid)DefaultSaveFileDialogUid.Clone();
        }

        #endregion
    }
}
