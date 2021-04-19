using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class MeshCutter : MonoBehaviour
{
    private TetMesh tm;
    public float dim;
    protected System.Numerics.Vector3 aVertices, bVertices, cVertices;
    protected float a, b, c, d;
    protected System.Numerics.Plane currentPlane;   // Evita di dover ricalcolare il taglio ad ogni frame qualora il piano restasse fermo

    // Start is called before the first frame update
    void Start()
    {
        dim = 0.1f;
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
        currentPlane = System.Numerics.Plane.CreateFromVertices(aVertices, bVertices, cVertices);
        PlaneCutter();

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
            PlaneCutter();
            currentPlane = System.Numerics.Plane.CreateFromVertices(aVertices, bVertices, cVertices);
        }
    }

    void PlaneCutter()
    {
        if (transform.parent.TryGetComponent<TetMesh>(out tm))
        {
            tm.PlaneCutter(a, b, c, d, dim, this.gameObject);
        }
        else
        {
            Debug.LogError("A volume mesh must be attached to this gameObject");
        }
    }
}
