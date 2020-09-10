using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hello_Cercle : MonoBehaviour
{

    const float PI = 3.1415926f;
    public int rayon, hauteur, nmeridien;
    public int[] triangles;
    public Vector3[] vertices;
    public Material mat;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        gameObject.AddComponent<MeshRenderer>();

        // Création des structures de données qui accueilleront sommets et  triangles  // Remplissage de la structure sommet 

        vertices = new Vector3[nmeridien + 1];
        triangles = new int[nmeridien * 3];


        vertices[nmeridien] = new Vector3(0, 0, 0);

        for (int i = 0; i < nmeridien; i++)
        {
            vertices[i] = new Vector3(rayon * Mathf.Sin((2 *PI * i+1) / nmeridien), 0, rayon * Mathf.Cos((2 *PI * i+1) / nmeridien));

        }
        int k = 0;
        for (int j = 0; j < nmeridien; j++)
        {
            triangles[k] = nmeridien;
            triangles[k + 1] = (j);
            triangles[k + 2] = ((j) + 1) % nmeridien;
            k += 3;
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
