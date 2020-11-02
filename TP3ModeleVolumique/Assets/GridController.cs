using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{

    public List<Transform> GO_spheres;
    public List<GameObject> cubes;
    public GameObject cube;
    public Vector3 dim;
    public int dimCubes;
    public GameObject[,,] argo;
    public static float SeuilPotentiel;
    public float Sp;
    // Start is called before the first frame update
    void Awake()
    {
        argo = new GameObject[dimCubes, dimCubes, dimCubes];
        cubes = new List<GameObject>();
        transform.position = Vector3.zero;
        createGrid();
    }
    void setBox()
    {
        Vector3 max= dim;
        Vector3 min= -dim;
        //Vector3 centerBox = min + (((max - min) / 2));
        Vector3 centerBox = transform.position;
        var GOmax = Instantiate(cube, max, Quaternion.identity);
        GOmax.name = "Max";
        var GOmin = Instantiate(cube, min, Quaternion.identity);
        GOmin.name = "Min";
        GameObject c = Instantiate(cube, centerBox, Quaternion.identity);
        c.transform.localScale = new Vector3(max.x - min.x, max.y - min.y, max.z - min.z);
        c.name = "Cube0";
        cubes.Add(c);

    }

    public void createGrid()
    {
        setBox();
        Vector3 v3Center = cubes[0].transform.position;
        for (var i = 0; i < dimCubes; i++)
        {
            for (var j = 0; j < dimCubes; j++)
            {
                for (var k = 0; k < dimCubes; k++)
                {
                    float x = (float)(v3Center.x - cubes[0].transform.localScale.x / 2 + (i + 0.5) * cubes[0].transform.localScale.x / dimCubes);
                    float y = (float)(v3Center.y + cubes[0].transform.localScale.y / 2 - (j + 0.5) * cubes[0].transform.localScale.y / dimCubes);
                    float z = (float)(v3Center.z - cubes[0].transform.localScale.z / 2 + (k + 0.5) * cubes[0].transform.localScale.z / dimCubes);

                    argo[i, j, k] = Instantiate(cube, new Vector3(x, y, z), Quaternion.identity);
                    argo[i, j, k].transform.localScale = cubes[0].transform.localScale / dimCubes;
                    argo[i, j, k].name = "Cube" + i + j + k;
                    
                    cubes.Add(argo[i, j, k]);
                }
            }
        }     
        cubes[0].SetActive(false);
        CalcPotentiel();
    }

    void CalcPotentiel()
    {
        foreach (GameObject c in cubes)
        {
            c.GetComponent<CubeController>().potentiel = 0;
            foreach (Transform t in GO_spheres)
            {
                c.GetComponent<CubeController>().potentiel += 1000/Vector3.Distance(c.transform.position, t.position);
                c.GetComponent<CubeController>().potentiel -= c.GetComponent<CubeController>().offset_potentiel;
            }
        }

    }




    // Update is called once per frame
    void Update()
    {
        SeuilPotentiel = Sp;
        CalcPotentiel();
    }
}
