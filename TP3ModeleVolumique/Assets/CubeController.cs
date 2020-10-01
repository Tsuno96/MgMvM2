using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public int onSphere;
    public List<Vector3> vertices;
    public bool[] arrb_vertices;
    // Start is called before the first frame update
    void Start()
    {
        arrb_vertices = new bool[8];
        for(int i = 0; i< arrb_vertices.Length;i++)
        {
            arrb_vertices[i] = false;
        }   
        
     }

    // Update is called once per frame
    void Update()
    {
        
    }


    /*private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Sphere")
        {
            onSphere = true;
        }
    }*/

    private void OnMouseDown()
    {

    }
}
