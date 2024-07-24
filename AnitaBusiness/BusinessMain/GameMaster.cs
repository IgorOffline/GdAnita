using AnitaBusiness.BusinessMain.BusinessMana;
using AnitaBusiness.BusinessMain.BusinessTeam;

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
            burn.Damage = new Damage(i + 2);
            burn.ManaCostA = new ManaCost(ManaType.A, new ManaVal(i + 1));
            Team1.Deck.Add(burn);
        }
        Team2 = new Team(this, TeamId.Team2);
    }
    
    public void DamageTeam(Team teamToDamage, Entity source)
    {
        teamToDamage.Hp = new Hp(teamToDamage.Hp.Val - source.Damage.Val);
    }
}