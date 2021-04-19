using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class VolumeMeshSlicer : MonoBehaviour
{
    [Range(-1.0f, 1.0f)]
    public float x=1.0f;
    [Range(-1.0f, 1.0f)]
    public float y = 1.0f;
    [Range(-1.0f, 1.0f)]
    public float z = 1.0f;
    private Vector3 currentSlice;
    public bool invertX;
    public bool invertY;
    public bool invertZ;


    private TetMesh tm;
    // Start is called before the first frame update
    void Start()
    {
        x = 1.0f;
        y = 1.0f;
        z = 1.0f;
        currentSlice = new Vector3(1.0f, 1.0f, 1.0f);
        Slice();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tmp = new Vector3(x, y, z);
        if(tmp != currentSlice)
        {
            Slice();
            currentSlice.x = x;
            currentSlice.y = y;
            currentSlice.z = z;
        }
    }

    void Slice()
    {
    if (TryGetComponent<TetMesh>(out tm))
        {
            tm.Slice(x, y, z);

        }
        else
        {
            Debug.LogError("A volume mesh must be attached to this gameObject");

        }
    }
}
