using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hello_Cylindre : MonoBehaviour
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

        vertices = new Vector3[nmeridien * 2 + 2];
        triangles = new int[nmeridien * 2 * 3];


        vertices[nmeridien * 2] = new Vector3(0, 0, 0);
        vertices[nmeridien * 2+1] = new Vector3(0, hauteur, 0);

        for (int i = 0; i<nmeridien; i++)
        {
            vertices[i*2] = new Vector3(rayon * Mathf.Cos(PI *i) /nmeridien, 0, rayon * Mathf.Sin(PI * i) / nmeridien);
            vertices[i*2 +1] = new Vector3(rayon * Mathf.Cos(PI * i) / nmeridien, hauteur, rayon * Mathf.Sin(PI * i) / nmeridien);

        }
        int k = 0;
        for(int j = 0; j<nmeridien;j++)
        {
            triangles[k] = nmeridien * 2; 
            triangles[k +1] = (j*2) ; 
            triangles[k +2] = ((j *2) + 2) % nmeridien; 
            triangles[k +3] = nmeridien * 2 +1; 
            triangles[k +4] = (j *2) + 1 ; 
            triangles[k +5] = ((j *2) + 3) % nmeridien;

            k += 6;
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
