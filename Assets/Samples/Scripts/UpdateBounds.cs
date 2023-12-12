using InsaneScatterbrain.ScriptGraph;
using UnityEngine;

public class UpdateBounds : MonoBehaviour
{
    [SerializeField] private ScriptGraphRunner scriptGraphRunner = null;
    [SerializeField] private DragCamera dragCamera = null;
    
    private void Start()
    {
        scriptGraphRunner.OnProcessed += result =>
        {
            var size = scriptGraphRunner.GetIn<int>("Size");
            var intSize = Mathf.RoundToInt(size * 2f);
            dragCamera.SetBounds(intSize, intSize);
        };
    }
}
