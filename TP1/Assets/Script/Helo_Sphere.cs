using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helo_Sphere : MonoBehaviour
{
    const float PI = 3.1415926f;
    public int rayon, nparallele, nmeridien;
    public int[] triangles;
    public Vector3[] vertices;
    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        gameObject.AddComponent<MeshRenderer>();

        // Création des structures de données qui accueilleront sommets et  triangles  // Remplissage de la structure sommet 

        vertices = new Vector3[(nmeridien +1) *(nparallele+1)];
        triangles = new int[nmeridien * 2 *nparallele * 3];


        int index = 0;

        for (int i = 0; i <= nparallele; i++)
        {
            float phi = -PI / 2 + i * PI / nparallele;
            for(int j = 0; j<= nmeridien; j++)
            {
                float teta = j * 2 * PI / nmeridien;
                
                float x = Mathf.Cos(teta) * Mathf.Cos(phi);
                float y = Mathf.Sin(teta) * Mathf.Cos(phi);
                float z = Mathf.Sin(phi);

                vertices[index] = new Vector3(x, y ,z);
                index++;
            }
        }

        int k = 0;
        for (int j = 0; j < nparallele; j++)
        {
            int r1 = j * (nmeridien + 1);
            int r2 = (j+1) * (nmeridien + 1);
            for (int i = 0; i < nmeridien; i++)
            {
                triangles[k] = r1 + i;
                triangles[k+1] = r2 + i +1;
                triangles[k+2] = r2 + i;

                triangles[k+3] = r1 + i;
                triangles[k+4] = r1 + i + 1;
                triangles[k+5] = r2 + i + 1;

                k += 6;
            }
        }






        // Remplissage de la structure triangle. Les sommets sont représentés par leurs indices
        // les triangles sont représentés par trois indices (et sont mis bout à bout)



        Mesh msh = new Mesh();                          // Création et remplissage du Mesh

        msh.vertices = vertices;
        msh.triangles = triangles;


        //foreach (Vector3 n in msh.vertices)
        //{
        //    Debug.Log(n);
        //}

        gameObject.GetComponent<MeshFilter>().mesh = msh;           // Remplissage du Mesh et ajout du matériel
        gameObject.GetComponent<MeshRenderer>().material = mat;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
