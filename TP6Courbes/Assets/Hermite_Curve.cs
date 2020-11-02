using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode, RequireComponent(typeof(LineRenderer))]
public class Hermite_Curve : MonoBehaviour
{
	public Transform P0, P1, PT0, PT1;

	public Color color = Color.white;
	public float width = 0.2f;
	public int numberOfPoints = 20;
	LineRenderer lineRenderer;

	void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.useWorldSpace = true;
		lineRenderer.material = new Material(
			Shader.Find("Legacy Shaders/Particles/Additive"));
	}

	void Update()
	{
		DrawLine();
	}

	void DrawLine()
    {
		lineRenderer.positionCount = numberOfPoints;
		List<Vector3> newPositions = new List<Vector3>();

		Vector3 V0 = PT0.position - P0.position;
		Vector3 V1 = PT1.position - P1.position;
		for (int i = 0; i<numberOfPoints; i++)
        {
			float u = i / (numberOfPoints-1.0f);
			float F1 = 2 * Mathf.Pow(u, 3) - 3 * Mathf.Pow(u, 2) + 1;
			float F2 = -2 * Mathf.Pow(u, 3) + 3 * Mathf.Pow(u, 2);
			float F3 = Mathf.Pow(u, 3) - 2 * Mathf.Pow(u, 2) + u;
			float F4 = Mathf.Pow(u, 3) -  Mathf.Pow(u, 2);
			newPositions.Add(F1*P0.position + F2*P1.position + F3* V0 + F4* V1);
        }

		lineRenderer.SetPositions(newPositions.ToArray());

    }
}
