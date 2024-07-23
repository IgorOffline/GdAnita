namespace AnitaBusiness.BusinessMain;

public static class Util
{
    public static string TeamStateToString(TeamState teamState) => teamState switch
    {
        TeamState.None => "None",
        TeamState.Casting => "Casting",
        _ => "?"
    };
}