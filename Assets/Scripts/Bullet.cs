using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string wallTag;
    public string floorTag;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(wallTag) || collision.gameObject.CompareTag(floorTag))
        {
            Destroy(this.gameObject);
        }
    }
}
