using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private BoxCollider2D cameraBox;
    private Transform player;
    private GameObject currentBoundary;
    public float cameraSmoothSpeed = 10f;

    private void Start()
    {
        cameraBox = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        AspectRatioBoxChange();
        FollowPlayer();
    }

    void AspectRatioBoxChange()
    {
        cameraBox.size = new Vector2(Camera.main.orthographicSize * Camera.main.aspect * 2, Camera.main.orthographicSize * 2);
    }

    void FollowPlayer()
    {
        if (GameObject.Find("Boundary"))
        {
            var boundBox = GameObject.Find("Boundary").GetComponent<BoxCollider2D>().bounds;
            transform.position = new Vector3(Mathf.Clamp(player.position.x, boundBox.min.x + cameraBox.size.x / 2, boundBox.max.x - cameraBox.size.x / 2),
                                             Mathf.Clamp(player.position.y, boundBox.min.y + cameraBox.size.y / 2, boundBox.max.y - cameraBox.size.y / 2),
                                             transform.position.z);
        }
    }
}
