using System.Collections.Generic;
using UnityEngine;

namespace Bigmode
{
    public static class Pool
    {
        private static readonly Dictionary<GameObject, Stack<GameObject>> pool = new();
        
        public static GameObject Get(GameObject prefab, Vector3Int cell, Transform parent)
        {
            if (!pool.TryGetValue(prefab, out var stack) || stack.Count == 0)
            {
                var created = Object.Instantiate(prefab, World.Instance.Grid.CellToWorld(cell), Quaternion.identity, parent);
                
                var comp = created.GetComponent<PooledObject>();
                comp.prefab = prefab;
                comp.cell = cell;
                
                return created;
            }
            
            var go = stack.Pop();
            
            var pooled = go.GetComponent<PooledObject>();
            
            pooled.cell = cell;
            go.transform.position = World.Instance.Grid.CellToWorld(cell);
            
            go.SetActive(true);
            
            return go;
        }
        
        public static void Return(PooledObject pooled)
        {
            if (!pool.TryGetValue(pooled.prefab, out var stack))
                pool[pooled.prefab] = stack = new Stack<GameObject>();
            
            var go = pooled.gameObject;
            go.SetActive(false);
            stack.Push(go);
            
            World.Instance.ClearObjectAt(pooled.cell);
        }
        
        public static void Clear()
        {
            foreach (var (_, stack) in pool)
            {
                foreach (var go in stack)
                {
                    Object.Destroy(go);
                }
                
                stack.Clear();
            }
            
            pool.Clear();
        }
    }
}
