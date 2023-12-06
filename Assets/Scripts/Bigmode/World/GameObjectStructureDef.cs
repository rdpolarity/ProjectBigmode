using UnityEngine;

namespace Bigmode
{
    [CreateAssetMenu(menuName = "BigMode/New game object structure def", fileName = "New structure", order = 0)]
    public class GameObjectStructureDef: StructureDef<GameObject>
    {
        protected override bool CanPlaceAt(Vector3Int pos)
        {
            return !World.Instance.ObjectAt(pos);
        }
        
        protected override void PlaceAt(Vector3Int pos, GameObject item)
        {
            World.Instance.SpawnAt(pos, item);
        }
    }
}