namespace AnitaBusiness.BusinessMain;

public class GameMaster
{
    private static int _id;
    
    public static int GetNextId()
    {
        return ++_id;
    }
    
    public Team Team1 { get; set; }
    public Team Team2 { get; set; }

    public GameMaster()
    {
        Team1 = new Team(this, TeamId.Team1);
        for (int i = 0; i < 3; i++)
        {
            var burn = new Entity(this);
            burn.Name = "Burn";
            burn.Zone = Zone.Deck;
            Team1.Deck.Add(burn);
        }
        Team2 = new Team(this, TeamId.Team2);
    }
    
    public void DamageTeam(Team teamToDamage)
    {
        teamToDamage.Hp -= 2;
    }
}