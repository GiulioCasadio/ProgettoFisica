using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveD : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;

    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "GeneratorX")
        {
            Destroy(this.gameObject);
        }
    }
}