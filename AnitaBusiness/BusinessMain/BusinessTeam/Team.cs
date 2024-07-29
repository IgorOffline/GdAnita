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
            
            if (Action.CardType == CardType.Sorcery)
            {
                TargetingFromHandTransitionCommon();
            }
            else if (Action.CardType == CardType.Creature)
            {
                ClearActionAndTeamState();
            }

            successfulTransition = true;
        }
        
        return successfulTransition;
    }
    
    public bool CreatureAction(Entity actionableEntity)
    {
        var successfulTransition = false;

        if (Action == null && TeamState == TeamState.None)
        {
            Action = actionableEntity;

            TeamState = TeamState.Targeting;
        }
        else if (Action != null && TeamState == TeamState.Targeting)
        {
            if (actionableEntity.CardType == CardType.Sorcery)
            {
                actionableEntity.Hp = new Hp(actionableEntity.Hp.Val - Action.Damage.Val);
            
                if (actionableEntity.Hp.Val < 1)
                {
                    foreach (var enemyCreatureInZone in EnemyTeam.CreatureZone)
                    {
                        if (enemyCreatureInZone.Id.Equals(actionableEntity.Id))
                        {
                            Util.RevertCreatureToEmptySlot(actionableEntity);
                        }
                    }
                }
            
                TargetingFromHandTransitionCommon();
                
                successfulTransition = true;
            }
            else if (actionableEntity.CardType == CardType.Creature)
            {
                actionableEntity.Hp = new Hp(actionableEntity.Hp.Val - Action.Damage.Val);
                Action.Hp = new Hp(Action.Hp.Val - actionableEntity.Damage.Val);

                if (actionableEntity.Hp.Val < 1)
                {
                    foreach (var enemyCreatureInZone in EnemyTeam.CreatureZone)
                    {
                        if (enemyCreatureInZone.Id.Equals(actionableEntity.Id))
                        {
                            Util.RevertCreatureToEmptySlot(actionableEntity);
                        }
                    }
                }
                if (Action.Hp.Val < 1)
                {
                    foreach (var creatureInZone in CreatureZone)
                    {
                        if (creatureInZone.Id.Equals(Action.Id))
                        {
                            Util.RevertCreatureToEmptySlot(Action);
                        }
                    }
                }
            
                ClearActionAndTeamState();
                
                successfulTransition = true;
            }
        }
        
        return successfulTransition;
    }

    public void TargetingFromHandTransitionCommon()
    {
        var card = Hand.First(card => card.Id == Action!.Id);
        card.Zone = Zone.Graveyard;
        Hand.Remove(card);

        ClearActionAndTeamState();
    }

    public void ClearActionAndTeamState()
    {
        Action = null;
        TeamState = TeamState.None;
    }
}