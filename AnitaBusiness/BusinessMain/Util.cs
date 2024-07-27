﻿using AnitaBusiness.BusinessMain.BusinessMana;
using AnitaBusiness.BusinessMain.BusinessTeam;

namespace AnitaBusiness.BusinessMain;

public static class Util
{
    public static Entity CreateEmptyCreatureSlot(GameMaster gameMaster)
    {
        var emptyCreature = new Entity(gameMaster);
        emptyCreature.Name = new EntityName("Empty");
        emptyCreature.Zone = Zone.Creature;
        emptyCreature.BusinessType = BusinessType.None;
        
        return emptyCreature;
    }
    
    public static void RevertCreatureToEmptySlot(Entity creature)
    {
        creature.Name = new EntityName("Empty");
        creature.BusinessType = BusinessType.None;
    }

    public static Identity Team2CreatureIdentityFormula(int index)
    {
        return new Identity(index + 1 + 8);
    }
    
    public static string TeamStateToString(TeamState teamState) => teamState switch
    {
        TeamState.None => "None",
        TeamState.CastingPayCosts => "CastingPayCosts",
        TeamState.CastingCostsPaid => "CastingCostsPaid",
        TeamState.Targeting => "Targeting",
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