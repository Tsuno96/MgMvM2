using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hello_Cone : MonoBehaviour
{
    const float PI = 3.1415926f;
    public int rayon, rayon2, hauteur, nmeridien;
    public int[] triangles;
    public Vector3[] vertices;
    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        gameObject.AddComponent<MeshRenderer>();

        // Création des structures de données qui accueilleront sommets et  triangles  // Remplissage de la structure sommet 

        vertices = new Vector3[nmeridien * 2 + 2];
        triangles = new int[nmeridien * 4 * 3];


        vertices[nmeridien * 2] = new Vector3(0, 0, 0);
        vertices[nmeridien * 2 + 1] = new Vector3(0, hauteur, 0);

        for (int i = 0; i < nmeridien; i++)
        {
            vertices[i] = new Vector3(rayon * Mathf.Sin((2 * PI * i + 1) / nmeridien), 0, rayon * Mathf.Cos((2 * PI * i + 1) / nmeridien));
            vertices[i + nmeridien + 1] = new Vector3(rayon2 * Mathf.Sin((2 * PI * i + 1) / nmeridien), hauteur, rayon2 * Mathf.Cos((2 * PI * i + 1) / nmeridien));
        }
        int k = 0;
        for (int j = 0; j < nmeridien; j++)
        {
            triangles[k] = nmeridien;
            triangles[k + 1] = (j + 1) % nmeridien;
            triangles[k + 2] = j;

            triangles[k + 3] = nmeridien * 2 + 1;
            triangles[k + 4] = j + nmeridien + 1;
            triangles[k + 5] = (j + 1) % nmeridien + nmeridien + 1;

            triangles[k + 6] = j;
            triangles[k + 7] = (j + 1) % nmeridien + nmeridien + 1;
            triangles[k + 8] = j + nmeridien + 1;

            triangles[k + 9] = j;
            triangles[k + 10] = (j + 1) % nmeridien;
            triangles[k + 11] = (j + 1) % nmeridien + nmeridien + 1;

            k += 12;
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
