﻿using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	public GameObject[] islandsPrefab;
	public GameObject islandsContainer;
	public GameObject[] spiritsPrefab;
	public GameObject spiritsContainer;
	public GameObject[] bonusPrefab;
	public GameObject bonusContainer;

	private float maxDist;
	public int nbIslands;
	public int nbSpirits;
	public int nbBonus;
	public bool spawned = false;
	public bool allGood = false;
	private ArrayList islands = new ArrayList();
	private ArrayList spirits = new ArrayList();
	private ArrayList bonus = new ArrayList();

	void Awake()
	{
		maxDist = GameMaster.GM.maxDistDecor;
	}

	void Update () {
		if (!spawned)
		{
			unspawnPrevious(islands);
			unspawnPrevious(spirits);
			unspawnPrevious(bonus);

			spawnObjects(islandsPrefab, islandsContainer, nbIslands, islands);
			spawnObjects(spiritsPrefab, spiritsContainer, nbSpirits, spirits, -maxDist, maxDist, -maxDist, maxDist, -maxDist, maxDist);
			spawnObjects(bonusPrefab, bonusContainer, nbBonus, bonus);
			spawned = true;
		}
		else if(!allGood)
		{
			checkForCollision(islands);
			checkForCollision(spirits, -maxDist, maxDist, -maxDist, maxDist, -maxDist, maxDist);
			checkForCollision(bonus);
			allGood = true;
		}
	}

	public void spawnObjects(GameObject[] _prefab, GameObject _container, int _nb, ArrayList _list)
	{
		spawnObjects(_prefab, _container, _nb, _list, -maxDist, maxDist, -maxDist, maxDist, -maxDist, maxDist);
	}

	public void spawnObjects(GameObject[] _prefab, GameObject _container, int _nb, ArrayList _list, float _xMin, float _xMax, float _yMin, float _yMax, float _zMin, float _zMax)
	{
		for (int i = 0; i < _nb; ++i)
		{
			Vector3 pos = new Vector3(Random.Range(_xMin, _xMax), Random.Range(_yMin, _yMax), Random.Range(_zMin, _zMax));
			Quaternion rot = Quaternion.Euler(.0f, Random.Range(.0f, 360.0f), .0f);
			int id = Random.Range(0, _prefab.Length);

			GameObject obj = Instantiate(_prefab[id], pos, rot) as GameObject;

			_list.Add(obj);
			obj.transform.parent = _container.transform;
		}
	}

	private void checkForCollision(ArrayList _objects)
	{
		checkForCollision(_objects, -maxDist, maxDist, -maxDist, maxDist, -maxDist, maxDist);
	}

	private void checkForCollision(ArrayList _objects, float _xMin, float _xMax, float _yMin, float _yMax, float _zMin, float _zMax)
	{
		for (int i = 0; i < _objects.Count; ++i)
		{
			GameObject obj = _objects[i] as GameObject;
			float radius = (obj.GetComponent<Collider>() as SphereCollider).radius;

			Collider[] hitColliders = Physics.OverlapSphere(obj.transform.position + (obj.GetComponent<Collider>() as SphereCollider).center, radius, LayerMask.GetMask("randomGen"));
			
			int nbHit = 0;
			foreach(Collider col in hitColliders)
			{
				if (col.gameObject != obj)
					nbHit++;
			}

			while (nbHit > 0)
			{
				obj.transform.position = new Vector3(Random.Range(_xMin, _xMax), Random.Range(_yMin, _yMax), Random.Range(_zMin, _zMax));
				hitColliders = Physics.OverlapSphere(obj.transform.position + (obj.GetComponent<Collider>() as SphereCollider).center, radius, LayerMask.GetMask("randomGen"));

				nbHit = 0;
				foreach (Collider col in hitColliders)
				{
					if (col.gameObject != obj)
						nbHit++;
				}
			}
		}
	}

	private void unspawnPrevious(ArrayList _objects)
	{
		for (int i = 0; i < _objects.Count; ++i)
			DestroyImmediate((GameObject)_objects[i]);
		_objects.Clear();
	}

	//private void OnDrawGizmos() {
	//	Gizmos.color = Color.red;
	//	if(allGood)
	//	 //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
	//	 for(int i = 0; i < nbIslands; ++i)
	//	 {
	//		 GameObject obj = islands[i] as GameObject;
	//		 Gizmos.DrawWireSphere(obj.transform.position + (obj.collider as SphereCollider).center, (obj.collider as SphereCollider).radius);
	//	 }
	// }
}