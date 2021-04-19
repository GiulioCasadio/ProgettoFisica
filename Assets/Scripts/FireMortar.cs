using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMortar : MonoBehaviour
{
    public int bulletSpeed;
    public Material matApple, matBanana, matPear, matPumpkin;

    private static float timeToFire;

    private static int mortars = 0;
    private int mortarId;

    private void Start()
    {
        if (mortars > 3)
            mortars = 0;
        mortarId = mortars++;
        timeToFire = 3;
    }

    // Update is called once per frame
    void Update()
    {
        timeToFire -= Time.deltaTime;
        // Va triggerato a random
        if (timeToFire < 0 && mortarId == Random.Range(0, 3))
        {
            timeToFire = 3;
            GameObject child = new GameObject("Fruit");
            child.transform.parent = this.gameObject.transform;
            child.gameObject.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
            child.gameObject.transform.localScale = new Vector3(5,5,5);

            switch (Random.Range(0, 4))
            {
                case 0:
                    child.gameObject.AddComponent<InitMesh>().path = "Assets/Mesh/AppleLow.mesh"; 
                    child.gameObject.AddComponent<TetMesh>().material= matApple;
                    break;
                case 1:
                    child.gameObject.AddComponent<InitMesh>().path = "Assets/Mesh/PearLow.mesh"; 
                    child.gameObject.AddComponent<TetMesh>().material = matPear;
                    break;
                case 2:
                    child.gameObject.AddComponent<InitMesh>().path = "Assets/Mesh/BananaLow.mesh"; 
                    child.gameObject.AddComponent<TetMesh>().material = matBanana;
                    break;
                case 3:
                    child.gameObject.AddComponent<InitMesh>().path = "Assets/Mesh/PumpkinLow.mesh"; 
                    child.gameObject.AddComponent<TetMesh>().material = matPumpkin;
                    break;
            }
        }
    }

    public int GetSpeed() { return bulletSpeed; }
}
