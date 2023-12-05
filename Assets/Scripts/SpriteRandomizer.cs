using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteRandomizer : MonoBehaviour
{
    private Vector2 lastPosition;
    private float lastPerlinValue;
    private SpriteRenderer spriteRenderer; // Sprite renderer to apply sprites to

    [SerializeField]
    private float perlinNoiseScale = 0.1f; // Scaling factor for the Perlin noise input

    [SerializeField]
    private List<Sprite> sprites = new List<Sprite>(); // List of sprites to choose from

    void Start()
    {
        RandomizeSprite();
    }

    void Update()
    {
        Vector2 currentPosition = (Vector2)transform.position;
        if (currentPosition != lastPosition)
        {
            RandomizeSprite();
            lastPosition = currentPosition;
        }
        // or if perlin noise changed
        float currentPerlinValue = perlinNoiseScale;
        if (currentPerlinValue != lastPerlinValue)
        {
            RandomizeSprite();
            lastPerlinValue = currentPerlinValue;
        }
    }

    void RandomizeSprite()
    {
        if (sprites.Count == 0) return;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (!spriteRenderer) return;
        // Example of previewing sprite placement
        Vector2 position = (Vector2)transform.position;
        Sprite chosenSprite = GetSpriteForPosition(position);
        spriteRenderer.sprite = chosenSprite;
    }

    private Sprite GetSpriteForPosition(Vector2 position)
    {
        // Adjust these values to change the noise characteristics
        float perlinValue = Mathf.PerlinNoise(position.x * perlinNoiseScale, position.y * perlinNoiseScale);

        // Map the Perlin noise value to a sprite index
        int spriteIndex = Mathf.FloorToInt(perlinValue * sprites.Count);

        // Ensure the index is within the bounds of the sprite list
        spriteIndex = Mathf.Clamp(spriteIndex, 0, sprites.Count - 1);

        return sprites[spriteIndex];
    }
}
