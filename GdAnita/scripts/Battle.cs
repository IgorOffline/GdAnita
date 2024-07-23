using AnitaBusiness.BusinessMain;
using Godot;
using ImGuiNET;

public partial class Battle : Node3D
{
    private Global? _global;
    private GameMaster GameMaster => _global!.GameMaster!;

    private Camera3D? _cam;
    private Button? _btnTeam1ManaA;
    private TextureButton? _btnCard1;
    private AudioStreamPlayer3D? _audioStreamPlayerCasting;

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

        var hpLabel = GetNode<Label>("Canvas/GridTeam1/VBox1/LblHP");
        hpLabel.Text = GameMaster.Team1.Hp.ToString();

        _btnTeam1ManaA = GetNode<Button>("Canvas/GridTeam1/VBox1/HBoxMana/BtnManaA");
        _btnTeam1ManaA.Text = GameMaster.Team1.ManaA.ToString();
        var btnTeam1ManaB = GetNode<Button>("Canvas/GridTeam1/VBox1/HBoxMana/BtnManaB");
        btnTeam1ManaB.Text = GameMaster.Team1.ManaB.ToString();
        var btnTeam1ManaC = GetNode<Button>("Canvas/GridTeam1/VBox1/HBoxMana/BtnManaC");
        btnTeam1ManaC.Text = GameMaster.Team1.ManaC.ToString();
        _btnTeam1ManaA.Pressed += () => { GD.Print("_btnTeam1ManaA"); };

        _btnCard1 = GetNode<TextureButton>("Canvas/GridTeam1/HBoxCards/BtnCard1");
        _btnCard1.Pressed += () =>
        {
            GameMaster.Team1.CastSpell();
            _audioStreamPlayerCasting!.Play();
        };

        _audioStreamPlayerCasting = GetNode<AudioStreamPlayer3D>("AudioStreamPlayerCasting");
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
        ImGui.Text(_lastCollider == null ? "lastCollider null" : _lastCollider.Name.ToString());
        ImGui.Text("Team1 State: " + Util.TeamStateToString(GameMaster.Team1.TeamState));
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
        }
        else
        {
            _lastCollider = null;
        }

        _doRaycast = false;
    }
}