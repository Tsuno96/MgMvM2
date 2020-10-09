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
    public List<GameObject> spheres;
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

        argo = new GameObject[dimCubes,dimCubes,dimCubes];
        
        setBox();

    }

    void setBox()
    {
        float minX, minY, minZ;
        minX = Mathf.Infinity;
        minY = Mathf.Infinity;
        minZ = Mathf.Infinity;


        float maxX, maxY, maxZ;
        maxX = Mathf.NegativeInfinity;
        maxY = Mathf.NegativeInfinity;
        maxZ = Mathf.NegativeInfinity;

        foreach(GameObject sphere in spheres)
        {
            if(sphere.transform.position.x + sphere.transform.localScale.x/2 > maxX)
            {
                maxX = sphere.transform.position.x + sphere.transform.localScale.x/2;
            }

            if(sphere.transform.position.x - sphere.transform.localScale.x/2 < minX)
            {
                minX = sphere.transform.position.x - sphere.transform.localScale.x/2;
            }

            if(sphere.transform.position.y + sphere.transform.localScale.y/2 > maxY)
            {
                maxY = sphere.transform.position.y + sphere.transform.localScale.y/2;
            }

            if(sphere.transform.position.y - sphere.transform.localScale.y/2 < minY)
            {
                minY = sphere.transform.position.y - sphere.transform.localScale.y/2;
            }

            if(sphere.transform.position.z + sphere.transform.localScale.z/2 > maxZ)
            {
                maxZ = sphere.transform.position.z + sphere.transform.localScale.z/2;
            }

            if(sphere.transform.position.z - sphere.transform.localScale.z/2 < minZ)
            {
                minZ = sphere.transform.position.z - sphere.transform.localScale.z/2;
            }
        }

        Vector3 min = new Vector3(minX,minY,minZ);
        Vector3 max = new Vector3(maxX,maxY,maxZ);
        Vector3 centerBox = min + (((max - min) / 2));
        Debug.Log(centerBox);
        var GOmax = Instantiate(cube, max, Quaternion.identity);
        GOmax.name = "Max";
        var GOmin = Instantiate(cube, min, Quaternion.identity);
        GOmin.name = "Min";
        GameObject c = Instantiate(cube, centerBox, Quaternion.identity);
        c.transform.localScale = new Vector3(maxX - minX,maxY -minY, maxZ-minZ);
        c.name = "Cube0";
        cubes.Add(c);

    }






    public void createGrid()
    {
        Vector3 v3Center = cubes[0].transform.position;
        for (var i = 0; i < dimCubes; i++) {
         for (var j = 0; j < dimCubes; j++) {
                for (var k = 0; k < dimCubes; k++)
                {
                    float x = (float)(v3Center.x - cubes[0].transform.localScale.x / 2 + (i+0.5) * cubes[0].transform.localScale.x / dimCubes);
                    float y = (float)(v3Center.y + cubes[0].transform.localScale.y / 2 - (j+0.5) * cubes[0].transform.localScale.y / dimCubes);
                    float z = (float)(v3Center.z - cubes[0].transform.localScale.z / 2 + (k+0.5) * cubes[0].transform.localScale.z / dimCubes);
                    
                    argo[i, j, k] = Instantiate(cube, new Vector3(x, y, z), Quaternion.identity);
                    argo[i, j, k].transform.localScale = cubes[0].transform.localScale / dimCubes;
                    argo[i, j, k].name = "Cube" + i + j + k;
                    cubes.Add(argo[i, j, k]);
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
            bool[] boolsv;
            boolsv = new bool[8];
            bool sphereIn = false;
             
            foreach(GameObject s in spheres)
            {
                for(int l = 0; l<verticesCube.Count; l++)
                {  
                    float rayon = s.transform.localScale.x /2;
                    if (Vector3.Distance(verticesCube[l], s.transform.position) <rayon)
                    {
                        boolsv[l] = true;
                    }
                    
                    
                }
            
                if(s.transform.position.x > verticesCube[0].x &&  s.transform.position.y > verticesCube[0].y && s.transform.position.z > verticesCube[0].z)
                {
                    Debug.Log(verticesCube[0]);
                    if(s.transform.position.x < verticesCube[7].x &&  s.transform.position.y < verticesCube[7].y && s.transform.position.z < verticesCube[7].z)
                    {
                        
                        sphereIn = true;
                    }
                }
                
            }
        
           
            foreach(bool b in boolsv) { if(b){counterOnSphere ++;}}
            if (counterOnSphere < 8)
            {

                if(counterOnSphere > 0 || sphereIn)
                {
                    GameObject GOchild  = Instantiate(cube, v, COTrans.rotation);
                    GOchild.GetComponent<CubeController>().onSphere = counterOnSphere;
                    GOchild.GetComponent<CubeController>().vertices = verticesCube;
                    childCube.Add(GOchild);
                }                    

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
            bool inSphere = false;
            foreach (GameObject s in spheres)
            {       
                if (Vector3.Distance(GO.transform.position, s.transform.position) < s.transform.localScale.x / 2)
                {
                    inSphere = true;
                }
            }
            if(!inSphere)
            {
                Destroy(GO);    
            }
        }
        Destroy(cubes[0]);
    }

    public void DeleteCubeUnion()
    {
        foreach (GameObject GO in cubes)
        {
            List<bool> inSphere = new List<bool>();
            foreach (GameObject s in spheres)
            {
                if (Vector3.Distance(GO.transform.position, s.transform.position) < s.transform.localScale.x / 2)
                {
                    inSphere.Add(true);
                }
            }
            if (inSphere.Count < 2)
            {
                Destroy(GO);
            }
        }
        Destroy(cubes[0]);
    }




    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            subdivise();
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            createGrid();
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            DeleteCube();
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            DeleteCubeUnion();
        }
    }
}
