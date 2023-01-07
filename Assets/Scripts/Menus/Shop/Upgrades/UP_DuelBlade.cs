public class UP_DuelBlade : Upgrade
{
    public override void Activate()
    {
        PlayerDataManager.PlayerStats.AddBlade();
    }
}