using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum EquipMethod { Add, Replace }

public class Equip : MonoBehaviour
{
    public static Equip Instance { get; private set; }
    [SerializeField] List<EquipData> equipData;

    void Start() => Instance = this;

    public void EquipPurchasedItem(string itemName)
    {
        EquipData _equipData = equipData[equipData.FindIndex(x => x.name == itemName)];
        EquipMethod equipMethod = ShopManager.Instance.GetItemEquipMethod(itemName);

        if (equipMethod == EquipMethod.Add)
        {
            foreach (var item in _equipData.enableOnEquip) item.enabled = true;
        }
        else if (equipMethod == EquipMethod.Replace)
        {
            foreach (var item in _equipData.replaceMethodData)
            {
                item.spriteRenderer.sprite = item.itemSprite;
            }
        }
    }

    public void UnequipSoldItem(string itemName)
    {
        EquipData _equipData = equipData[equipData.FindIndex(x => x.name == itemName)];
        EquipMethod equipMethod = ShopManager.Instance.GetItemEquipMethod(itemName);

        if (equipMethod == EquipMethod.Add)
        {
            foreach (var item in _equipData.enableOnEquip) item.enabled = false;
        }
        else if (equipMethod == EquipMethod.Replace)
        {
            foreach (var item in _equipData.replaceMethodData)
            {
                item.spriteRenderer.sprite = item.originalSprite;
            }
        }
    }
}

[System.Serializable]
public class EquipData
{
    public string name;

    //EquipMethod.Add (can be null)
    public List<SpriteRenderer> enableOnEquip;

    //EquipMethod.Replace (can be null)
    public List<ReplaceMethodData> replaceMethodData;   
}

[System.Serializable]
public class ReplaceMethodData
{
    public SpriteRenderer spriteRenderer;
    public Sprite originalSprite;
    public Sprite itemSprite;
}
