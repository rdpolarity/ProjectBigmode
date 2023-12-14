using Bigmode;

public class Planet : IUpgrade
{
    public void Init()
    {
        var playerController = PlayerController.Instance;
        var biggie = playerController.gameObject.GetComponent<Biggie>();
        biggie.IncreaseMass(10);
    }
}
