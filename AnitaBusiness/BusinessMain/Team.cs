namespace AnitaBusiness.BusinessMain;

public class Team(GameMaster gameMaster, TeamId teamId)
{
    public GameMaster GameMaster { get; } = gameMaster;
    public TeamId TeamId { get; } = teamId;

    public Team EnemyTeam => TeamId == TeamId.Team1 ? GameMaster.Team2 : GameMaster.Team1;
    public List<Entity> Deck { get; set; } = [];
    public List<Entity> Hand { get; set; } = [];
    public int Hp { get; set; } = 20;
    public int ManaA { get; set; } = 0;
    public int ManaB { get; set; } = 0;
    public int ManaC { get; set; } = 0;
    public int ManaToPay { get; set; }

    public TeamState TeamState { get; set; }

    public bool CastSpell(CardIndex cardIndex)
    {
        var successfulTransition = false;
        
        if (Hand.Count > cardIndex.Val && TeamState == TeamState.None)
        {
            TeamState = TeamState.CastingPayCosts;

            ManaToPay = 2;

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
            if (ManaA > 0)
            {
                ManaToPay -= 1;
                ManaA -= 1;

                costSuccessfullyPayed = true;
            }

            if (ManaToPay == 0)
            {
                TeamState = TeamState.CastingCostsPayed;
            }
        }

        return costSuccessfullyPayed;
    }

    public void CastingCostsPayed()
    {
        if (Hand.Count > 0 && TeamState == TeamState.CastingCostsPayed)
        {
            GameMaster.DamageTeam(EnemyTeam);

            var entity = Hand.First();
            Hand.RemoveAt(0);
            
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