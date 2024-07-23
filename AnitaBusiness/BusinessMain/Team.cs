namespace AnitaBusiness.BusinessMain;

public class Team
{
    public int Hp { get; set; } = 4;
    public int ManaA { get; set; } = 2;
    public int ManaB { get; set; } = 0;
    public int ManaC { get; set; } = 0;

    public TeamState TeamState { get; set; }

    public void CastSpell()
    {
        if (TeamState == TeamState.None)
        {
            TeamState = TeamState.CastingPayCosts;
        }
    }
    
    public void PayManaA()
    {
        if (TeamState == TeamState.CastingPayCosts)
        {
            if (ManaA > 0)
            {
                ManaA -= 1;
            }

            if (ManaA == 0)
            {
                TeamState = TeamState.CastingCostsPayed;
            }
        }
    }
}