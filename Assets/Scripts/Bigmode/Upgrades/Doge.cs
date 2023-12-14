using Bigmode;

public class Doge : IUpgrade 
{
    public void Init()
    {
        var playerController = PlayerController.Instance;
        var biggie = playerController.gameObject.GetComponent<Biggie>();
        biggie.SpawnDoge();
    }
}
