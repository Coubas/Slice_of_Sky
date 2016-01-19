using UnityEngine;
using System.Collections;

public class CloudsGenerator : MonoBehaviour {

    public GameObject[] CloudsPrefab;
    public GameObject CloudsContainer;
    public GameObject[] SpecialCloudsPrefab;
    public GameObject SpecialCloudsContainer;

    public bool spawned = false;
    public int nbClouds;
    public int nbSpecialClouds;
    public int nbMaxCloudComponent;
    public float spaceBetweenComponent;
    private float maxDist;
    private ArrayList clouds = new ArrayList();

    // Use this for initialization
    void Start () {
        maxDist = GameMaster.GM.maxDistDecor;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerData.PD.gaugesLvl.Length > 0 && PlayerData.PD.gaugesLvl[0] > 0 || PlayerData.PD.gaugesLvl.Length <= 0)
        {
            if (!spawned)
            {
                unspawnPrevious(clouds);

                //Normal clouds
                for (int i = 0; i < nbClouds; ++i)
                {
                    //Create the container of the cloud
                    Vector3 pos = new Vector3(Random.Range(-maxDist, maxDist), Random.Range(-maxDist, maxDist), Random.Range(-maxDist, maxDist));
                    Quaternion rot = Quaternion.Euler(.0f, Random.Range(.0f, 360.0f), .0f);
                    GameObject cloudMaster = new GameObject();
                    cloudMaster.name = "Cloud" + i.ToString();
                    cloudMaster.transform.position = pos;
                    cloudMaster.transform.rotation = rot;

                    //Fill the container with a few cloud prefab to make a nice unique cloud
                    int nbComponent = Random.Range(1, nbMaxCloudComponent + 1);
                    for (int j = 0; j < nbComponent; ++j)
                    {
                        Vector3 cpos = pos + new Vector3(Random.Range(-spaceBetweenComponent, spaceBetweenComponent), Random.Range(-spaceBetweenComponent, spaceBetweenComponent), Random.Range(-spaceBetweenComponent, spaceBetweenComponent));
                        Quaternion crot = Quaternion.Euler(Random.Range(-45.0f, 45.0f), Random.Range(-45.0f, 45.0f), Random.Range(-45.0f, 45.0f));
                        int cid = Random.Range(0, CloudsPrefab.Length);
                        GameObject obj = Instantiate(CloudsPrefab[cid], cpos, crot) as GameObject;
                        obj.transform.parent = cloudMaster.transform;
                    }

                    //Add the container in the list to update it and store it with the other clouds
                    clouds.Add(cloudMaster);
                    cloudMaster.transform.parent = CloudsContainer.transform;

                }

                //Special clouds
                for (int i = 0; i < nbSpecialClouds; ++i)
                {
                    Vector3 pos = new Vector3(Random.Range(-maxDist, maxDist), Random.Range(-maxDist, maxDist), Random.Range(-maxDist, maxDist));
                    Quaternion rot = Quaternion.Euler(.0f, Random.Range(.0f, 360.0f), .0f);
                    int cid = Random.Range(0, SpecialCloudsPrefab.Length);
                    GameObject obj = Instantiate(SpecialCloudsPrefab[cid], pos, rot) as GameObject;
                    obj.transform.parent = SpecialCloudsContainer.transform;
                }

                spawned = true;
            }
        }
    }

    private void unspawnPrevious(ArrayList _objects)
    {
        for (int i = 0; i < _objects.Count; ++i)
            DestroyImmediate((GameObject)_objects[i]);
        _objects.Clear();
    }
}
