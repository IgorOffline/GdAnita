﻿namespace AnitaBusiness.BusinessMain;

public class GameMaster
{
    public int Counter { get; set; }
    public Team Team1 { get; set; } = new();
    public Team Team2 { get; set; } = new();
}