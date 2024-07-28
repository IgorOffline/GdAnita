using Godot;
using AnitaBusiness.BusinessMain.BusinessLogging;

public class AnitaLogger : ILogger
{
	public LogLevel LogLevel { get; set; }
        
	public void Print(string s)
	{
		GD.Print(s);
	}
}
