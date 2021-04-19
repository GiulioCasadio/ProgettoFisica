using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitMesh : MonoBehaviour
{
    public string path;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<TetMesh>().filePath = path;
    }
}