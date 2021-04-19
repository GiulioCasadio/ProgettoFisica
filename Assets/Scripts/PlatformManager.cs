using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public Material redMat, greenMat, blueMat;

    private void Start()
    {
        if(Random.Range(0, 3) == 0)
        {
            CreateUrn();
        }
        
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    private void CreateUrn()
    {
        GameObject child = new GameObject("Urn");
        child.transform.parent = this.gameObject.transform;
        child.gameObject.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
        child.gameObject.transform.position += new Vector3(0, 0, 2);
        child.gameObject.transform.localScale = new Vector3(0.07f, 0.07f, 0.25f);
        child.gameObject.AddComponent<InitMesh>().path = "Assets/VolumetricMeshes/Urn.mesh";

        switch (Random.Range(0, 3)){
            case 0:
                child.gameObject.AddComponent<TetMesh>().material = redMat;
                break;
            case 1:
                child.gameObject.AddComponent<TetMesh>().material = blueMat;
                break;
            case 2:
                child.gameObject.AddComponent<TetMesh>().material = greenMat;
                break;
        }
        
    }
}
