public class UP_PowerUpSlots : Upgrade
{
    public override void Activate()
    {
        PlayerDataManager.PlayerStats.AddSlot();
    }
}
