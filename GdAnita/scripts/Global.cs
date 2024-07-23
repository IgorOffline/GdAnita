using Godot;
using AnitaBusiness.BusinessMain;

public partial class Global : Node
{
	public GameMaster GameMaster { get; private set; }
	
	public override void _Ready()
	{
		GD.Print("Global");
		GameMaster = new GameMaster();
	}
	
	public override void _Process(double delta)
	{
	}
}
