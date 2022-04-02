using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

	public GameObject objectToSpawn;

    public void Spawn(){

    	Instantiate(objectToSpawn, transform.position, transform.rota);

    }
}
