using Bigmode;

public class Tongue : IUpgrade
{
    public void Init()
    {
        var playerController = PlayerController.Instance;
        var biggie = playerController.gameObject.GetComponent<Biggie>();
        biggie.IncreaseTongueLength(0.2f);
    }
}
