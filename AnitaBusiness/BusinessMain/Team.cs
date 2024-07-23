namespace AnitaBusiness.BusinessMain;

public class Team(GameMaster gameMaster, TeamId teamId)
{
    public GameMaster GameMaster { get; } = gameMaster;
    public TeamId TeamId { get; } = teamId;

    public Team EnemyTeam => TeamId == TeamId.Team1 ? GameMaster.Team2 : GameMaster.Team1;
    public int Hp { get; set; } = 20;
    public int ManaA { get; set; } = 0;
    public int ManaB { get; set; } = 0;
    public int ManaC { get; set; } = 0;
    public int ManaToPay { get; set; }

    public TeamState TeamState { get; set; }

    public bool CastSpell()
    {
        var successfulTransition = false;
        
        if (TeamState == TeamState.None)
        {
            TeamState = TeamState.CastingPayCosts;

            ManaToPay = 2;

            successfulTransition = true;
        }

        return successfulTransition;
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
        if (TeamState == TeamState.CastingCostsPayed)
        {
            GameMaster.DamageTeam(EnemyTeam);
                
            TeamState = TeamState.None;
        }
    }

    public void DrawCard()
    {
        //
    }
}