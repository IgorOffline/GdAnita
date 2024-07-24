using AnitaBusiness.BusinessMain.BusinessMana;

namespace AnitaBusiness.BusinessMain.BusinessTeam;

public class Team(GameMaster gameMaster, TeamId teamId)
{
    public GameMaster GameMaster { get; } = gameMaster;
    public TeamId TeamId { get; } = teamId;

    public Team EnemyTeam => TeamId == TeamId.Team1 ? GameMaster.Team2 : GameMaster.Team1;
    public List<Entity> Deck { get; set; } = [];
    public List<Entity> Hand { get; set; } = [];
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
    
    public Entity? Hover(CardIndex cardIndex)
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
                
                ManaReserveA.Reserve = new ManaVal(ManaReserveA.Reserve.Val - 1);

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
            GameMaster.DamageTeam(EnemyTeam, Action);

            var card = Hand.First(card => card.Id == Action.Id);
            card.Zone = Zone.Graveyard;
            Hand.Remove(card);
            Action = null;
            
            TeamState = TeamState.None;
        }
    }

    public void DrawCard()
    {
        if (Deck.Count > 0 && Hand.Count < 5)
        {
            var entity = Deck.First();
            Deck.RemoveAt(0);
            entity.Zone = Zone.Hand;
            Hand.Add(entity);
        }
    }
}