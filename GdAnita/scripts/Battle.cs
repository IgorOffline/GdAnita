using System.Collections.Generic;
using AnitaBusiness.BusinessMain;
using AnitaBusiness.BusinessMain.BusinessMana;
using AnitaBusiness.BusinessMain.BusinessType;
using AnitaBusiness.BusinessMain.BusinessType.Enums;
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
    private TextureButton? _btnTeam2Avatar;
    private TextureButton? _btnCard1;
    private TextureButton? _btnCard2;
    private TextureButton? _btnCard3;
    private TextureButton? _btnCard4;
    private TextureButton? _btnCard5;
    private TextureButton? _btnCard6;
    private TextureButton? _btnCard7;
    private TextureButton? _btnCard8;
    private TextureButton? _btnCard9;
    private TextureButton? _btnCard10;
    private TextureButton[] _buttons = new TextureButton[10];
    private CompressedTexture2D? _btnCardTextureNormalEmpty;
    private CompressedTexture2D? _btnCardTextureNormalHeld;
    private AudioStreamPlayer3D? _audioStreamPlayerCasting;
    private AudioStreamPlayer3D? _audioStreamPlayerPayCost;
    private AudioStreamPlayer3D? _audioStreamPlayerTargetFace;
    private AudioStreamPlayer3D? _audioStreamPlayerTargetCreature;
    private AudioStreamPlayer3D? _audioStreamPlayerDrawCard;

    private PackedScene? _creature1A;
    private List<Node3D> _creatures = [];

    private Node3D? _lastGroundCollider;
    private Node3D? _lastCreatureCollider;
    private uint _groundMask;
    private uint _creatureMask;

    private Entity? _hoveredHand;
    private Entity? _hoveredCreature;
    private double _raycastTimer;
    private double _raycastTimerMax = 0.11;
    private bool _doRaycast;
    private double _manaTimer;
    private double _manaTimerMax = 0.11;
    private double _handTimer;
    private double _handTimerMax = 0.11;

    public override void _Ready()
    {
        _global = GetNode<Global>("/root/Global");
        _cam = GetNode<Camera3D>("Camera");
        _groundMask = GetNode("Slots/Team1Creature1").GetNode<StaticBody3D>("StaticBody3D").CollisionLayer;
        _lblTeam1Hp = GetNode<Label>("Canvas/GridTeam1/VBox1/LblHP");
        _lblTeam2Hp = GetNode<Label>("Canvas/GridTeam2/VBox1/LblHP");
        _btnTeam1ManaA = GetNode<Button>("Canvas/GridTeam1/VBox1/HBoxMana/BtnManaA");
        var btnTeam2ManaA = GetNode<Button>("Canvas/GridTeam2/VBox1/HBoxMana/BtnManaA");
        var btnTeam1ManaB = GetNode<Button>("Canvas/GridTeam1/VBox1/HBoxMana/BtnManaB");
        var btnTeam1ManaC = GetNode<Button>("Canvas/GridTeam1/VBox1/HBoxMana/BtnManaC");
        var btnTeam2ManaB = GetNode<Button>("Canvas/GridTeam2/VBox1/HBoxMana/BtnManaB");
        var btnTeam2ManaC = GetNode<Button>("Canvas/GridTeam2/VBox1/HBoxMana/BtnManaC");
        _btnTeam2Avatar = GetNode<TextureButton>("Canvas/GridTeam2/BtnAvatar");
        _btnCard1 = GetNode<TextureButton>("Canvas/GridTeam1/VBoxCards/HBoxCardsTop/BtnCard1");
        _btnCard2 = GetNode<TextureButton>("Canvas/GridTeam1/VBoxCards/HBoxCardsTop/BtnCard2");
        _btnCard3 = GetNode<TextureButton>("Canvas/GridTeam1/VBoxCards/HBoxCardsTop/BtnCard3");
        _btnCard4 = GetNode<TextureButton>("Canvas/GridTeam1/VBoxCards/HBoxCardsTop/BtnCard4");
        _btnCard5 = GetNode<TextureButton>("Canvas/GridTeam1/VBoxCards/HBoxCardsTop/BtnCard5");
        _btnCard6 = GetNode<TextureButton>("Canvas/GridTeam1/VBoxCards/HBoxCardsBottom/BtnCard6");
        _btnCard7 = GetNode<TextureButton>("Canvas/GridTeam1/VBoxCards/HBoxCardsBottom/BtnCard7");
        _btnCard8 = GetNode<TextureButton>("Canvas/GridTeam1/VBoxCards/HBoxCardsBottom/BtnCard8");
        _btnCard9 = GetNode<TextureButton>("Canvas/GridTeam1/VBoxCards/HBoxCardsBottom/BtnCard9");
        _btnCard10 = GetNode<TextureButton>("Canvas/GridTeam1/VBoxCards/HBoxCardsBottom/BtnCard10");
        _audioStreamPlayerCasting = GetNode<AudioStreamPlayer3D>("AudioStreamPlayerCasting");
        _audioStreamPlayerPayCost = GetNode<AudioStreamPlayer3D>("AudioStreamPlayerPayCost");
        _audioStreamPlayerTargetFace = GetNode<AudioStreamPlayer3D>("AudioStreamPlayerTargetFace");
        _audioStreamPlayerTargetCreature = GetNode<AudioStreamPlayer3D>("AudioStreamPlayerTargetCreature");
        _audioStreamPlayerDrawCard = GetNode<AudioStreamPlayer3D>("AudioStreamPlayerDrawCard");

        _btnCardTextureNormalEmpty = GD.Load<CompressedTexture2D>("res://textures/anitabrown.png");
        _btnCardTextureNormalHeld = GD.Load<CompressedTexture2D>("res://textures/anitagreen1.png");

        _creature1A = ResourceLoader.Load<PackedScene>("scenes/alice_1a.tscn");
        var instanceForCreatureMask = _creature1A.Instantiate<Node3D>();
        _creatureMask = instanceForCreatureMask.GetNode<StaticBody3D>("StaticBody3D").CollisionLayer;
        instanceForCreatureMask.QueueFree();

        GameMaster.Team1.ManaReserveA = new ManaReserve(ManaType.A, new ManaVal(15));

        SpawnCreatures();

        _buttons[0] = _btnCard1;
        _buttons[1] = _btnCard2;
        _buttons[2] = _btnCard3;
        _buttons[3] = _btnCard4;
        _buttons[4] = _btnCard5;
        _buttons[5] = _btnCard6;
        _buttons[6] = _btnCard7;
        _buttons[7] = _btnCard8;
        _buttons[8] = _btnCard9;
        _buttons[9] = _btnCard10;

        btnTeam2ManaA.Text = GameMaster.Team2.ManaReserveA.Reserve.Val.ToString();
        btnTeam1ManaB.Text = GameMaster.Team1.ManaReserveB.Reserve.Val.ToString();
        btnTeam1ManaC.Text = GameMaster.Team1.ManaReserveC.Reserve.Val.ToString();
        btnTeam2ManaB.Text = GameMaster.Team1.ManaReserveB.Reserve.Val.ToString();
        btnTeam2ManaC.Text = GameMaster.Team1.ManaReserveC.Reserve.Val.ToString();

        for (var i = 0; i < _buttons.Length; i++)
        {
            var val = i;
            var button = _buttons[val];

            button.Pressed += () =>
            {
                if (GameMaster.Team1.CastSpell(new CardIndex(val)))
                {
                    _audioStreamPlayerCasting!.Play();
                }
            };
            button.MouseEntered += () =>
            {
                _hoveredHand = GameMaster.Team1.HoverHand(new CardIndex(val));
                //GD.Print("Mouse entered: " + val);
            };
            button.MouseExited += () =>
            {
                _hoveredHand = null;
                //GD.Print("Mouse exited: " + val);
            };
        }

        _btnTeam2Avatar.Pressed += () =>
        {
            if (GameMaster.Team1.TargetEnemyAvatar())
            {
                _audioStreamPlayerTargetFace!.Play();
            }
        };

        _btnTeam1ManaA.Pressed += () =>
        {
            if (GameMaster.Team1.PayManaA())
            {
                _audioStreamPlayerPayCost!.Play();
            }
        };
    }

    public void DestroyCreatures()
    {
        _hoveredCreature = null;
        _lastCreatureCollider = null;

        foreach (var creature in _creatures)
        {
            creature.QueueFree();
        }

        _creatures = [];
    }

    public void SpawnCreatures()
    {
        for (var i = 0; i < GameMaster.Team1.CreatureZone.Length; i++)
        {
            var creature = GameMaster.Team1.CreatureZone[i];

            if (creature.AnitaType == AnitaType.Card)
            {
                var newCreature = _creature1A!.Instantiate<Node3D>();
                newCreature.Name = "Team1Creature" + i;
                AddChild(newCreature, true);
                newCreature.Position = BattleUtil.CreatureIndexToPosition(i, true);

                _creatures.Add(newCreature);
            }
        }
        for (var i = 0; i < GameMaster.Team2.CreatureZone.Length; i++)
        {
            var creature = GameMaster.Team2.CreatureZone[i];

            if (creature.AnitaType == AnitaType.Card)
            {
                var newCreature = _creature1A!.Instantiate<Node3D>();
                newCreature.Name = "Team2Creature" + i;
                AddChild(newCreature, true);
                newCreature.Position = BattleUtil.CreatureIndexToPosition(i, false);

                _creatures.Add(newCreature);
            }
        }
    }

    public override void _Process(double delta)
    {
        _raycastTimer += delta;

        if (_raycastTimer > _raycastTimerMax)
        {
            // START: ADDITIONAL TIMER LOGIC
            _hoveredCreature = null;

            if (_lastCreatureCollider != null)
            {
                var lastCreatureColliderPlacedPosition = _lastCreatureCollider.Position;

                foreach (var team1Creature in GameMaster.Team1.CreatureZone)
                {
                    var placedIndex = BattleUtil.PositionToIndex(GameMaster, lastCreatureColliderPlacedPosition, true);

                    if (!placedIndex.Equals(new Identity(0)) && placedIndex.Equals(team1Creature.PlacedIndex))
                    {
                        _hoveredCreature = team1Creature;
                    }
                }
                foreach (var team2Creature in GameMaster.Team2.CreatureZone)
                {
                    var placedIndex = BattleUtil.PositionToIndex(GameMaster, lastCreatureColliderPlacedPosition, false);

                    if (!placedIndex.Equals(new Identity(0)) && placedIndex.Equals(team2Creature.PlacedIndex))
                    {
                        _hoveredCreature = team2Creature;
                    }
                }
            }
            // END: ADDITIONAL TIMER LOGIC

            _doRaycast = true;

            _raycastTimer = 0;
        }

        if (_hoveredCreature != null && Input.IsActionJustPressed("MousePrimary"))
        {
            if (GameMaster.CreatureAction(_hoveredCreature))
            {
                DestroyCreatures();

                SpawnCreatures();

                _audioStreamPlayerTargetCreature!.Play();
            }
        }

        _manaTimer += delta;

        if (_manaTimer > _manaTimerMax)
        {
            GameMaster.Team1.CastingCostsPaid();

            _manaTimer = 0;
        }

        _lblTeam1Hp!.Text = GameMaster.Team1.Hp.Val.ToString();
        _lblTeam2Hp!.Text = GameMaster.Team2.Hp.Val.ToString();
        _btnTeam1ManaA!.Text = GameMaster.Team1.ManaReserveA.Reserve.Val.ToString();

        _handTimer += delta;

        if (_handTimer > _handTimerMax)
        {
            for (var i = 0; i < _buttons.Length; i++)
            {
                var button = _buttons[i];

                if (GameMaster.Team1.Hand.Count > i)
                {
                    button.TextureNormal = _btnCardTextureNormalHeld;
                }
                else
                {
                    button.TextureNormal = _btnCardTextureNormalEmpty;
                }
            }

            _handTimer = 0;
        }

        ImGui.Begin("Battle");
        ImGui.Text(_lastGroundCollider == null ? "lastCollider null" : _lastGroundCollider.Name.ToString());
        if (ImGui.Button("Draw card"))
        {
            if (GameMaster.Team1.DrawCard())
            {
                _audioStreamPlayerDrawCard!.Play();
            }
        }

        ImGui.Text("--- Team1 ---");
        ImGui.Text("Deck Count: " + GameMaster.Team1.Deck.Count);
        ImGui.Text("State: " + Util.TeamStateToString(GameMaster.Team1.TeamState));
        ImGui.Text("Hand count: " + GameMaster.Team1.Hand.Count);
        ImGui.Text("ManaToPayA: " + GameMaster.Team1.ManaToPayA.Cost.Val);
        ImGui.End();

        ImGui.Begin("Card");
        if (_hoveredHand == null && _hoveredCreature == null)
        {
            ImGui.Text("?");
        }
        else
        {
            var hoveredEntity = _hoveredHand ?? _hoveredCreature;

            ImGui.Text("Id: " + hoveredEntity!.Id.Val);
            ImGui.Text("Name: " + hoveredEntity.Name.Val);
            ImGui.Text("Zone: " + Util.ZoneToString(hoveredEntity.Zone));
            ImGui.Text("CardType: " + Util.CardTypeToString(hoveredEntity.CardType));
            ImGui.Text("ManaCostA: " + hoveredEntity.ManaCostA.Cost.Val);
            ImGui.Text("Damage: " + hoveredEntity.Damage.Val);
            ImGui.Text("Hp: " + hoveredEntity.Hp.Val);
        }

        ImGui.End();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_doRaycast)
        {
            return;
        }

        _lastGroundCollider = null;
        _lastCreatureCollider = null;

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
            _lastGroundCollider = sb.GetParentNode3D();
        }

        rayParams.CollisionMask = _creatureMask;
        intersection = GetWorld3D().DirectSpaceState.IntersectRay(rayParams);
        if (intersection.Count > 0 && intersection["collider"].AsGodotObject() is StaticBody3D)
        {
            var sb = intersection["collider"].As<StaticBody3D>();
            _lastCreatureCollider = sb.GetParentNode3D();
        }

        _doRaycast = false;
    }

    private static class BattleUtil
    {
        public static Vector3 CreatureIndexToPosition(int i, bool team1)
        {
            var zOffset = team1 ? 2 : 0;
            
            return new Vector3(3 + i * 2, 0, 5 + zOffset);
        }

        public static Identity PositionToIndex(GameMaster gameMaster, Vector3 position, bool team1)
        {
            var team = team1 ? gameMaster.Team1 : gameMaster.Team2;
            
            for (var i = 0; i < team.CreatureZone.Length; i++)
            {
                if (position.Equals(CreatureIndexToPosition(i, team1)))
                {
                    return Util.TeamCreatureIdentityFormula(i, team1);
                }
            }

            return new Identity(0);
        }
    }
}