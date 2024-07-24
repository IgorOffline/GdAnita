using AnitaBusiness.BusinessMain.BusinessMana;

namespace AnitaBusiness.BusinessMain;

public class Entity(GameMaster gameMaster)
{
    public GameMaster GameMaster { get; set; } = gameMaster;
    public int Id { get; set; } = GameMaster.GetNextId();
    public EntityType Type { get; set; } = EntityType.Card;
    public Zone Zone { get; set; }
    public string Name { get; set; } = "?";
    public ManaCost ManaCostA { get; set; } = new(ManaType.A, new ManaVal(0));
    public Damage Damage { get; set; } = new Damage(0);
}