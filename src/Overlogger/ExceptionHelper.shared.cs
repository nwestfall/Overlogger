using System;

namespace Overlogger
{
	/// <summary>
	/// Exception helper.
	/// </summary>
    public static class ExceptionHelper
    {
		/// <summary>
		/// Gets the stack trace.
		/// </summary>
		/// <returns>The stack trace.</returns>
		/// <param name="ex">Ex.</param>
        public static string GetStackTrace(this Exception ex)
        {
            string trace = "";
			if (ex != null)
			{
				trace += $"{ex.Message}: ";
				if(!string.IsNullOrEmpty(ex?.StackTrace))
					trace += ex.StackTrace;
				if (ex.InnerException != null)
				{
					trace += "\n";
					trace += ex.InnerException.GetStackTrace();
				}
			}
			return trace;
        }
    }
}