using Godot;
using System;

public partial class Battle : Node3D
{
    private Button? _btnTeam1RPA1;
    private TextureButton? _btnCard1;

    public override void _Ready()
    {
        _btnTeam1RPA1 = GetNode<Button>("Canvas/GridTeam1/VBox1/HBoxRP/BtnRPA1");
        _btnTeam1RPA1.Pressed += () => { GD.Print("_btnTeam1RPA1"); };
        _btnCard1 = GetNode<TextureButton>("Canvas/GridTeam1/HBoxCards/BtnCard1");
        _btnCard1.Pressed += () => { GD.Print("_btnCard1"); };
    }

    public override void _Process(double delta)
    {
    }
}
