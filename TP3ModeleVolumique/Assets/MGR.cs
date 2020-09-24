using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MGR : MonoBehaviour
{


    private static MGR p_instance = null;
    public static MGR Instance { get { return p_instance; } }


    public GameObject cube;
    public GameObject sphere;

    public List<GameObject> cubes;
    List<GameObject> childCube;

    public int subDiv;

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
        GameObject c = Instantiate(cube, sphere.transform.position, Quaternion.identity);
        c.transform.localScale = Vector3.one * (sphere.GetComponent<SphereES>().rayon * 2);
        cubes.Add(c);

        for(int i = 0; i<subDiv;i++)
        {
            subdivise();
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

        Transform COTrans = cubeOriginal.transform;
        List<GameObject> localchildCube = new List<GameObject>();
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
            bool onSphere = false;
            List<Vector3> verticesCube = new List<Vector3>();
            verticesCube.Add(v+ new Vector3(-COTrans.localScale.x / 8, -COTrans.localScale.y / 8, -COTrans.localScale.z / 8));
            verticesCube.Add(v+ new Vector3(-COTrans.localScale.x / 8, -COTrans.localScale.y / 8, COTrans.localScale.z / 8));
            verticesCube.Add(v+ new Vector3(COTrans.localScale.x / 8, -COTrans.localScale.y / 8, -COTrans.localScale.z / 8));
            verticesCube.Add(v+ new Vector3(COTrans.localScale.x / 8, -COTrans.localScale.y / 8, COTrans.localScale.z / 8));

            verticesCube.Add(v+ new Vector3(-COTrans.localScale.x / 8, COTrans.localScale.y / 8, -COTrans.localScale.z / 8));
            verticesCube.Add(v+ new Vector3(-COTrans.localScale.x / 8, COTrans.localScale.y / 8, COTrans.localScale.z / 8));
            verticesCube.Add(v+ new Vector3(COTrans.localScale.x / 8, COTrans.localScale.y / 8, -COTrans.localScale.z / 8));
            verticesCube.Add(v + new Vector3(COTrans.localScale.x / 8, COTrans.localScale.y / 8, COTrans.localScale.z / 8));

            foreach (Vector3 vPoint in verticesCube)
            {
                if (Vector3.Distance(vPoint, sphere.transform.position) < sphere.GetComponent<SphereES>().rayon)
                {
                    onSphere = true;
                    break;
                }
            }
            if (onSphere)
            {
                childCube.Add(Instantiate(cube, v, COTrans.rotation));
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
            if (Vector3.Distance(GO.transform.position, sphere.transform.position) > sphere.GetComponent<SphereES>().rayon)
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
