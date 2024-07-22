using Godot;
using System;

public partial class Battle : Node3D
{
    private Button? _btnTeam1RPA1;

    public override void _Ready()
    {
        _btnTeam1RPA1 = GetNode<Button>("Control/Canvas/VBoxTeam1/BtnTeam1RPA1");
        _btnTeam1RPA1.Pressed += () => { GD.Print("_btnTeam1RPA1"); };
    }

    public override void _Process(double delta)
    {
    }
}