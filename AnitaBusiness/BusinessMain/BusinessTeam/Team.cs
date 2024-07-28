using AnitaBusiness.BusinessMain.BusinessMana;
using AnitaBusiness.BusinessMain.BusinessType;
using AnitaBusiness.BusinessMain.BusinessType.Enums;

namespace AnitaBusiness.BusinessMain.BusinessTeam;

public class Team(GameMaster gameMaster, TeamId teamId)
{
    public GameMaster GameMaster { get; } = gameMaster;
    public TeamId TeamId { get; } = teamId;

    public Team EnemyTeam => TeamId == TeamId.Team1 ? GameMaster.Team2 : GameMaster.Team1;
    public List<Entity> Deck { get; set; } = [];
    public List<Entity> Hand { get; set; } = [];
    public Entity[] CreatureZone { get; set; } = new Entity[8];
    public Entity? Action { get; set; }
    public Hp Hp { get; set; } = new Hp(20);
    public ManaReserve ManaReserveA { get; set; } = new ManaReserve(ManaType.A, new ManaVal(0));
    public ManaReserve ManaReserveB { get; set; } = new ManaReserve(ManaType.B, new ManaVal(0));
    public ManaReserve ManaReserveC { get; set; } = new ManaReserve(ManaType.C, new ManaVal(0));
    public ManaCost ManaToPayA { get; set; } = new ManaCost(ManaType.A, new ManaVal(0));

    public TeamState TeamState { get; set; }

    public bool CastSpell(CardIndex cardIndex)
    {
        var successfulTransition = false;
        
        if (Hand.Count > cardIndex.Val && TeamState == TeamState.None)
        {
            Action = Hand[cardIndex.Val];
            
            TeamState = TeamState.CastingPayCosts;

            ManaToPayA = new ManaCost(Action.ManaCostA.ManaType, Action.ManaCostA.Cost);

            successfulTransition = true;
        }

        return successfulTransition;
    }
    
    public Entity? HoverHand(CardIndex cardIndex)
    {
        return Hand.Count > cardIndex.Val ? Hand[cardIndex.Val] : null;
    }
    
    public bool PayManaA()
    {
        var costSuccessfullyPayed = false;
        
        if (TeamState == TeamState.CastingPayCosts)
        {
            if (ManaReserveA.Reserve.Val > 0)
            {
                ManaToPayA = new ManaCost(ManaType.A, new ManaVal(ManaToPayA.Cost.Val - 1));
                
                ManaReserveA = new ManaReserve(ManaType.A, new ManaVal(ManaReserveA.Reserve.Val - 1));

                costSuccessfullyPayed = true;
            }

            if (ManaToPayA.Cost.Val == 0)
            {
                TeamState = TeamState.CastingCostsPaid;
            }
        }

        return costSuccessfullyPayed;
    }

    public void CastingCostsPaid()
    {
        if (Action != null && TeamState == TeamState.CastingCostsPaid)
        {
            TeamState = TeamState.Targeting;
        }
    }

    public bool DrawCard()
    {
        var successfulDraw = false;
        
        if (Deck.Count > 0 && Hand.Count < 5)
        {
            var entity = Deck.First();
            Deck.RemoveAt(0);
            entity.Zone = Zone.Hand;
            Hand.Add(entity);

            successfulDraw = true;
        }

        return successfulDraw;
    }

    public bool TargetEnemyAvatar()
    {
        var successfulTransition = false;
        
        if (Action != null && TeamState == TeamState.Targeting)
        {
            GameMaster.DamageTeam(EnemyTeam, Action);

            TargetingTransitionCommon();

            successfulTransition = true;
        }
        
        return successfulTransition;
    }
    
    public bool TargetEnemyCreature(Entity enemyCreature)
    {
        var successfulTransition = false;
        
        if (Action != null && TeamState == TeamState.Targeting)
        {
            enemyCreature.Hp = new Hp(enemyCreature.Hp.Val - Action.Damage.Val);

            TargetingTransitionCommon();

            if (enemyCreature.Hp.Val < 1)
            {
                foreach (var enemyCreatureInZone in EnemyTeam.CreatureZone)
                {
                    if (enemyCreatureInZone.Id.Equals(enemyCreature.Id))
                    {
                        Util.RevertCreatureToEmptySlot(enemyCreature);
                    }
                }
            }
            
            successfulTransition = true;
        }
        
        return successfulTransition;
    }

    public void TargetingTransitionCommon()
    {
        var card = Hand.First(card => card.Id == Action!.Id);
        card.Zone = Zone.Graveyard;
        Hand.Remove(card);
        Action = null;
            
        TeamState = TeamState.None;
    }
}