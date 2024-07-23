namespace AnitaBusiness.BusinessMain;

public class Entity(GameMaster gameMaster)
{
    public GameMaster GameMaster { get; set; } = gameMaster;
    public int Id { get; set; } = GameMaster.GetNextId();
    public Zone Zone { get; set; }
    public string Name { get; set; } = "?";
}