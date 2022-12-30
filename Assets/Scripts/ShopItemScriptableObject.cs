using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "Scriptable Objects/New Shop Item", order = 1)]
public class ShopItemScriptableObject : ScriptableObject
{
    public string name;
    public Sprite sprite;
    public int cost;

    public EquipMethod equipMethod;

}
