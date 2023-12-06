using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Bigmode
{
    public class World : Singleton<World>
    {
        // Configuration
        [SerializeField, Title("Assignments")] private Transform gameObjectParent;
        
        [SerializeField, PropertySpace, Title("World")] private List<Tile> defaultTiles;
        [SerializeField, Tooltip("Amount of tiles from center of screen to edge")] private Vector2Int visibleTiles;

        // Working vars
        private Grid grid;
        private Tilemap tilemap;
        private Vector3Int lastPos;
        
        private List<TiledStructureDef> tiledStructures;
        private List<GameObjectStructureDef> gameObjectStructures;
        private readonly Dictionary<Vector3Int, GameObject> spawnedObjects = new(250);
        private readonly Dictionary<GameObject, Transform> prefabParents = new();
        
        // Constants
        private const int TiledPattern = 8;
        private const int GameObjectPattern = 8;
        private const int GameObjectPatternOffset = 3;
        private const float PlaceChance = 0.75f;

        // Properties
        public Grid Grid => grid;
        
        public Tilemap Tilemap => tilemap;

        public static Vector3Int Cell(Vector3 pos) => Instance.grid.WorldToCell(pos);
        
        private BoundsInt VisibleBounds => GetVisibleBoundsAbout(PlayerController.Instance.Cell, visibleTiles);

        private BoundsInt StructuresBounds => GetVisibleBoundsAbout(PlayerController.Instance.Cell, visibleTiles + new Vector2Int(10, 5));
        
        private BoundsInt UnloadBounds => GetVisibleBoundsAbout(PlayerController.Instance.Cell, visibleTiles + new Vector2Int(15, 10));
        
        protected override void Awake()
        {
            base.Awake();
            
            grid = FindObjectOfType<Grid>();
            tilemap = FindObjectOfType<Tilemap>();
            
            tiledStructures = Assets.GetAll<TiledStructureDef>(Constants.DefTiledStructure);
            gameObjectStructures = Assets.GetAll<GameObjectStructureDef>(Constants.DefGameObjectStructure);
            
            foreach (var cell in GetVisibleBoundsAbout(Vector3Int.zero, visibleTiles * 2).allPositionsWithin)
            {
                TryGenerateTiledStructure(cell);
                TryGenerateGameObjectStructure(cell);
            }
            
            foreach (var cell in GetVisibleBoundsAbout(Vector3Int.zero, visibleTiles * 2).allPositionsWithin)
            {
                if (!tilemap.HasTile(cell))
                    tilemap.SetTile(cell, defaultTiles.RandomElement());
            }
        }
        
        private void FixedUpdate()
        {
            foreach (var cell in VisibleBounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(cell))
                    tilemap.SetTile(cell, defaultTiles.RandomElement());
            }
            
            if (lastPos != PlayerController.Instance.Cell)
                UpdateCells();
            
            lastPos = PlayerController.Instance.Cell;
        }

        public bool ObjectAt(Vector3Int cell) => spawnedObjects.ContainsKey(cell);
        
        public void SpawnAt(Vector3Int cell, GameObject go)
        {
            if (spawnedObjects.ContainsKey(cell))
            {
                Debug.LogError($"Attempted to spawn game object ({go.name}) on occupied cell {cell}");
                return;
            }
            
            if (!prefabParents.TryGetValue(go, out var parent))
            {
                prefabParents[go] = parent = new GameObject(go.name).transform;
                parent.SetParent(gameObjectParent);
            }
            
            spawnedObjects[cell] = Pool.Get(go, cell, parent);
        }

        public void ClearObjectAt(Vector3Int cell)
        {
            spawnedObjects.Remove(cell);
        }
        
        private static BoundsInt GetVisibleBoundsAbout(Vector3Int p, Vector2Int size)
        {
            return new BoundsInt(p - new Vector3Int(size.x, size.y, 0), 
                new Vector3Int(size.x * 2, size.y * 2, 1));
        }
        
        private void UpdateCells()
        {
            UnloadGameObjects();
            
            foreach (var cell in StructuresBounds.EdgeCells())
            {
                TryGenerateTiledStructure(cell);
                TryGenerateGameObjectStructure(cell);
            }
        }

        private static readonly HashSet<GameObject> toUnload = new();
        
        private void UnloadGameObjects()
        {
            var unload = UnloadBounds;

            foreach (var (cell, go) in spawnedObjects)
            {
                if (!unload.Contains(cell))
                    toUnload.Add(go);
            }

            foreach (var go in toUnload)
            {
                go.SetActive(false);
            }

            toUnload.Clear();
        }

        private void TryGenerateTiledStructure(Vector3Int cell)
        {
            if (cell.x % TiledPattern != 0 || cell.y % TiledPattern != 0) return;

            using (Rand.State(cell.GetHashCode()))
            {
                if (!Rand.Chance(PlaceChance)) return;

                var def = tiledStructures.RandomElement();
                var p = cell + Rand.Offset(5);
                var rot = Rand.RotationAction;

                if (def.CanPlaceAt(p, rot))
                    def.PlaceAt(p, rot);
            }
        }

        private void TryGenerateGameObjectStructure(Vector3Int cell)
        {
            if ((cell.x + GameObjectPatternOffset) % GameObjectPattern != 0 || 
                (cell.y + GameObjectPatternOffset) % GameObjectPattern != 0) return;

            using (Rand.State(cell.GetHashCode()))
            {
                if (!Rand.Chance(PlaceChance)) return;

                var def = gameObjectStructures.RandomElement();
                var p = cell + Rand.Offset(5);
                var rot = Rand.RotationAction;
                
                if (def.CanPlaceAt(p, rot))
                    def.PlaceAt(p, rot);
            }
        }
    }
}