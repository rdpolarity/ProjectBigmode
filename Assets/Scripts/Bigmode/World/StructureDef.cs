using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Bigmode
{
    public abstract class StructureDef<T>: SerializedScriptableObject
    {
        // Configuration
        [SerializeField] private Vector2Int size = new(5,5);
        [SerializeField, TableMatrix(SquareCells = true)] private T[,] elements = new T[5,5];
        
        // Working vars
        private readonly Dictionary<RotationAction, T[,]> cachedRotations = new();

        private void OnEnable()
        {
            foreach (var rotation in Constants.RotationActions)
            {
                cachedRotations[rotation] = elements.GetRotation(rotation);
            }
        }

        private void OnValidate()
        {
            if (size.x == elements.GetLength(0) && size.y == elements.GetLength(1))
                return;

            if (size.x <= 0 || size.y <= 0)
                return;
            
            var tmpNew = new T[size.x, size.y];

            for (var x = 0; x < size.x && x < elements.GetLength(0); x++)
            {
                for (var y = 0; y < size.y && y < elements.GetLength(1); y++)
                {
                    tmpNew[x, y] = elements[x, y];
                }
            }
            
            elements = tmpNew;
        }
        
        public bool CanPlaceAt(Vector3Int cell, RotationAction rotation)
        {
            var array = cachedRotations[rotation];
            
            for (var x = 0; x < array.GetLength(0); x++)
            {
                for (var y = 0; y < array.GetLength(1); y++)
                {
                    if (array[x, y] == null)
                        continue;
                    
                    var p = cell + new Vector3Int(x, y, 0);
                    
                    if (!CanPlaceAt(p))
                        return false;
                }
            }
            
            return true;
        }
        
        public void PlaceAt(Vector3Int cell, RotationAction rotation)
        {
            var array = cachedRotations[rotation];
            
            for (var x = 0; x < array.GetLength(0); x++)
            {
                for (var y = 0; y < array.GetLength(1); y++)
                {
                    if (array[x, y] == null)
                        continue;
                    
                    var p = cell + new Vector3Int(x, y, 0);
                    PlaceAt(p, array[x, y]);
                }
            }
        }
        
        protected abstract bool CanPlaceAt(Vector3Int pos);
        
        protected abstract void PlaceAt(Vector3Int pos, T item);
    }
}