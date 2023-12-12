using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleCanvasVisibility : MonoBehaviour
{
    [SerializeField] private Canvas dontHide = null;

    private void Start()
    {
        var toggle = GetComponent<Toggle>();
        var canvases = FindObjectsOfType<Canvas>();

        toggle.onValueChanged.AddListener(show =>
        {
            foreach (var canvas in canvases)
            {
                if (canvas == dontHide) continue;
            
                canvas.gameObject.SetActive(show);
            }
        });
    }
}
