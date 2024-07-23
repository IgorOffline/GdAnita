using Godot;
using System;
using ImGuiNET;

public partial class Battle : Node3D
{
    private Global? _global;
    
    private Camera3D? _cam;
    private Button? _btnTeam1RPA1;
    private TextureButton? _btnCard1;
    
    private Node? _lastCollider;
    private uint _groundMask;

    private double _raycastTimer;
    private double _raycastTimerMax = 0.25;
    private bool _doRaycast;

    public override void _Ready()
    {
        _global = GetNode<Global>("/root/Global");
        _cam = GetNode<Camera3D>("Camera");
        _groundMask = GetNode("Slots/Team1Creature1").GetNode<StaticBody3D>("StaticBody3D").CollisionLayer;
        _btnTeam1RPA1 = GetNode<Button>("Canvas/GridTeam1/VBox1/HBoxRP/BtnRPA1");
        _btnTeam1RPA1.Pressed += () => { GD.Print("_btnTeam1RPA1"); };
        _btnCard1 = GetNode<TextureButton>("Canvas/GridTeam1/HBoxCards/BtnCard1");
        _btnCard1.Pressed += () => { GD.Print("_btnCard1"); };
    }

    public override void _Process(double delta)
    {
        _raycastTimer += _raycastTimerMax;

        if (_raycastTimer > _raycastTimerMax)
        {
            _doRaycast = true;
            
            _raycastTimer = 0;
        }

        ImGui.Begin("Battle");
        ImGui.Text("Counter: " + _global!.Counter);
        ImGui.Text(_lastCollider == null ? "lastCollider null" : _lastCollider.Name.ToString());
        ImGui.End();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_doRaycast)
        {
            return;
        }
        
        var mousePosition = GetViewport().GetMousePosition();
        var from = _cam!.ProjectRayOrigin(mousePosition);
        var to = from + _cam.ProjectRayNormal(mousePosition) * 200F;
            
        var rayParams = new PhysicsRayQueryParameters3D();
        rayParams.From = from;
        rayParams.To = to;
        rayParams.CollisionMask = _groundMask;
        
        var intersection = GetWorld3D().DirectSpaceState.IntersectRay(rayParams);
        if (intersection.Count > 0 && intersection["collider"].AsGodotObject() is StaticBody3D)
        {
            var sb = intersection["collider"].As<StaticBody3D>();
            _lastCollider = sb.GetParent();

            ++_global!.Counter;
        }
        else
        {
            _lastCollider = null;
        }
        
        _doRaycast = false;
    }
}
