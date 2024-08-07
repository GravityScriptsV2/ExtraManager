using ExtraManager.Handlers;
using Rage;
using System;

namespace ExtraManager.Utils
{
	public enum LoggingLevel
	{
		DEBUG,
		INFO,
		WARNING,
		ERROR
	}
	internal class Logging
    {
		#region Log Methods

		public static void Debug(string message, string caller, Exception ex = null)
		{
			Log(LoggingLevel.DEBUG, message, caller, ex);
		}

		public static void Error(string message, string caller, Exception ex = null)
		{
			Log(LoggingLevel.ERROR, message, caller, ex);
		}

		public static void Info(string message, string caller, Exception ex = null)
		{
			Log(LoggingLevel.INFO, message, caller, ex);
		}

		public static void Warning(string message, string caller, Exception ex = null)
		{
			Log(LoggingLevel.WARNING, message, caller, ex);
		}

		#endregion

		#region Internal Methods

		public static void Log(LoggingLevel level, string message, string caller, Exception ex = null)
		{
			if ((int)level >= ConfigHandler.LogLevel)
			{
				Game.LogTrivial("[" + caller + "] " + message);
				if (ex != null)
				{
					Error(ex.Message, caller);
					Debug(ex.StackTrace, caller);
				}
			}
		}

        #endregion
    }
}
