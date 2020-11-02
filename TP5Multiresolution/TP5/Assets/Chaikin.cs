using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaikin : MonoBehaviour
{
    public float u;
    public float v;

    LineRenderer lr;
    public Vector3[] positions;
    public List<Vector3> newPositions;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        
    }

    void IterationChaikin()
    {
        positions = new Vector3[lr.positionCount];
        lr.GetPositions(positions);
        newPositions = new List<Vector3>();
        lr.positionCount *= 2;
        lr.positionCount -= 1;
        for (int i = 0; i<positions.Length-1; i++)
        {
            Vector3 P1 = positions[i];
            Vector3 P2 = positions[(i + 1)%positions.Length];
            Debug.Log("p1 : "+P1+ " " +i);
            Debug.Log("p2 : "+P2+ " " +(i + 1) % positions.Length);


            Vector3 newP1 = P1 + ((P2 - P1).normalized * u*Vector3.Distance(P1,P2));
            Vector3 newP2 = P1 + ((P2 - P1).normalized * v * Vector3.Distance(P1, P2));

            newPositions.Add(newP1);
            newPositions.Add(newP2);

        }
        if (positions[0] == positions[positions.Length-1])
        {
            newPositions.Add(newPositions[0]);      
        }
        else
        {
            lr.positionCount -= 1;
        }
        
        lr.SetPositions(newPositions.ToArray());


    }



    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("g"))
        {
            IterationChaikin();
        }
    }
}
