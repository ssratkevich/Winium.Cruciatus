extern alias UIAComWrapper;
using NLog;
using NLog.Config;
using NLog.Targets;
using WindowsInput;
using Winium.Cruciatus.Core;
using Winium.Cruciatus.Elements;
using Winium.Cruciatus.Exceptions;
using Winium.Cruciatus.Settings;
using Automation = UIAComWrapper::System.Windows.Automation;

namespace Winium.Cruciatus
{
    /// <summary>
    /// Class for access to Cruciatus infrastructure.
    /// </summary>
    public static class CruciatusFactory
    {
        #region Static Fields

        private static KeyboardSimulatorExt keyboardSimulatorExt;

        private static MouseSimulatorExt mouseSimulatorExt;

        private static Screenshoter screenshoter;

        private static SendKeysExt sendKeysExt;

        #endregion

        #region Constructors and Destructors

        static CruciatusFactory()
        {
            InitAutomation();
            LoggerInit();
            InputSimulatorsInit();
            ScreenshotersInit();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Get the focused element.
        /// </summary>
        public static CruciatusElement FocusedElement =>
            CruciatusElement.Create(Automation::AutomationElement.FocusedElement, null, null);

        /// <summary>
        /// Get the current keyboard simulator.
        /// </summary>
        public static IKeyboard Keyboard =>
            GetSpecificKeyboard(Settings.KeyboardSimulatorType);

        /// <summary>
        /// Get the logger.
        /// </summary>
        public static Logger Logger =>
            LogManager.GetLogger("cruciatus");

        /// <summary>
        /// Get the mouse simulator.
        /// </summary>
        public static MouseSimulatorExt Mouse =>
            mouseSimulatorExt;
            
        /// <summary>
        /// Get the root element for Desktop.
        /// </summary>
        public static CruciatusElement Root =>
            CruciatusElement.Create(Automation::AutomationElement.RootElement, null, null);
            
        /// <summary>
        /// Get the screenshoter.
        /// </summary>
        public static IScreenshoter Screenshoter =>
            screenshoter;
            
        /// <summary>
        /// Get the Crucaitus settings.
        /// </summary>
        public static CruciatusSettings Settings =>
            CruciatusSettings.Instance;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Get keyboard simulator of given type.
        /// </summary>
        /// <param name="keyboardSimulatorType">
        /// Keyboard simulator type.
        /// </param>
        /// <returns>
        /// Keyboard simulator.
        /// </returns>
        public static IKeyboard GetSpecificKeyboard(KeyboardSimulatorType keyboardSimulatorType) =>
            keyboardSimulatorType switch
            {
                KeyboardSimulatorType.BasedOnInputSimulatorLib => keyboardSimulatorExt,
                KeyboardSimulatorType.BasedOnWindowsFormsSendKeysClass => sendKeysExt,
                _ => throw new CruciatusException("Unknown KeyboardSimulatorType")
            };

        #endregion

        #region Methods

        private static void InitAutomation()
        {
            _ = Automation::AutomationElement.RootElement.Current.Name;
        }

        private static void InputSimulatorsInit()
        {
            var inputSimulator = new InputSimulator();
            keyboardSimulatorExt = new KeyboardSimulatorExt(inputSimulator.Keyboard, Logger);
            mouseSimulatorExt = new MouseSimulatorExt(inputSimulator.Mouse);

            sendKeysExt = new SendKeysExt(Logger);
        }

        private static void LoggerInit()
        {
            // Step 0. Not override if there is some configuration
            if (LogManager.Configuration != null)
            {
                return;
            }

            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration 
            var consoleTarget = new ConsoleTarget();
            config.AddTarget("console", consoleTarget);

            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);

            const string Layout =
                @"[${date:format=HH\:mm\:ss}] [${level}] ${message} "
                + "${onexception:${exception:format=tostring,stacktrace}${newline}${stacktrace}}";

            // Step 3. Set target properties 
            consoleTarget.Layout = Layout;
            fileTarget.FileName = "Cruciatus.log";
            fileTarget.Layout = Layout;

            // Step 4. Define rules
            var rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule1);

            var rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);

            // Step 5. Activate the configuration
            LogManager.Configuration = config;
        }

        private static void ScreenshotersInit()
        {
            screenshoter = new Screenshoter();
        }

        #endregion
    }
}
