using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicerPlaneMovement : MonoBehaviour
{
    public float rotationSpeed = 50;
    public float speed = 10;
    public float width;

    // Update is called once per frame
    /*void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (Input.GetKey(KeyCode.A))
            transform.position += Vector3.left * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            transform.position += Vector3.right * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Q))
            transform.position += Vector3.up * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.E))
            transform.position += Vector3.down * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
            transform.position += Vector3.forward * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            transform.position += Vector3.back * speed * Time.deltaTime;

        transform.Rotate((Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime), (Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime), 0, Space.World);
    }*/
}
