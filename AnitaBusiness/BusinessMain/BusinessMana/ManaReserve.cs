namespace AnitaBusiness.BusinessMain.BusinessMana;

public class ManaReserve(ManaType manaType, ManaVal reserve)
{
    public ManaType ManaType { get; set; } = manaType;
    public ManaVal Reserve { get; set; } = reserve;
}