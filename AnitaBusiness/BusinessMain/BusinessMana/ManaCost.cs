namespace AnitaBusiness.BusinessMain.BusinessMana;

public class ManaCost(ManaType manaType, ManaVal cost)
{
    public ManaType ManaType { get; set; } = manaType;
    public ManaVal Cost { get; set; } = cost;
}