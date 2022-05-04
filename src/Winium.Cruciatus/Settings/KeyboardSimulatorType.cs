namespace Winium.Cruciatus.Settings
{
    /// <summary>
    /// Supported keyboard simulators.
    /// </summary>
    public enum KeyboardSimulatorType
    {
        /// <summary>
        /// SendKeys simulator (see https://msdn.microsoft.com/ru-ru/library/system.windows.forms.sendkeys(v=vs.110).aspx).
        /// Not recommended to use in complex scenarios.
        /// </summary>
        BasedOnWindowsFormsSendKeysClass,

        /// <summary>
        /// Input simulator (see https://github.com/HavenDV/H.InputSimulator).
        /// </summary>
        BasedOnInputSimulatorLib
    }
}
