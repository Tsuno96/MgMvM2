using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BezierMGR : MonoBehaviour
{

    public List<GameObject> beziers;

    // Start is called before the first frame update
    void Start()
    {
        beziers = GameObject.FindGameObjectsWithTag("Bezier").ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
