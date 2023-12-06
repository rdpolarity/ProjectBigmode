using UnityEngine;
using UnityEngine.Tilemaps;

namespace Bigmode
{
    [CreateAssetMenu(menuName = "BigMode/New tiled structure def", fileName = "New structure", order = 0)]
    public class TiledStructureDef: StructureDef<Tile>
    {
        protected override bool CanPlaceAt(Vector3Int pos)
        {
            return !World.Instance.Tilemap.HasTile(pos);
        }
        
        protected override void PlaceAt(Vector3Int pos, Tile item)
        {
            World.Instance.Tilemap.SetTile(pos, item);
        }
    }
}