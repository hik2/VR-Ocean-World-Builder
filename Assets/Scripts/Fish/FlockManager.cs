using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour {

    public GameObject fishPrefab;
    public GameObject fishBait;
    public List<GameObject> fish;
    public int boundary;
    public int numFish;

	// Use this for initialization
	void Start ()
    {
        fish = new List<GameObject>();	
        
        for(int i = 0; i < numFish; i++)
        {
            Vector3 fishPosition = new Vector3(Random.Range(-boundary + transform.position.x, boundary + transform.position.x), Random.Range(-boundary + transform.position.y, boundary + transform.position.y), Random.Range(-boundary + transform.position.z, boundary + transform.position.z));
            SpawnFish(fishPosition, Quaternion.identity);
        }
	}

    public GameObject SpawnFish(Vector3 position, Quaternion rotation)
    {
        GameObject newFish = Instantiate(fishPrefab, position, rotation) as GameObject;
        newFish.GetComponent<FishController>().flock = this;
        fish.Add(newFish);
        return newFish;
    }
}
