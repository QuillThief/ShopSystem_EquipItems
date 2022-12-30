using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    private List<string> inventory = new List<string>();

    void Start() => Instance = this;

    public void AddToInventory(string name) => inventory.Add(name);

    public void RemoveFromInventory(string name) => inventory.Remove(name);

    public bool CheckItemIfExist(string name)
    {
        if (inventory.Contains(name)) return true;
        else return false;
    }

    public bool IsInventoryEmpty()
    {
        if (inventory.Count == 0) return true;
        else return false;
    }

}
