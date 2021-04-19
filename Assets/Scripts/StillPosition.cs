using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StillPosition : MonoBehaviour
{
    private float globalTime = 7;

    // Update is called once per frame
    void Update()
    {
        globalTime -= Time.deltaTime;
        if (globalTime < 0)
            Destroy(this.gameObject);
    }
}
