using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hello_Grid : MonoBehaviour
{

    public int row, column;
    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        gameObject.AddComponent<MeshRenderer>();

        // Création des structures de données qui accueilleront sommets et  triangles  // Remplissage de la structure sommet 

        Vector3[] vertices = new Vector3[row * column * 4];
        int[] triangles = new int[row * column * 6];

        for (int i = 0; i < column; i++)
        {
            for (int j = 0; j < row; j++)
            {
                vertices[(i + j * column) * 4] = new Vector3(i, j, 0);
                vertices[(i + j * column) * 4 + 1] = new Vector3(i + 1, j, 0);
                vertices[(i + j * column) * 4 + 2] = new Vector3(i, j + 1, 0);
                vertices[(i + j * column) * 4 + 3] = new Vector3(i + 1, j + 1, 0);


                triangles[(i + j * column) * 6] = (i + j * column) * 4;
                triangles[(i + j * column) * 6 + 1] = (i + j * column) * 4 + 1;
                triangles[(i + j * column) * 6 + 2] = (i + j * column) * 4 + 2;
                triangles[(i + j * column) * 6 + 3] = (i + j * column) * 4 + 2;
                triangles[(i + j * column) * 6 + 4] = (i + j * column) * 4 + 1;
                triangles[(i + j * column) * 6 + 5] = (i + j * column) * 4 + 3;


            }
        }


        // Remplissage de la structure triangle. Les sommets sont représentés par leurs indices
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

    // Update is called once per frame
    void Update()
    {
        Vector3[] vertices = new Vector3[row * column * 4];
        int[] triangles = new int[row * column * 6];

        for (int i = 0; i < column ; i++)
        {
            for (int j = 0; j < row; j++)
            {
                vertices[(i+j*column)*4 ] = new Vector3(i, j, 0);
                vertices[(i+j*column)*4 + 1] = new Vector3(i + 1, j, 0);
                vertices[(i+j*column)*4 + 2] = new Vector3(i, j + 1, 0);
                vertices[(i + j * column) * 4 + 3] = new Vector3(i + 1, j + 1, 0);


                triangles[(i+j*column)*6] =  (i+j*column)*4;
                triangles[(i+j*column)*6 + 1] = (i+j*column)*4 + 1;
                triangles[(i+j*column)*6 + 2] = (i+j*column)*4 + 2;
                triangles[(i+j*column)*6 + 3] = (i+j*column)*4 + 2;
                triangles[(i+j*column)*6 + 4] = (i+j*column)*4 + 1;
                triangles[(i + j * column) * 6 + 5] = (i + j * column) * 4 + 3;


            }
        }


        // Remplissage de la structure triangle. Les sommets sont représentés par leurs indices
        // les triangles sont représentés par trois indices (et sont mis bout à bout)



        Mesh msh = new Mesh();                          // Création et remplissage du Mesh

        msh.vertices = vertices;
        msh.triangles = triangles;

    
        gameObject.GetComponent<MeshFilter>().mesh = msh;           // Remplissage du Mesh et ajout du matériel
        gameObject.GetComponent<MeshRenderer>().material = mat;
    }
}
