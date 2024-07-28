using Godot;
using AnitaBusiness.BusinessMain;
using AnitaBusiness.BusinessMain.BusinessLogging;

public partial class Global : Node
{
	public GameMaster? GameMaster { get; private set; }
	
	public override void _Ready()
	{
		GD.Print("Global");
		var logger = new AnitaLogger();
		logger.LogLevel = LogLevel.Trace;
		GameMaster = new GameMaster(logger);
	}
	
	public override void _Process(double delta)
	{
	}
}
