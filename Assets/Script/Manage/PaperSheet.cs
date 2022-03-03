using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Code
{
    first = 2,
    second = 5,
    third = 1,
    fourth = 3
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/PaperSheet")]
public class PaperSheet : Item
{
    public Code code;
}
