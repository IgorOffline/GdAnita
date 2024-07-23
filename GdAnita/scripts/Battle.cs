using AnitaBusiness.BusinessMain;
using Godot;
using ImGuiNET;

public partial class Battle : Node3D
{
    private Global? _global;
    private GameMaster GameMaster => _global!.GameMaster!;

    private Camera3D? _cam;
    private Label? _lblTeam1Hp;
    private Label? _lblTeam2Hp;
    private Button? _btnTeam1ManaA;
    private TextureButton? _btnCard1;
    private AudioStreamPlayer3D? _audioStreamPlayerCasting;

    private Node? _lastCollider;
    private uint _groundMask;

    private double _raycastTimer;
    private double _raycastTimerMax = 0.25;
    private bool _doRaycast;
    private double _manaTimer;
    private double _manaTimerMax = 0.25;

    public override void _Ready()
    {
        _global = GetNode<Global>("/root/Global");
        _cam = GetNode<Camera3D>("Camera");
        _groundMask = GetNode("Slots/Team1Creature1").GetNode<StaticBody3D>("StaticBody3D").CollisionLayer;

        _lblTeam1Hp = GetNode<Label>("Canvas/GridTeam1/VBox1/LblHP");
        _lblTeam2Hp = GetNode<Label>("Canvas/GridTeam2/VBox1/LblHP");
        _btnTeam1ManaA = GetNode<Button>("Canvas/GridTeam1/VBox1/HBoxMana/BtnManaA");

        var btnTeam1ManaB = GetNode<Button>("Canvas/GridTeam1/VBox1/HBoxMana/BtnManaB");
        btnTeam1ManaB.Text = GameMaster.Team1.ManaB.ToString();
        var btnTeam1ManaC = GetNode<Button>("Canvas/GridTeam1/VBox1/HBoxMana/BtnManaC");
        btnTeam1ManaC.Text = GameMaster.Team1.ManaC.ToString();

        _btnTeam1ManaA.Pressed += () => { GameMaster.Team1.PayManaA(); };

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
        _raycastTimer += delta;

        if (_raycastTimer > _raycastTimerMax)
        {
            _doRaycast = true;

            _raycastTimer = 0;
        }

        _manaTimer += delta;

        if (_manaTimer > _manaTimerMax)
        {
            if (GameMaster.Team1.TeamState == TeamState.CastingCostsPayed)
            {
                GameMaster.Team2.Hp -= 2;
                GameMaster.Team1.ManaA = 2;

                GameMaster.Team1.TeamState = TeamState.None;
            }

            _manaTimer = 0;
        }

        _lblTeam1Hp!.Text = GameMaster.Team1.Hp.ToString();
        _lblTeam2Hp!.Text = GameMaster.Team2.Hp.ToString();
        _btnTeam1ManaA!.Text = GameMaster.Team1.ManaA.ToString();

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