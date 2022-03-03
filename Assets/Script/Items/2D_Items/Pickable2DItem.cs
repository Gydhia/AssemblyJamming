using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Pickable2DItem : MonoBehaviour
{
    public Item item;
    private BoxCollider2D bc;
    public Sprite sprite;
    private SpriteRenderer spriteR;
    public BoxCollider2D Bc => bc;

    void Awake()
    {
        spriteR = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        spriteR.sprite = sprite;
    }
}
