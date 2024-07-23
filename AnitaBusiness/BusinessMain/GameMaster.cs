namespace AnitaBusiness.BusinessMain;

public class GameMaster
{
    public Team Team1 { get; set; }
    public Team Team2 { get; set; }

    public GameMaster()
    {
        Team1 = new Team(this, TeamId.Team1);
        Team2 = new Team(this, TeamId.Team2);
    }
    
    public void DamageTeam(Team teamToDamage)
    {
        teamToDamage.Hp -= 2;
    }
}