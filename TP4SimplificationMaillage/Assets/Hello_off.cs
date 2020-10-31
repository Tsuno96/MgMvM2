using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Dynamic;
using System.Linq;

public class Hello_off : MonoBehaviour
{

    public string nameObject;
    public int nSommet;
    public int nFacette;
    public int nArrete;

    public GameObject cube;
    public List<GameObject> cubes;

    public Vector3[] vertices;
    public Vector3[] normales;
    public int[] triangles;
    public Material mat;
    float sommeX, sommeY, sommeZ;
    float minX, minY, minZ, maxX, maxY, maxZ;

    public GameObject[,,] argo;
    public int dimCubes;
    public Dictionary<GameObject, List<Vector3>> dictCubeVertices;
    public Dictionary<int, int> dictVerticeIndex;
    public List<int> lst_triangles;

    void ReadOff()
    {
        string path = "Assets/Off/" + nameObject + ".off";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        reader.ReadLine();
        string[] Count =  reader.ReadLine().Split(' ');
        nSommet = int.Parse(Count[0]);
        nFacette = int.Parse(Count[1]);
        nArrete = int.Parse(Count[2]);


        vertices = new Vector3[nSommet];
        triangles = new int[nFacette * 3];
        normales = new Vector3[nSommet];
        for(int i = 0; i< normales.Length; i++)
        {
            normales[i] = Vector3.zero;
        }

        for(int l =0;l<nSommet;l++) 
        {
            string[] v = reader.ReadLine().Split(' ');
            float x = float.Parse(v[0], CultureInfo.InvariantCulture);
            float y = float.Parse(v[1], CultureInfo.InvariantCulture);
            float z = float.Parse(v[2], CultureInfo.InvariantCulture);
            sommeX += x;
            sommeY += y;
            sommeZ += z;

            if(x < minX)
            {
                minX = x;
            }
            if(y < minY)
            {
                minY = y;
            }
            if(z < minZ)
            {
                minZ = z;
            }

            if(x > maxX)
            {
                maxX = x;
            }
            if(y > maxY)
            {
                maxY = y;
            }
            if(z > maxZ)
            {
                maxZ = z;
            }

            vertices[l] = new Vector3(x, y,z);
        }
        int k = 0;
        for(int l = 0; l<nFacette;l++)
        { 
            string[] f = reader.ReadLine().Split(' ');
            triangles[k] = int.Parse(f[1]);
            triangles[k+1] = int.Parse(f[2]);
            triangles[k+2] = int.Parse(f[3]);
            
            k += 3;
        }
        
        reader.Close();
    }

    // Start is called before the first frame update
    void Start()
    {
        argo = new GameObject[dimCubes, dimCubes, dimCubes];
        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        gameObject.AddComponent<MeshRenderer>();

        sommeX = sommeY = sommeZ = 0;
        minX = minY = minZ = Mathf.Infinity;
        maxX = maxY = maxZ = Mathf.NegativeInfinity;

        ReadOff();

        sommeX /= nSommet;
        sommeY /= nSommet;
        sommeZ /= nSommet;

        for(int v = 0; v <vertices.Length; v++)
        {
            vertices[v] -= new Vector3(sommeX, sommeY, sommeZ);
        }

        float maxCoord = Mathf.Max(Mathf.Abs(maxZ - minZ), Mathf.Abs(maxY - minY), Mathf.Abs(maxX - minX));

        for (int v = 0; v < vertices.Length; v++)
        { 
            vertices[v] = new Vector3(vertices[v].x/maxCoord, vertices[v].y/maxCoord, vertices[v].z/maxCoord);
        }

        Mesh msh = new Mesh();                          // Création et remplissage du Mesh

        msh.vertices = vertices;
        msh.triangles = triangles;
        calcNormal();
        msh.normals = normales;
        //msh.RecalculateNormals();
        
        gameObject.GetComponent<MeshFilter>().mesh = msh;           // Remplissage du Mesh et ajout du matériel
        gameObject.GetComponent<MeshRenderer>().material = mat;

        CreateBox();

    }

    void calcNormal()
    {
        for(int i = 0; i<nSommet; i++)
        {
            normales[i] = Vector3.zero;
        }

        int k = 0;
        for (int i = 0; i < nFacette; i++)
        {
            Vector3 p1 = vertices[triangles[k + 1]] - vertices[triangles[k]];
            Vector3 p2 = vertices[triangles[k + 2]] - vertices[triangles[k]];

            Vector3 n = Vector3.Cross(p1, p2);

            normales[triangles[k]] += n;
            normales[triangles[k + 1]] += n;
            normales[triangles[k + 2]] += n;

            k += 3;
        }

        for (int i = 0; i < nSommet; i++)
        {
            normales[i] = Vector3.Normalize(normales[i]);
        }


    }

    void Exportoff()
    {
        string path = "Assets/Off/" + nameObject + "newMesh.off";


        using (StreamWriter sw = File.CreateText(path))
        {
            sw.WriteLine("OFF");
            sw.WriteLine(nSommet + " " + nFacette + " "+nArrete);
            foreach(Vector3 v in vertices)
            {
                string line = v.x + " " + v.y + " " + v.z;
                line = line.Replace(",", ".");

                sw.WriteLine(line);
            }

            int k = 0;
            for( int i = 0; i<nFacette; i++)
            {

                sw.WriteLine("3 "+ triangles[k]+ " " + triangles[k+1] + " " + triangles[k+2]);
                k += 3;
            }
            

        }

    }
    void CreateBox()
    {
        GetMaxMin();
        float offset = 0.1f;
        Vector3 min = new Vector3(minX - offset, minY - offset, minZ - offset) ;
        Vector3 max = new Vector3(maxX + offset, maxY + offset, maxZ + offset);
        //Vector3 centerBox = min + (((max - min) / 2));
        Vector3 centerBox = gameObject.GetComponent<MeshFilter>().mesh.bounds.center;
        var GOmax = Instantiate(cube, max, Quaternion.identity);
        GOmax.name = "Max";
        var GOmin = Instantiate(cube, min, Quaternion.identity);
        GOmin.name = "Min";
        GameObject c = Instantiate(cube, centerBox, Quaternion.identity);
        c.transform.localScale = new Vector3(max.x - min.x, max.y - min.y, max.z - min.z);
        c.name = "Cube0";
        cubes.Add(c);
        
    }


    void GetMaxMin()
    {
        foreach(Vector3 vertices in gameObject.GetComponent<MeshFilter>().mesh.vertices)
        {
            if (vertices.x < minX)
            {
                minX = vertices.x;
            }
            if (vertices.y < minY)
            {
                minY = vertices.y;
            }
            if (vertices.z < minZ)
            {
                minZ = vertices.z;
            }

            if (vertices.x > maxX)
            {
                maxX = vertices.x;
            }
            if (vertices.y > maxY)
            {
                maxY = vertices.y;
            }
            if (vertices.z > maxZ)
            {
                maxZ = vertices.z;
            }
        }
    }

    public void createGrid()
    {

        Vector3 v3Center = cubes[0].transform.position;
        for (var i = 0; i < dimCubes; i++)
        {
            for (var j = 0; j < dimCubes; j++)
            {
                for (var k = 0; k < dimCubes; k++)
                {
                    float x = (float)(v3Center.x - cubes[0].transform.localScale.x / 2 + (i + 0.5) * cubes[0].transform.localScale.x / dimCubes);
                    float y = (float)(v3Center.y + cubes[0].transform.localScale.y / 2 - (j + 0.5) * cubes[0].transform.localScale.y / dimCubes);
                    float z = (float)(v3Center.z - cubes[0].transform.localScale.z / 2 + (k + 0.5) * cubes[0].transform.localScale.z / dimCubes);

                    argo[i, j, k] = Instantiate(cube, new Vector3(x, y, z), Quaternion.identity);
                    argo[i, j, k].transform.localScale = cubes[0].transform.localScale / dimCubes;
                    argo[i, j, k].name = "Cube" + i + j + k;

                    cubes.Add(argo[i, j, k]);
                }
            }
        }
        cubes[0].SetActive(false);
        cubes.Remove(cubes[0]);
        SetBoxVertices();
    }

    // Update is called once per frame
    void Update()
    {

        gameObject.GetComponent<MeshFilter>().mesh.RecalculateNormals();

        if(Input.GetKeyDown("g"))
        {
            createGrid();
        }

    }

    void SetBoxVertices()
    {
        dictCubeVertices = new Dictionary<GameObject, List<Vector3>>();
        dictVerticeIndex = new Dictionary<int, int>();
        Debug.Log(cubes.Count);
        int indexCell = 0;
        foreach (GameObject cube in cubes)
        {
            List<Vector3> vertices = new List<Vector3>();
            Vector3 somme = Vector3.zero;
            int indexVertice = 0;

            foreach (Vector3 v in GetComponent<MeshFilter>().mesh.vertices)
            {
                if(CheckVerticeInBox(cube,v))
                {
                    somme += v;
                    vertices.Add(v);
                    dictVerticeIndex.Add(indexVertice, indexCell);
                }
                indexVertice++;
            }

            if(vertices.Count > 0)
            {
                cube.GetComponent<CubeController>().moyenne = somme / vertices.Count;
                cube.GetComponent<CubeController>().lstvec3_Vertices = vertices;
                dictCubeVertices.Add(cube, vertices);
                indexCell++;
            }
            else
            {
                cube.GetComponent<Renderer>().enabled = false;
            }

            

        }

        CreateNewMesh();

    }

    bool CheckVerticeInBox(GameObject c, Vector3 v)
    {
        bool inside = false;
        List<Vector3> MaxMin = GetMaxMinCube(c);
        Vector3 max = MaxMin[0];
        Vector3 min = MaxMin[1];

        if(v.x < max.x && v.y < max.y && v.z < max.z)
        {
            if (v.x > min.x && v.y > min.y && v.z > min.z)
            {
                inside = true;
            }
        }
        
        return inside;
    }

    List<Vector3> GetMaxMinCube(GameObject c)
    {       
        Vector3 ccenter = c.transform.position;
        Vector3 clocscale = c.transform.localScale/2;
        Vector3 max = ccenter + clocscale;
        Vector3 min = ccenter - clocscale;
        List<Vector3> MaxMin = new List<Vector3>();
        MaxMin.Add(max);
        MaxMin.Add(min);
        /*Debug.Log("Center"+ccenter);
        Debug.Log("Scale"+clocscale);
        Debug.Log(max);
        Debug.Log(min);*/
        return MaxMin;
    }

    void CreateNewMesh()
    {
        Mesh msh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        
        foreach(GameObject cube in cubes)
        {
            if(cube.GetComponent<CubeController>().lstvec3_Vertices.Count > 0)
            {
                vertices.Add(cube.GetComponent<CubeController>().moyenne);
            }
        }

        msh.vertices = vertices.ToArray();

        lst_triangles = new List<int>();
        
        foreach(int t in GetComponent<MeshFilter>().mesh.triangles)
        {
            lst_triangles.Add(dictVerticeIndex[t]);
        }
        lst_triangles = CleanTriangles(lst_triangles.ToArray()).ToList<int>();
        msh.triangles = lst_triangles.ToArray();
        
        GetComponent<MeshFilter>().mesh = msh;
    }

    private int[] CleanTriangles(int[] triangles)
    {
        int[] cleanedTriangles = new int[triangles.Length];

        int size = 0;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            if (!(triangles[i] == triangles[i + 1] || triangles[i] == triangles[i + 2] || triangles[i + 1] == triangles[i + 2]))
            {
                cleanedTriangles[size] = triangles[i];
                cleanedTriangles[size + 1] = triangles[i + 1];
                cleanedTriangles[size + 2] = triangles[i + 2];
                size += 3;
            }
        }
        System.Array.Resize(ref cleanedTriangles, size);
        return cleanedTriangles;
    }

}
