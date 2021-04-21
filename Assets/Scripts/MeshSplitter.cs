using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSplitter : MonoBehaviour
{
    private TetMesh tm;
    public float dim;
    public int color;

    protected List<bool> thisVisiblePolys = new List<bool>();
    protected List<Vector3> thisVertices =  new List<Vector3>();
    protected Dictionary<int, int> thisOldNewVertices = new Dictionary<int, int>();

    private GameObject planeCut;

    void Start()
    {
        dim = 0.01f;    // Larghezza del taglio

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.RecalculateBounds();
        this.transform.gameObject.AddComponent<MeshCollider>().convex = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Bullet(Clone)")
        {
            // Aggiorno il punteggio
            if (transform.parent.parent != null)
            {
                switch (this.transform.parent.gameObject.GetComponent<TetMesh>().material.name) {
                    case "TransparentRed":
                        color = 1;
                        break;
                    case "TransparentBlue":
                        color = 2;
                        break;
                    case "TransparentGreen":
                        color = 3;
                        break;
                }
                GameObject.Find("Player").GetComponent<PlayerMovement>().AddPoint(color);
            }

            transform.parent.parent = null;
            transform.parent.GetComponent<TetMesh>().UpdateStatus();
            planeCut = GameObject.Find("PlaneCutter");
            if (transform.parent.GetComponent<TetMesh>().GetStatus() > 15)
                planeCut.transform.position = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            else
                planeCut.transform.position = this.gameObject.transform.position;
            planeCut.transform.rotation = other.transform.rotation;
            planeCut.transform.Rotate(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

            SplitMesh();
        }
        else if(other.gameObject.name != "GeneratorX")
        {
            transform.parent.parent = null;
        }
    }

    void SplitMesh()
    {
        if (transform.parent.TryGetComponent<TetMesh>(out tm))
        {
            tm.SplitMesh(this.gameObject);
        }
        else
        {
            Debug.LogError("A volume mesh must be attached to this gameObject parent");
        }
    }

    public List<bool> GetVisiblePolys()
    {
        return thisVisiblePolys;
    }

    public List<Vector3> GetVertices()
    {
        return thisVertices;
    }

    public Dictionary<int,int> GetOldNewVertices()
    {
        return thisOldNewVertices;
    }
}
