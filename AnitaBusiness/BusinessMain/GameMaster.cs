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
        Team2 = new Team(this, TeamId.Team2);
        for (var i = 0; i < 8; i++)
        {
            var emptyCreature1 = Util.CreateEmptyCreatureSlot(this);
            Team1.CreatureZone[i] = emptyCreature1;
            var emptyCreature2 = Util.CreateEmptyCreatureSlot(this);
            Team2.CreatureZone[i] = emptyCreature2;
        }
        for (var i = 0; i < 3; i++)
        {
            var burn = new Entity(this);
            burn.Name = new EntityName("Burn");
            burn.CardType = CardType.Sorcery;
            burn.Zone = Zone.Deck;
            burn.Damage = new Damage(i + 2);
            burn.ManaCostA = new ManaCost(ManaType.A, new ManaVal(i + 1));
            Team1.Deck.Add(burn);
        }
        for (var i = 0; i < 3; i++)
        {
            var bee = new Entity(this);
            bee.Name = new EntityName("Bee");
            bee.CardType = CardType.Creature;
            bee.Zone = Zone.Creature;
            bee.Hp = new Hp(1);
            bee.Damage = new Damage(0);
            bee.ManaCostA = new ManaCost(ManaType.A, new ManaVal(0));

            bee.PlacedName = new EntityName("Team2Creature" + i);
            Team2.CreatureZone[i] = bee;
        }
    }

    public void DamageTeam(Team teamToDamage, Entity source)
    {
        teamToDamage.Hp = new Hp(teamToDamage.Hp.Val - source.Damage.Val);
    }
}