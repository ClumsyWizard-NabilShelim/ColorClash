public class UP_Health : Upgrade
{
    public override void Activate()
    {
        PlayerDataManager.PlayerStats.AddHealth();
    }
}