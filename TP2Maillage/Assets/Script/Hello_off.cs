using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Globalization;
using System.Threading;

public class Hello_off : MonoBehaviour
{

    public string nameObject;
    public int nSommet;
    public int nFacette;
    public int nArrete;

    public Vector3[] vertices;
    public Vector3[] normales;
    public int[] triangles;
    public Material mat;
    float sommeX, sommeY, sommeZ;
    float minX, minY, minZ, maxX, maxY, maxZ;
    
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


       /* string[] Test = reader.ReadLine().Split(' ');

        string i = "-0.003393500111997128";
        Debug.Log(float.Parse(i, CultureInfo.InvariantCulture));*/

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


    // Update is called once per frame
    void Update()
    {

        gameObject.GetComponent<MeshFilter>().mesh.normals = normales;

    }
}
