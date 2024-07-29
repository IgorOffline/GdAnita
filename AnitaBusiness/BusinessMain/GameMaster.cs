using AnitaBusiness.BusinessMain.BusinessLogging;
using AnitaBusiness.BusinessMain.BusinessMana;
using AnitaBusiness.BusinessMain.BusinessTeam;
using AnitaBusiness.BusinessMain.BusinessType;
using AnitaBusiness.BusinessMain.BusinessType.Enums;

namespace AnitaBusiness.BusinessMain;

public class GameMaster
{
    public ILogger Logger { get; set; }
    
    private static Identity _id = new(0);

    public static Identity GetNextId()
    {
        _id = new Identity(_id.Val + 1);
        
        return _id;
    }

    public Team Team1 { get; set; }
    public Team Team2 { get; set; }

    public GameMaster(ILogger logger)
    {
        Logger = logger;
        
        Team1 = new Team(this, TeamId.Team1);
        Team2 = new Team(this, TeamId.Team2);
        for (var i = 0; i < 8; i++)
        {
            var emptyCreature1 = Util.CreateEmptyCreatureSlot(this);
            Team1.CreatureZone[i] = emptyCreature1;
            var emptyCreature2 = Util.CreateEmptyCreatureSlot(this);
            Team2.CreatureZone[i] = emptyCreature2;
        }
        for (var i = 1; i < 15; i++)
        {
            if (i % 2 == 1)
            {
                var burn = new Entity(this);
                burn.Name = new EntityName("Burn");
                burn.CardType = CardType.Sorcery;
                burn.Zone = Zone.Deck;
                burn.Damage = new Damage(i + 2);
                burn.ManaCostA = new ManaCost(ManaType.A, new ManaVal(i + 1));
                Team1.Deck.Add(burn);
            }
            else
            {
                var bee = new Entity(this);
                bee.Name = new EntityName("Bee");
                bee.CardType = CardType.Creature;
                bee.Zone = Zone.Deck;
                bee.Damage = new Damage(i + 2);
                bee.Hp = new Hp(i + 2);
                bee.ManaCostA = new ManaCost(ManaType.A, new ManaVal(i + 1));
                Team1.Deck.Add(bee);
            }
        }
        {
            var bee = new Entity(this);
            bee.Name = new EntityName("Strong Bee");
            bee.CardType = CardType.Creature;
            bee.Zone = Zone.Creature;
            bee.Hp = new Hp(9);
            bee.Damage = new Damage(9);
            bee.ManaCostA = new ManaCost(ManaType.A, new ManaVal(0));

            bee.PlacedIndex = Util.TeamCreatureIdentityFormula(0, true);
            Team1.CreatureZone[0] = bee;
        }
        for (var i = 0; i < 3; i++)
        {
            var bee = new Entity(this);
            bee.Name = new EntityName("Bee");
            bee.CardType = CardType.Creature;
            bee.Zone = Zone.Creature;
            bee.Hp = new Hp(i + 1);
            bee.Damage = new Damage(i + 1);
            bee.ManaCostA = new ManaCost(ManaType.A, new ManaVal(0));

            bee.PlacedIndex = Util.TeamCreatureIdentityFormula(i, false);
            Team2.CreatureZone[i] = bee;
        }
    }

    public void DamageTeam(Team teamToDamage, Entity source)
    {
        teamToDamage.Hp = new Hp(teamToDamage.Hp.Val - source.Damage.Val);
    }

    public bool CreatureAction(Entity actionableEntity)
    {
        return Team1.CreatureAction(actionableEntity);
    }
    
    public bool SpawnCreature()
    {
        return Team1.SpawnCreature();
    }
}