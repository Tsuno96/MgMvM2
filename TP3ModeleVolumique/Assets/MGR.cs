using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MGR : MonoBehaviour
{


    private static MGR p_instance = null;
    public static MGR Instance { get { return p_instance; } }

    float blueMat;
    Material material;
    public GameObject cube;
    public GameObject sphere;
    int rayon;
    public List<GameObject> cubes;
    List<GameObject> childCube;

    public int subDiv;
    public int dimCubes;
 
    public GameObject[,,] argo;


    private void Awake()
    {
        if (p_instance == null)
            //if not, set instance to this
            p_instance = this;
        //If instance already exists and it's not this:
        else if (p_instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);


    }

    // Start is called before the first frame update
    void Start()
    {
        rayon = (int)sphere.transform.localScale.x /2;
        //argo = new GameObject[dimCubes,dimCubes,dimCubes];
        GameObject c = Instantiate(cube, sphere.transform.position, Quaternion.identity);
        c.transform.localScale = Vector3.one * rayon* 2;
        cubes.Add(c);
        blueMat = c.GetComponent<Renderer>().material.color.b;

    }

    public void createGrid()
    {
        Vector3 v3Center = sphere.transform.position;
        for (var i = 0; i < dimCubes; i++) {
         for (var j = 0; j < dimCubes; j++) {
             for (var k = 0; k < dimCubes; k++) {
                 float x = (float)(v3Center.x - dimCubes / 2.0 + i);
                 float y = (float)(v3Center.y + dimCubes / 2.0 - j);
                 float z = (float)(v3Center.z - dimCubes / 2.0 + k);
                 
                 argo[i,j,k] = Instantiate(cube, new Vector3(x,y,z), Quaternion.identity);
                 //argo[i,j,k].transform.localScale = Vector3.one * (sphere.GetComponent<SphereES>().rayon * 2) / dimCubes;
             }
            }
         }
    }
  

    public void subdivise()
    {
        childCube = new List<GameObject>();
        foreach (GameObject cubeGO in cubes)
        {
            DividedCube(cubeGO);

        }
        cubes = childCube;
    }

    void DividedCube(GameObject cubeOriginal)
    {
        blueMat *= 0.7f;
        Transform COTrans = cubeOriginal.transform;
        List<Vector3> posCubesChild = new List<Vector3>();
        posCubesChild.Add(COTrans.position + new Vector3(-COTrans.localScale.x / 4, -COTrans.localScale.y / 4, -COTrans.localScale.z / 4));
        posCubesChild.Add(COTrans.position + new Vector3(-COTrans.localScale.x / 4, -COTrans.localScale.y / 4, COTrans.localScale.z / 4));
        posCubesChild.Add(COTrans.position + new Vector3(COTrans.localScale.x / 4, -COTrans.localScale.y / 4, -COTrans.localScale.z / 4));
        posCubesChild.Add(COTrans.position + new Vector3(COTrans.localScale.x / 4, -COTrans.localScale.y / 4, COTrans.localScale.z / 4));

        posCubesChild.Add(COTrans.position + new Vector3(-COTrans.localScale.x / 4, COTrans.localScale.y / 4, -COTrans.localScale.z / 4));
        posCubesChild.Add(COTrans.position + new Vector3(-COTrans.localScale.x / 4, COTrans.localScale.y / 4, COTrans.localScale.z / 4));
        posCubesChild.Add(COTrans.position + new Vector3(COTrans.localScale.x / 4, COTrans.localScale.y / 4, -COTrans.localScale.z / 4));
        posCubesChild.Add(COTrans.position + new Vector3(COTrans.localScale.x / 4, COTrans.localScale.y / 4, COTrans.localScale.z / 4));

        foreach (Vector3 v in posCubesChild)
        {
            int counterOnSphere = 0;
            List<Vector3> verticesCube = new List<Vector3>();
            verticesCube.Add(v+ new Vector3(-COTrans.localScale.x / 4, -COTrans.localScale.y / 4, -COTrans.localScale.z / 4));
            verticesCube.Add(v+ new Vector3(-COTrans.localScale.x / 4, -COTrans.localScale.y / 4, COTrans.localScale.z / 4));
            verticesCube.Add(v+ new Vector3(COTrans.localScale.x / 4, -COTrans.localScale.y / 4, -COTrans.localScale.z / 4));
            verticesCube.Add(v+ new Vector3(COTrans.localScale.x / 4, -COTrans.localScale.y / 4, COTrans.localScale.z / 4));

            verticesCube.Add(v+ new Vector3(-COTrans.localScale.x / 4, COTrans.localScale.y / 4, -COTrans.localScale.z / 4));
            verticesCube.Add(v+ new Vector3(-COTrans.localScale.x / 4, COTrans.localScale.y / 4, COTrans.localScale.z / 4));
            verticesCube.Add(v+ new Vector3(COTrans.localScale.x / 4, COTrans.localScale.y / 4, -COTrans.localScale.z / 4));
            verticesCube.Add(v + new Vector3(COTrans.localScale.x / 4, COTrans.localScale.y / 4, COTrans.localScale.z / 4));

            foreach (Vector3 vPoint in verticesCube)
            {
                if (Vector3.Distance(vPoint, sphere.transform.position) <rayon)
                {
                    counterOnSphere ++;
                }
            }
            if (counterOnSphere < 8 && counterOnSphere> 0)
            {
                GameObject GOchild  = Instantiate(cube, v, COTrans.rotation);
                GOchild.GetComponent<CubeController>().onSphere = counterOnSphere;
                GOchild.GetComponent<CubeController>().vertices = verticesCube;
                //GOchild.GetComponent<Renderer>().material.color = new Color(0,46,blueMat,255);
                childCube.Add(GOchild);


            }
            else if(counterOnSphere >=8)
            {
                GameObject GO  = Instantiate(cube, v, COTrans.rotation);
                GO.GetComponent<CubeController>().onSphere = counterOnSphere;
                GO.GetComponent<CubeController>().vertices = verticesCube; 
                //GO.GetComponent<Renderer>().material.color = new Color(0,46,blueMat);
                GO.transform.localScale = COTrans.localScale / 2;
            }
        }

        foreach (GameObject GO in childCube)
        {
            if (GO.transform.localScale.x < COTrans.localScale.x)
            {
                GO.transform.localScale = COTrans.localScale / 2;

            }
        }

        Destroy(cubeOriginal);
    }

    public void DeleteCube()
    {
        foreach (GameObject GO in cubes)
        {
            if (Vector3.Distance(GO.transform.position, sphere.transform.position) > rayon)
            {
                Destroy(GO);
            }
        }
    }

    


    // Update is called once per frame
    void Update()
    {
        
    }
}
