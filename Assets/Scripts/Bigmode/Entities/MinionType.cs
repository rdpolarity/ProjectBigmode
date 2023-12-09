using Sirenix.OdinInspector;
using UnityEngine;

namespace Bigmode
{
    [CreateAssetMenu(menuName = "BigMode/New MinionType", fileName = "New Minion", order = 0)]
    public class MinionType : SerializedScriptableObject
    {
         public int cost;
         public new string name;
         public string description;
         public GameObject prefab;
         public Color color;
    }
}