using System.Collections;
using UnityEngine;

public class SpriteFlasher : MonoBehaviour
{
    [SerializeField] private float overlayTime = 0.5f;

    private SpriteRenderer _spriteRenderer;
    private Material _material;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _material = _spriteRenderer.material;
    }

    public void Reset()
    {
        SetColor(Color.white);
        SetOverlayAmount(0);
    }

    public void Flash(Color color)
    {
        if (isActiveAndEnabled) StartCoroutine(TriggerFlash(color));
    }

    private void SetColor(Color color)
    {
        _material.SetColor("_OverlayColor", color);
    }

    private void SetOverlayAmount(float amount)
    {
        _material.SetFloat("_OverlayAmount", amount);
    }

    private IEnumerator TriggerFlash(Color color)
    {
        SetColor(color);

        float currentOverlayAmount = 0;
        float elapsedTime = 0;
        while (elapsedTime < overlayTime)
        {
            elapsedTime += Time.deltaTime;
            currentOverlayAmount = Mathf.Lerp(1, 0, elapsedTime / overlayTime);
            SetOverlayAmount(currentOverlayAmount);
            yield return null;
        }
    }
}
