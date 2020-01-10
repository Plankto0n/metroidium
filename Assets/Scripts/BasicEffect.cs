using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEffect : MonoBehaviour
{
    public int rotSpeedUp = 5;
    public int rotSpeedSide = 5;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotSpeedUp * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.right, rotSpeedSide * Time.deltaTime, Space.World);
    }
}
