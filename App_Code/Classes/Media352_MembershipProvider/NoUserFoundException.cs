using System;

/// <summary>
/// Exception class to be thrown if a user is not found
/// </summary>
public class NoUserFoundException : ApplicationException
{
	public NoUserFoundException(string message)
		: base(message)
	{
	}
}