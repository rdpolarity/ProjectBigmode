using Bigmode;

public class Leaf : IUpgrade
{
    public void Init()
    {
        // get the player
        PlayerController.Instance.Force += 100;
    }
}
