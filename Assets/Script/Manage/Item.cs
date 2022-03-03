using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    PaperSheet,
    Key,
    Sword,
    CloudInABottle
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item", order = 1)]
public class Item : ScriptableObject
{
    public string name = "New Item";
    public Sprite icon;
    public ItemType type;
}
