using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(menuName = "BigMode/Upgrades", fileName = "New upgrade", order = 0)]
public class Upgrade : SerializedScriptableObject
{
    public string Title;
    public Sprite Icon;
    public string Description;
}
