using System.Collections.Generic;
using UnityEngine;

namespace Bigmode
{
    [ExecuteInEditMode]
    public class SpriteRandomizer : MonoBehaviour
    {
        private Vector2 lastPosition;
        private float lastPerlinValue;
        private SpriteRenderer spriteRenderer; // Sprite renderer to apply sprites to
        
        [SerializeField]
        private float perlinNoiseScale = 0.1f; // Scaling factor for the Perlin noise input
        
        [SerializeField]
        private List<Sprite> sprites = new(); // List of sprites to choose from
        
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        private void OnEnable()
        {
            RandomizeSprite();
        }
        
        private void RandomizeSprite()
        {
            if (sprites.Count == 0) return;
            
            // Example of previewing sprite placement
            var position = (Vector2)transform.position;
            
            // var chosenSprite = GetSpriteForPosition(position);
            var chosenSprite = sprites[Random.Range(0, sprites.Count)];
            spriteRenderer.sprite = chosenSprite;
        }
        
        private Sprite GetSpriteForPosition(Vector2 position)
        {
            // Adjust these values to change the noise characteristics
            var perlinValue = Mathf.PerlinNoise(position.x * perlinNoiseScale, position.y * perlinNoiseScale);
            
            // Map the Perlin noise value to a sprite index
            var spriteIndex = Mathf.FloorToInt(perlinValue * sprites.Count);
            
            // Ensure the index is within the bounds of the sprite list
            spriteIndex = Mathf.Clamp(spriteIndex, 0, sprites.Count - 1);
            
            return sprites[spriteIndex];
        }
    }
}