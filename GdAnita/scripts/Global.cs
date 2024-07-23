using Godot;
using System;

public partial class Global : Node
{
	public int Counter { get; set; }
	
	public override void _Ready()
	{
		GD.Print("Global");
	}
	
	public override void _Process(double delta)
	{
	}
}
