using System.Diagnostics;
using System.Threading;
using Lax.Data.SharePoint.Lists.CodeAnnotations;

namespace Lax.Data.SharePoint.Lists.Diagnostics {

    /// <summary>
    /// Represents class of logging endpoint that writes messages to <see cref="Debugger"/> instance.
    /// </summary>
    [PublicAPI]
    public class DebuggerLoggingEndpoint : ILoggingEndpoint {

        /// <summary>
        /// Logs message with specified level and category.
        /// </summary>
        /// <param name="level">Logging level of the message.</param>
        /// <param name="category">Category of the message.</param>
        /// <param name="message">Message to print.</param>
        public void Log(LogLevel level, string category, string message) {
            if (!Debugger.IsAttached) {
                return;
            }

            var logMessage =
                $"[Thread: {Thread.CurrentThread.ManagedThreadId}, Level: {level}, Category: {category}]\n{message}\n\n";

            Debugger.Log(0, category, logMessage);
        }

    }

}