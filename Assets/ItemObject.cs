using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemDate item;
    public SpriteRenderer sr;
    private void OnValidate()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = item.image;
        gameObject.name = "itemObject---" + item.name;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Player>() != null)
        {
            Inventory.instance.AddItem(item);
            Destroy(gameObject);
        }
    }
}
