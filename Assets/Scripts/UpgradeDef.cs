using System.Collections;
using System.Collections.Generic;
using Bigmode;
using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(menuName = "BigMode/Upgrades", fileName = "New upgrade", order = 0)]
public class UpgradeDef : SerializedScriptableObject
{
    public string Title;
    public Sprite Icon;
    public string Description;
    public IUpgrade UpgradeLogic;
}
