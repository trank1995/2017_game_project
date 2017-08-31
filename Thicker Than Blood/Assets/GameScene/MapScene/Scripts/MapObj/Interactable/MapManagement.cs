﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManagement : MonoBehaviour {
    const int spawnRange = 20;
    int monthSCounter = TimeSystem.month;
    int monthECounter = TimeSystem.month;
    public GameObject[] banditSpawnPointList;
    public GameObject bandit, frenchTroop, papalTroop, italianTroop, imperialTroop;
	// Use this for initialization
	void Awake () {
        banditInitialization();
	}
	
	// Update is called once per frame
	void Update () {
        //banditUpdate();
        monthSCounter = TimeSystem.month;
        if (monthSCounter != monthECounter)
        {
            banditUpdate();
        }
        monthECounter = TimeSystem.month;
	}

    
    void banditInitialization()
    {
        banditSpawnPointList = GameObject.FindGameObjectsWithTag("BanditSpawnPoint");
        foreach (GameObject sp in banditSpawnPointList)
        {
            initialSpawn(sp, bandit);
        }
    }
    void banditUpdate()
    {
        banditSpawnPointList = GameObject.FindGameObjectsWithTag("BanditSpawnPoint");
        foreach (GameObject sp in banditSpawnPointList)
        {
            normalSpawn(sp, bandit);
        }
    }
    
    void normalSpawn(GameObject spawnPoint, GameObject toSpawn)
    {
        var rot = new Quaternion(0, Random.Range(0, 360), 0, 0);
        Instantiate(toSpawn, spawnPoint.transform.position, rot);
        //WaitForSeconds();
    }
    void initialSpawn(GameObject spawnPoint, GameObject toSpawn)
    {
        for (int i = 0; i < 0; i++)
        {
            var pos = new Vector3(Random.Range(-spawnRange, spawnRange), 3, Random.Range(-spawnRange, spawnRange));
            var rot = new Quaternion(0, Random.Range(0, 360), 0, 0);
            Instantiate(toSpawn, spawnPoint.transform.position + pos, rot);
        }
    }

}