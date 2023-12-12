using InsaneScatterbrain.ScriptGraph;
using UnityEngine;

public class UpdateBackgroundSize : MonoBehaviour
{
    [SerializeField] private ScriptGraphRunner scriptGraphRunner = null;

    private SpriteRenderer spriteRenderer;
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        scriptGraphRunner.OnProcessed += result =>
        {
            var size = scriptGraphRunner.GetIn<int>("Size");
            spriteRenderer.size = new Vector2(size * 2, size * 2);
            transform.position = new Vector3(size, size, 0);
        };
    }
}
