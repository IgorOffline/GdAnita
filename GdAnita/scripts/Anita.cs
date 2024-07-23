using Godot;
using ImGuiNET;

public partial class Anita : Node3D
{
    private AnimationPlayer? _animationPlayerEnemy;
    private AnimationPlayer? _animationPlayerWorker;
    
    public override void _Ready()
    {
        GD.Print("Anita");

        _animationPlayerEnemy = GetNode<AnimationPlayer>("Enemy/AnimationPlayer");
        _animationPlayerWorker = GetNode<AnimationPlayer>("Worker/AnimationPlayer");

        _animationPlayerEnemy.SpeedScale = 2.5F;
        _animationPlayerWorker.SpeedScale = 2.5F;
    }

    public override void _Process(double delta)
    {
        ImGui.Begin("Anita");
        if (ImGui.Button("Button"))
        {
            _animationPlayerEnemy!.Stop();
            _animationPlayerWorker!.Stop();
            
            _animationPlayerEnemy!.Play("LArmAction");
            _animationPlayerWorker!.Play("LArmAction");
        }
        ImGui.End();
    }
}