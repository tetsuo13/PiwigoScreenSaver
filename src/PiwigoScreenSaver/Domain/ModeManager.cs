using System;

namespace PiwigoScreenSaver.Domain;

public class ModeManager
{
    /// <summary>
    /// The ways in which Windows runs the screen saver.
    /// </summary>
    public enum Mode
    {
        /// <summary>
        /// Show the configuration settings dialog box.
        /// </summary>
        Configuration,

        /// <summary>
        /// Display a preview of the screen saver.
        /// </summary>
        Preview,

        /// <summary>
        /// Start the screen saver in full-screen mode.
        /// </summary>
        FullScreen
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public Mode GetMode(string[] args)
    {
        if (args == null || args.Length == 0)
        {
            return Mode.Configuration;
        }

        var firstArgument = args[0].ToLower().Trim();
        string secondArgument = null;

        // Handle cases where arguments are separated by colon.
        // Examples: /c:1234567 or /P:1234567
        if (firstArgument.Length > 2)
        {
            secondArgument = firstArgument.Substring(3).Trim();
            firstArgument = firstArgument[..2];
        }
        else if (args.Length > 1)
        {
            secondArgument = args[1];
        }

        if (firstArgument == "/c")
        {
            return Mode.Configuration;
        }
        else if (firstArgument == "/p")
        {
            return Mode.Preview;
        }
        else if (firstArgument == "/s")
        {
            return Mode.FullScreen;
        }

        throw new ArgumentException($"Command line argument '{firstArgument}' is not valid.");
    }
}
