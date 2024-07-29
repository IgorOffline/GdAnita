using AnitaBusiness.BusinessMain.BusinessMana;
using AnitaBusiness.BusinessMain.BusinessTeam;
using AnitaBusiness.BusinessMain.BusinessType;
using AnitaBusiness.BusinessMain.BusinessType.Enums;

namespace AnitaBusiness.BusinessMain;

public static class Util
{
    public static Entity CreateEmptyCreatureSlot(GameMaster gameMaster)
    {
        var emptyCreature = new Entity(gameMaster);
        emptyCreature.Name = new EntityName("Empty");
        emptyCreature.Zone = Zone.Creature;
        emptyCreature.AnitaType = AnitaType.None;
        
        return emptyCreature;
    }
    
    public static void RevertCreatureToEmptySlot(Entity creature)
    {
        creature.Name = new EntityName("Empty");
        creature.AnitaType = AnitaType.None;
    }

    public static Identity TeamCreatureIdentityFormula(int index, TeamId teamId)
    {
        var team2Offset = TeamIdUtil.IsTeam1(teamId) ? 0 : 8;
        
        return new Identity(index + 1 + team2Offset);
    }

    public static Entity CloneCreature(Entity original)
    {
        var newCreature = new Entity(original.GameMaster);
        newCreature.Name = original.Name;
        newCreature.CardType = original.CardType;
        newCreature.Zone = original.Zone;
        newCreature.Damage = original.Damage;
        newCreature.Hp = original.Hp;
        newCreature.ManaCostA = original.ManaCostA;

        return newCreature;
    }
    
    public static string TeamStateToString(TeamState teamState) => teamState switch
    {
        TeamState.None => "None",
        TeamState.CastingPayCosts => "CastingPayCosts",
        TeamState.CastingCostsPaid => "CastingCostsPaid",
        TeamState.Targeting => "Targeting",
        TeamState.SpawningCreature => "SpawningCreature",
        _ => "?"
    };

    public static string ZoneToString(Zone zone) => zone switch
    {
        Zone.None => "None",
        Zone.Deck => "Deck",
        Zone.Hand => "Hand",
        Zone.Graveyard => "Graveyard",
        Zone.Creature => "Creature",
        _ => "?"
    };

    public static string ManaTypeToString(ManaType manaType) => manaType switch
    {
        ManaType.None => "None",
        ManaType.A => "A",
        ManaType.B => "B",
        ManaType.C => "C",
        _ => "?"
    };
    
    public static string CardTypeToString(CardType cardType) => cardType switch
    {
        CardType.None => "None",
        CardType.Sorcery => "Sorcery",
        CardType.Creature => "Creature",
        _ => "?"
    };
}