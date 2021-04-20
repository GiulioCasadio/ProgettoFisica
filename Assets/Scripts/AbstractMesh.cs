using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System.IO;
using System.Linq;
using System;

[ExecuteInEditMode]
public class AbstractMesh : MonoBehaviour
{
    protected List<Vector3> vertices;
    protected List<List<int>> polys;
    protected List<List<int>> faces = new List<List<int>>();
    protected List<List<int>> adjP2P;
    protected List<List<int>> adjF2F;
    public Dictionary<int, int> SF2Poly = new Dictionary<int, int>();
    protected List<Vector3> polyCentroids;
    protected List<bool> visiblePolys;
    protected List<bool> childVisiblePolys;
    public string filePath = "";
    protected string _currentPath = "";
    public Material material;
    protected bool meshLoaded = false;
    protected Dictionary<int, int> oldNewVertices;


    //public static float sum = 0;

    public void Clear()
    {
        if (vertices != null)
            vertices.Clear();
        if (polys != null)
            polys.Clear();
        if (faces != null)
            faces.Clear();
        if (adjP2P != null)
            adjP2P.Clear();
        if (adjF2F != null)
            adjF2F.Clear();
        if (SF2Poly != null)
            SF2Poly.Clear();
        if (polyCentroids != null)
            polyCentroids.Clear();
        if (visiblePolys != null)
            visiblePolys.Clear();
    }

    public void LoadMesh()
    {

		string line = "";
        if (!File.Exists(filePath)) {
            meshLoaded = false;
            return;
        }

        Clear();
		StreamReader reader = new StreamReader(filePath);

		while (line != "Vertices")
		{
			line = reader.ReadLine();
		}
        line = reader.ReadLine();
		int numVerts = int.Parse(line);

        vertices.Capacity = numVerts;

        for (int i = 0; i < numVerts; i++)
        {
            line = reader.ReadLine();
            string[] coords = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            float x = (float)double.Parse(coords[0], CultureInfo.InvariantCulture);
            float y = (float)double.Parse(coords[1], CultureInfo.InvariantCulture);
            float z = (float)double.Parse(coords[2], CultureInfo.InvariantCulture);
            vertices.Add(new Vector3(x, y, z));

            oldNewVertices.Add(i, i); // originale -> nuovo
        }
        Normalize();

        while (line != "Hexahedra" && line != "Tetrahedra")
        {
            line = reader.ReadLine();
        }
        line = reader.ReadLine();
        int numPolys = int.Parse(line);
        polys = new List<List<int>>();
        polys.Capacity = numPolys;

        // visiblePolys deve far riferimento a thisVisiblePolys del meshSplitter dell'oggetto in uso
        visiblePolys.Capacity = numPolys;
        polyCentroids = new List<Vector3>();
        polyCentroids.Capacity = numPolys;

        for (int i = 0; i < numPolys; i++)
        {
            polys.Add(new List<int>());

            line = reader.ReadLine();
            string[] indices = line.Split(new[] { ' ', '\t' },
                 System.StringSplitOptions.RemoveEmptyEntries);

            for(int j=0; j < indices.Length - 1; j++)
            {
                string idx = indices[j];
                int index = int.Parse(idx)-1;
                polys[i].Add(index);
            }

            polyCentroids.Add(ComputePolyCentroid());
            visiblePolys.Add(true);


        }

        meshLoaded = true;
        _currentPath = filePath;

    }

    private Vector3 ComputePolyCentroid()
    {
        Vector3 centroid = new Vector3(0.0f,0.0f,0.0f);
        foreach(int idx in polys[polys.Count() - 1])
        {
            centroid += vertices[idx];
        }
        centroid /= polys[polys.Count()-1].Count();

        return centroid;
    }

    protected void Slice(double x, double y, double z)
    {
        int i = 0;
        foreach(Vector3 centroid in polyCentroids)
        {
            if (centroid.x > x || centroid.y > y || centroid.z > z)
            {
                visiblePolys[i] = false;
            }
            else
            {
                visiblePolys[i] = true;
            }
            i++;
        }
    }

    void ResetSlice()
    {
        for(int i=0; i<visiblePolys.Count(); i++)
        {
            visiblePolys[i] = true;
        }
    }

    void polyVisibilitySwitch(int idx, bool isVisible)
    {
        visiblePolys[idx] = isVisible;
    }

    void Normalize()
    {
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;
        float minZ = float.MaxValue;
        float maxZ = float.MinValue;
        Vector3 centroid = new Vector3(0,0,0);

        foreach (Vector3 vert in vertices)
        {
            if(vert.x > maxX) maxX = vert.x;
            if (vert.x < minX) minX = vert.x;
            if (vert.y > maxY) maxY = vert.y;
            if (vert.y < minY) minY = vert.y;
            if (vert.z > maxZ) maxZ = vert.z;
            if (vert.z < minZ) minZ = vert.z;

            centroid += vert;

        }
        centroid /= vertices.Count();

        Vector3 v0 = new Vector3(minX, minY, minZ);
        Vector3 v1 = new Vector3(maxX, maxY, maxZ);

        float distance = Vector3.Distance(v0, v1);

        for (int i=0; i<vertices.Count(); i++)
        {
            vertices[i] -= centroid;
            vertices[i] /= distance;
        }
    }

    protected void PlaneSlice(float a, float b, float c, float d)
    {
        int i = 0;
        foreach (Vector3 centroid in polyCentroids)
        {
            // ricalcolo le coordinate di quel centroid in base alla rotazione
            Vector3 centroidRotated = new Vector3(centroid.x, centroid.y, centroid.z);
            centroidRotated = this.transform.rotation * centroidRotated;

            centroidRotated.x *= this.transform.localScale.x;
            centroidRotated.y *= this.transform.localScale.y;
            centroidRotated.z *= this.transform.localScale.z;
            // Controllo
            if (a * centroidRotated.x + b * centroidRotated.y + c * centroidRotated.z - d > 0)
            {
                visiblePolys[i] = false;
            }
            else
                visiblePolys[i] = true;
            i++;
        }
    }

    protected void PlaneSlice(float a, float b, float c, float d, float dim)
    {
        int i = 0;
        foreach (Vector3 centroid in polyCentroids)
        {
            // ricalcolo le coordinate di quel centroid in base alla rotazione
            Vector3 centroidRotated = new Vector3(centroid.x, centroid.y, centroid.z);
            centroidRotated = this.transform.rotation * centroidRotated;

            centroidRotated.x *= this.transform.localScale.x;
            centroidRotated.y *= this.transform.localScale.y;
            centroidRotated.z *= this.transform.localScale.z;

            // Controllo
            if (a * centroidRotated.x + b * centroidRotated.y + c * centroidRotated.z - d < dim)
            {
                visiblePolys[i] = false;
            }
            else
                visiblePolys[i] = true;
            i++;
        }
    }

    protected void PlaneCutter(float a, float b, float c, float d, float dim)
    {
        int i = 0;

        foreach (Vector3 centroid in polyCentroids)
        {
            // ricalcolo le coordinate di quel centroid in base alla rotazione e alla scala
            Vector3 centroidRotated = new Vector3(centroid.x, centroid.y, centroid.z);
            centroidRotated = this.transform.rotation * (centroidRotated - this.transform.position);
            centroidRotated.x *= this.transform.localScale.x;
            centroidRotated.y *= this.transform.localScale.y;
            centroidRotated.z *= this.transform.localScale.z;

            // ricalcolo le coordinate di quel centroid in base alla posizione
            centroidRotated += this.transform.position;

            // Controllo
            if (a * centroidRotated.x + b * centroidRotated.y + c * centroidRotated.z - d < dim &&
                a * centroidRotated.x + b * centroidRotated.y + c * centroidRotated.z - d > 0)
                visiblePolys[i] = false;
            else
                visiblePolys[i] = true;
            i++;
        }
    }

    protected void SplitMesh(GameObject current)
    {
        GameObject upper = new GameObject("upper");
        upper.transform.parent = this.gameObject.transform;
        GameObject lower = new GameObject("lower");
        lower.transform.parent = this.gameObject.transform;

        // Queste funzioni creano 2 diverse mesh e le assegnano a 2 nuovi oggetti inviati come parametro
        ExportMesh(current, 0, upper);
        ExportMesh(current, 1, lower);


        if (transform.name.Equals("Urn"))
        {
            upper.transform.parent.gameObject.AddComponent<StillPosition>();
        }

        // Aggiungo una forza alle mesh
        lower.GetComponent<Rigidbody>().AddForce(GameObject.Find("Player").transform.forward * 4000); 
        upper.GetComponent<Rigidbody>().AddForce(GameObject.Find("Player").transform.forward * 4000);
        upper.GetComponent<Rigidbody>().mass = lower.GetComponent<Rigidbody>().mass = 0.1f;
    }

    void ExportMesh(GameObject input_mesh, int label_val, GameObject output_mesh)
    {
        System.Numerics.Vector3 aVertices, bVertices, cVertices;
        float a, b, c, d;

        // Setto posizione, rotazione e scala del nuovo oggetto
        output_mesh.transform.SetPositionAndRotation(input_mesh.transform.position, input_mesh.transform.rotation);
        output_mesh.transform.localScale = new Vector3(1, 1, 1);

        // Calcolo piano tagliente
        aVertices.X = GameObject.Find("Apoint").transform.position.x;
        aVertices.Y = GameObject.Find("Apoint").transform.position.y;
        aVertices.Z = GameObject.Find("Apoint").transform.position.z;
        bVertices.X = GameObject.Find("Bpoint").transform.position.x;
        bVertices.Y = GameObject.Find("Bpoint").transform.position.y;
        bVertices.Z = GameObject.Find("Bpoint").transform.position.z;
        cVertices.X = GameObject.Find("Cpoint").transform.position.x;
        cVertices.Y = GameObject.Find("Cpoint").transform.position.y;
        cVertices.Z = GameObject.Find("Cpoint").transform.position.z;

        // Determino i valori di a, b, c, e d in base alla posizione dei 3 punti del piano cartesiano
        a = aVertices.Y * (bVertices.Z - cVertices.Z) + bVertices.Y * (cVertices.Z - aVertices.Z) + cVertices.Y * (aVertices.Z - bVertices.Z);
        b = aVertices.Z * (bVertices.X - cVertices.X) + bVertices.Z * (cVertices.X - aVertices.X) + cVertices.Z * (aVertices.X - bVertices.X);
        c = aVertices.X * (bVertices.Y - cVertices.Y) + bVertices.X * (cVertices.Y - aVertices.Y) + cVertices.X * (aVertices.Y - bVertices.Y);
        d = aVertices.X * (bVertices.Y * cVertices.Z - cVertices.Y * bVertices.Z) + bVertices.X * (cVertices.Y * aVertices.Z - aVertices.Y * cVertices.Z) + cVertices.X * (aVertices.Y * bVertices.Z - bVertices.Y * aVertices.Z);

        // Determino i componenti del nuovo oggetto
        Mesh neWUnityMesh;
        MeshFilter newFilter;
        MeshRenderer newMeshRenderer;

        newFilter = output_mesh.AddComponent<MeshFilter>();
        newMeshRenderer = output_mesh.AddComponent<MeshRenderer>();
        neWUnityMesh = new Mesh();
        newFilter.mesh = neWUnityMesh;
        output_mesh.AddComponent<Rigidbody>().mass = 10;

        // faccio riferimento alla lista visiblePolys dell'oggetto inserendo al suo interno lo script MeshSplitter. Permetterà future suddivisioni
        output_mesh.AddComponent<MeshSplitter>();

        childVisiblePolys = output_mesh.GetComponent<MeshSplitter>().GetVisiblePolys();
        childVisiblePolys.Clear();

        vertices = output_mesh.GetComponent<MeshSplitter>().GetVertices();
        oldNewVertices = output_mesh.GetComponent<MeshSplitter>().GetOldNewVertices();

        // Aggiorno visiblePolys dato che ho già scompattato la mesh in precedenza
        visiblePolys = input_mesh.GetComponent<MeshSplitter>().GetVisiblePolys();

        // Calcolo quali poligoni rendere visibili
        int i = 0, aux = 0;

        bool emptyObj = true;   // Verificherà se l'oggetto deve essere o meno cancellato
        
        foreach (Vector3 centroid in polyCentroids)
        {
            // ricalcolo le coordinate di quel centroid in base alla rotazione e alla scala
            Vector3 centroidRotated = new Vector3(centroid.x, centroid.y, centroid.z);
            centroidRotated = (input_mesh.transform.rotation) * centroidRotated;
            
            centroidRotated.x *= this.transform.localScale.x;
            centroidRotated.y *= this.transform.localScale.y;
            centroidRotated.z *= this.transform.localScale.z;

            // ricalcolo le coordinate di quel centroid in base alla posizione
            centroidRotated += input_mesh.transform.position;

            // In base alla posizione del nuovo oggetto e rispetto al taglio rendo visibili determinati poligoni
            if (label_val == 0) { // mesh superiore
                if (a * centroidRotated.x + b * centroidRotated.y + c * centroidRotated.z - d > - GameObject.Find("PlaneCutter").GetComponent<SlicerPlaneMovement>().width)
                {
                    childVisiblePolys.Add(false); 
                }
                else if (visiblePolys[i])
                {
                    childVisiblePolys.Add(true);
                    emptyObj = false;

                    // Aggiungo i vertici di quel poly
                    foreach (int vert in polys[i])
                    {
                        // Se non è già dentro al dizionario
                        if (!oldNewVertices.ContainsKey(vert))
                        {
                            vertices.Add(input_mesh.GetComponent<MeshSplitter>().GetVertices()[input_mesh.GetComponent<MeshSplitter>().GetOldNewVertices()[vert]]);
                            oldNewVertices.Add(vert, aux);
                            aux++;
                        }
                    }
                }
                else
                    childVisiblePolys.Add(false);
                i++;
            }
            else  // mesh inferiore
            {
                if (a * centroidRotated.x + b * centroidRotated.y + c * centroidRotated.z - d < GameObject.Find("PlaneCutter").GetComponent<SlicerPlaneMovement>().width)
                {
                    childVisiblePolys.Add(false);
                }
                else if (visiblePolys[i])
                {
                    childVisiblePolys.Add(true);
                    emptyObj = false;

                    // Aggiungo i vertici di quel poly
                    foreach (int vert in polys[i])
                    {
                        // Se non è già dentro al dizionario
                        if (!oldNewVertices.ContainsKey(vert))
                        {
                            vertices.Add(input_mesh.GetComponent<MeshSplitter>().GetVertices()[input_mesh.GetComponent<MeshSplitter>().GetOldNewVertices()[vert]]);
                            oldNewVertices.Add(vert, aux);
                            aux++;
                        }
                    }
                }
                else
                    childVisiblePolys.Add(false);
                i++;
            }
        }
        
        // Se l'oggetto risulterà invisibile
        if (emptyObj)
        {
            Destroy(output_mesh);
            return;
        }

        // Renderizzo il nuovo oggetto
        neWUnityMesh.vertices = vertices.ToArray();

        List<int> triangles = new List<int>();

        int idx = 0;
        int visibleIdx = 0;
        foreach (List<int> face in faces)
        {
            int polyIdx = idx / 4;
            if (childVisiblePolys[polyIdx])
            {
                if (adjF2F[idx].Count() == 0 || !childVisiblePolys[adjF2F[idx][0] / 4])
                {
                    foreach (int f in face)
                    {
                        triangles.Add(oldNewVertices[f]);
                    }
                    SF2Poly[visibleIdx] = polyIdx;
                    visibleIdx++;
                }
            }
            idx++;
        }
        
        neWUnityMesh.triangles = triangles.ToArray();
        neWUnityMesh.RecalculateNormals();
        neWUnityMesh.RecalculateBounds();

        newMeshRenderer.material = material == null ? new Material(Shader.Find("Diffuse")) : material;
    }
}
