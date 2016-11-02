namespace MVC.Core.Exceptions
{
	using System;
    using System.Runtime.CompilerServices;

    public interface IExceptionHandler
	{
        void HandleException(Exception ex, string Message = null, [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0);
    }
}
