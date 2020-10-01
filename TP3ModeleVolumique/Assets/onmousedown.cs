using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onmousedown : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnMouseDown()
    {
        MGR.Instance.subdivise();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
