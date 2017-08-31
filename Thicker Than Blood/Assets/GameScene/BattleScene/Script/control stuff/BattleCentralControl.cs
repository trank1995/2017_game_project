﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleCentralControl : MonoBehaviour {
    public static bool battleStart, playerTurn;
    public static Grid[,] map;
    public static int gridXMax, gridZMax;
    public static Dictionary<Grid, GameObject> gridToObj;
    public static Dictionary<GameObject, Grid> objToGrid;
    public static List<GameObject> objsChanged { get; set; }
    private void Awake()
    {
        gridToObj = new Dictionary<Grid, GameObject>();
        objToGrid = new Dictionary<GameObject, Grid>();
    }
    // Use this for initialization
    void Start()
    {
        playerTurn = true;
        battleStart = false;
        gridXMax = 100;
        gridZMax = 100;
        map = new Grid[gridXMax, gridZMax];
        generateMap(gridXMax, gridZMax);
        placeOnMap(gridXMax, gridZMax);
        objsChanged = new List<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuScene");
        }

    }

    public static List<GameObject> gridInLine(GameObject start, GameObject end)
    {
        List<GameObject> result = new List<GameObject>();
        RaycastHit[] hits;
        var direction = end.transform.position - start.transform.position;
        var distance = Vector3.Distance(start.transform.position, end.transform.position);
        hits = Physics.RaycastAll(start.transform.position, direction, distance);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            result.Add(hit.transform.gameObject);
            /**
            Renderer rend = hit.transform.GetComponent<Renderer>();

            if (rend)
            {
                // Change the material of all hit colliders
                // to use a transparent shader.
                rend.material.shader = Shader.Find("Transparent/Diffuse");
                Color tempColor = rend.material.color;
                tempColor.a = 0.3F;
                rend.material.color = tempColor;
            }**/
        }
        return result;
    }

    public static void hideCurShowing()
    {
        if (objsChanged.Count > 0)
        {
            foreach (GameObject go in objsChanged)
            {
                recoverColor(go);
            }
        }
    }
    public static void changeColor(GameObject toChange)
    {
        Renderer rend = toChange.transform.Find("Cube").transform.GetComponent<Renderer>();

        if (rend)
        {
            rend.material.shader = Shader.Find("Transparent/Diffuse");
            Color tempColor = rend.material.color;
            tempColor.a = 0.8F;
            rend.material.color = tempColor;
        }
    }
    public static void recoverColor(GameObject toChange)
    {
        Renderer rend = toChange.transform.GetComponent<Renderer>();

        if (rend)
        {
            rend.material.shader = Shader.Find("Transparent/Diffuse");
            Color tempColor = rend.material.color;
            tempColor.a = 1F;
            rend.material.color = tempColor;
        }
    }

    void generateMap(int x, int z)
    {
        for (int ix = 0; ix < x; ix++)
        {
            for (int iz = 0; iz < z; iz++)
            {
                mapGridDecider(ix, iz);
            }
        }
        foreach (Grid g in map)
        {
            for (int dx = 0; dx < 3; dx++)
            {
                for (int dz = 0; dz < 3; dz++)
                {

                    int tx = g.x - 1 + dx;
                    int tz = g.z - 1 + dz;
                    if (tx >= 0 && tx < gridXMax && tz >= 0 && tz < gridZMax)
                    {
                        Grid curG = BattleCentralControl.map[tx, tz];
                        if ((tx != g.x || tz != g.z) && !g.neighbors.Contains(curG))
                        {
                            g.neighbors.Add(curG);
                            
                        }
                        
                    }
                }
            }
            
        }
    }
    void placeOnMap(int x, int z)
    {
        for (int ix = 0; ix < x; ix++)
        {
            for (int iz = 0; iz < z; iz++)
            {
                var pos = new Vector3(ix, 0, iz);
                var rot = new Quaternion(0, 0, 0, 0);
                var obj = Instantiate(map[ix, iz].mapSettingModel, pos, rot);
                gridToObj.Add(map[ix, iz], obj);
                objToGrid.Add(obj, map[ix, iz]);
            }
        }
    }
    GameObject mapGridDecider(int x, int z)
    {
        int rand = (int) Random.Range(1, 4);
        GameObject model;
        if (rand == 1)
        {
            model = GameObject.Find("tree");
            var temp = new Grid(x, z, model, GridType.tree);
            map[x, z] = temp;
        }
        else if (rand == 2)
        {
            model = GameObject.Find("rockAndTree");
            var temp = new Grid(x, z, model, GridType.rockAndTree);
            map[x, z] = temp;
        }
        else
        {
            model = GameObject.Find("flatGrass");
            var temp = new Grid(x, z, model, GridType.flatGrass);
            map[x, z] = temp;
        }
        return model;
    }
}