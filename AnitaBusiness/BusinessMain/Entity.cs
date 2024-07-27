﻿using AnitaBusiness.BusinessMain.BusinessMana;

namespace AnitaBusiness.BusinessMain;

public class Entity(GameMaster gameMaster)
{
    public GameMaster GameMaster { get; set; } = gameMaster;
    public Identity Id { get; set; } = GameMaster.GetNextId();
    public BusinessType BusinessType { get; set; } = BusinessType.Card;
    public CardType CardType { get; set; } = CardType.None;
    public Zone Zone { get; set; }
    public EntityName Name { get; set; } = new("?");
    public Identity PlacedIndex { get; set; } = new(0);
    public ManaCost ManaCostA { get; set; } = new(ManaType.A, new ManaVal(0));
    public Hp Hp { get; set; } = new(1);
    public Damage Damage { get; set; } = new(0);
}