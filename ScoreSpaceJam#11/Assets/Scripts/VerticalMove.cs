using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMove : MonoBehaviour
{
    // Start is called before the first frame update
    public static float GameRate = 15;
    private void Start()
    {
        GameRate = 15f;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += new Vector3(0f, GameRate * Time.deltaTime, 0);
    }
}
