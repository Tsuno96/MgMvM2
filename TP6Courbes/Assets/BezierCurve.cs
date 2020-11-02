using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(LineRenderer))]
public class BezierCurve : MonoBehaviour
{
    static long Factorial(long n)
    {
        return n > 1 ? n * Factorial(n - 1) : 1;
    }

    public GameObject bezierLink;
    public List<Transform> points;
    public int numberOfPoints = 20;
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.material = new Material(
            Shader.Find("Legacy Shaders/Particles/Additive"));
        DrawBezier();
    }

    // Update is called once per frame
    void Update()
    {
        DrawBezier();
        Collapse();
    }

    void DrawBezier()
    {
        lineRenderer.positionCount = numberOfPoints;
        List<Vector3> newPositions = new List<Vector3>();

        for (int i = 0; i <= numberOfPoints; i++)
        {
            float t = i / (numberOfPoints-1.0f);
            Vector3 newPoint = Vector3.zero;
            for (int j = 0; j < points.Count; j++)
            {
                newPoint += points[j].position * Bernstein(j, t);
            }
            newPositions.Add(newPoint);
        }
    

        lineRenderer.SetPositions(newPositions.ToArray());

    }

    float Bernstein(int i, float t)
    {
        float bernstein = Factorial(points.Count-1)/(Factorial(Convert.ToInt64(i)) *Factorial(Convert.ToInt64(points.Count-1-i)))*Mathf.Pow(t,i)*Mathf.Pow(1-t,points.Count-1-i);
        return bernstein;
    }

    void OnMouseDown()
    {
        
    }
    void Collapse()
    {
        Transform bLs = bezierLink.GetComponent<BezierCurve>().points[0];
        Transform bLe = bezierLink.GetComponent<BezierCurve>().points[bezierLink.GetComponent<BezierCurve>().points.Count-1];


        if(bLs.position.z > points[0].position.z)
        {
            bLs.position = points[points.Count - 1].position;
        }
        else
        {
            lineRenderer.SetPosition(0, bLe.position);
        }

    }

}
