using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    public float rayLenght;

    // Start is called before the first frame update
    void Start()
    {
        rayLenght = 4.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayLenght, Color.red, 0.5f);

    }
}
