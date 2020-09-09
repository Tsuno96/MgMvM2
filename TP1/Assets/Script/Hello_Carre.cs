using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hello_Carre : MonoBehaviour
{

    public Material mat;

    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        gameObject.AddComponent<MeshRenderer>();

        Vector3[] vertices = new Vector3[] {          // Création des structures de données qui accueilleront sommets et  triangles
        

        new Vector3(0, 0, 0),           // Remplissage de la structure sommet 
        new Vector3(1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector3(1, 1, 0),
    };
        int[] triangles = new int[]
        {
            0,1,2,
            2,1,3,

        };// Remplissage de la structure triangle. Les sommets sont représentés par leurs indices
                                                                 // les triangles sont représentés par trois indices (et sont mis bout à bout)



        Mesh msh = new Mesh();                          // Création et remplissage du Mesh

        msh.vertices = vertices;
        msh.triangles = triangles;

        foreach (Vector3 n in msh.vertices)
        {
            Debug.Log(n);
        }

        gameObject.GetComponent<MeshFilter>().mesh = msh;           // Remplissage du Mesh et ajout du matériel
        gameObject.GetComponent<MeshRenderer>().material = mat;
    }
}
