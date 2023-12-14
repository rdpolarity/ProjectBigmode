using Bigmode;

public class EpicFly : IUpgrade
{
    public void Init()
    {
        var playerController = PlayerController.Instance;
        var biggie = playerController.gameObject.GetComponent<Biggie>();
        biggie.AdditionalMassPerFly += 1;
    }
}
