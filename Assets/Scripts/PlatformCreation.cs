using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCreation : MonoBehaviour
{
    public GameObject platPrefab;
    private float timeToFire = 0.8f;

    // Update is called once per frame
    void Update()
    {
        timeToFire -= Time.deltaTime;

        if (timeToFire < 0)
        {
            timeToFire = 0.8f;
            GameObject plat = Instantiate(platPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
            plat.transform.parent = this.gameObject.transform;
        }
    }
}
