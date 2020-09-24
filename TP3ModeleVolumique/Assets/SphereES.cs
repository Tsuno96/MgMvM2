using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class SphereES : MonoBehaviour
{
    SphereCollider SC;
    public int rayon;

    /*void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, rayon);
    }*/

    // Start is called before the first frame update
    void Awake()
    {
        SC = GetComponent<SphereCollider>();
        SC.radius = rayon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
