using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class RotatingVolumeMeshSlicer : MonoBehaviour
{
    private TetMesh tm;
    protected System.Numerics.Vector3 aVertices, bVertices, cVertices;
    protected float a, b, c, d, a1, b1, c1, a2, b2, c2;
    protected System.Numerics.Plane currentPlane;

    // Start is called before the first frame update
    void Start()
    {
        aVertices.X = GameObject.Find("Apoint").transform.position.x;
        aVertices.Y = GameObject.Find("Apoint").transform.position.y;
        aVertices.Z = GameObject.Find("Apoint").transform.position.z;
        bVertices.X = GameObject.Find("Bpoint").transform.position.x;
        bVertices.Y = GameObject.Find("Bpoint").transform.position.y;
        bVertices.Z = GameObject.Find("Bpoint").transform.position.z;
        cVertices.X = GameObject.Find("Cpoint").transform.position.x;
        cVertices.Y = GameObject.Find("Cpoint").transform.position.y;
        cVertices.Z = GameObject.Find("Cpoint").transform.position.z;
        currentPlane = new System.Numerics.Plane();
        currentPlane= System.Numerics.Plane.CreateFromVertices(aVertices, bVertices, cVertices);
        PlaneSlice();

    }

    // Update is called once per frame
    void Update()
    {
        aVertices.X = GameObject.Find("Apoint").transform.position.x;
        aVertices.Y = GameObject.Find("Apoint").transform.position.y;
        aVertices.Z = GameObject.Find("Apoint").transform.position.z;
        bVertices.X = GameObject.Find("Bpoint").transform.position.x;
        bVertices.Y = GameObject.Find("Bpoint").transform.position.y;
        bVertices.Z = GameObject.Find("Bpoint").transform.position.z;
        cVertices.X = GameObject.Find("Cpoint").transform.position.x;
        cVertices.Y = GameObject.Find("Cpoint").transform.position.y;
        cVertices.Z = GameObject.Find("Cpoint").transform.position.z;

        System.Numerics.Plane tmp = System.Numerics.Plane.CreateFromVertices(aVertices, bVertices, cVertices);
        if (!tmp.Equals(currentPlane))
        {
            // Determino i valori di a, b, c, e d in base alla posizione dei 3 punti del piano cartesiano
            a = aVertices.Y * (bVertices.Z - cVertices.Z) + bVertices.Y * (cVertices.Z - aVertices.Z) + cVertices.Y * (aVertices.Z - bVertices.Z);
            b = aVertices.Z * (bVertices.X - cVertices.X) + bVertices.Z * (cVertices.X - aVertices.X) + cVertices.Z * (aVertices.X - bVertices.X);
            c = aVertices.X * (bVertices.Y - cVertices.Y) + bVertices.X * (cVertices.Y - aVertices.Y) + cVertices.X * (aVertices.Y - bVertices.Y);
            d = aVertices.X * (bVertices.Y * cVertices.Z - cVertices.Y * bVertices.Z) + bVertices.X * (cVertices.Y * aVertices.Z - aVertices.Y * cVertices.Z) + cVertices.X * (aVertices.Y * bVertices.Z - bVertices.Y * aVertices.Z);

            // Render
            PlaneSlice();
            currentPlane = System.Numerics.Plane.CreateFromVertices(aVertices, bVertices, cVertices);
        }
    }

    void PlaneSlice()
    {
    if (TryGetComponent<TetMesh>(out tm))
        {
            tm.PlaneSlice(a, b, c, d);
        }
        else
        {
            Debug.LogError("A volume mesh must be attached to this gameObject");
        }
    }
}
