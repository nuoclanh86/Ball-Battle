﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GameStates
{
    Idle,
    Playing,
    PlayerLose,
    PlayerWin
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    void Awake()
    {
        // Save a reference to the AudioManager component as our //singleton instance.
        Instance = this;
    }

    public ConfigScriptableObject configScripttableObject;

    public GameObject battleFieldPrefab;
    public GameObject soldierPrefab;
    public GameObject theBallPrefab;

    GameObject theBall;
    GameObject[] soldiersAtt;

    // Start is called before the first frame update
    void Start()
    {
        GameObject wallBot = battleFieldPrefab.transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject wallRight = battleFieldPrefab.transform.GetChild(0).transform.GetChild(3).gameObject;
        float deltaRange = 0.5f; // do not spawn too nearly walls
        float ran_x = Random.Range(-wallRight.transform.position.x + deltaRange, wallRight.transform.position.x - deltaRange);
        float ran_z = Random.Range(wallBot.transform.position.z + deltaRange, 0.0f - deltaRange);
        theBall = Instantiate(theBallPrefab, new Vector3(ran_x, 0.5f, ran_z), Quaternion.identity);

        soldiersAtt = new GameObject[configScripttableObject.maxArray];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 spawnPos = hit.point;
                spawnPos.y = 1.0f; // spawn on the plane
                int i = 0;
                for (i = 0; i < configScripttableObject.maxArray; i++)
                {
                    if (soldiersAtt[i] == null)
                    {
                        Debug.Log("soldiersAtt " + i + " null");
                        break;
                    }
                }
                soldiersAtt[i] = soldierPrefab.Spawn(spawnPos);
                soldiersAtt[i].gameObject.GetComponent<SoldierControler>().SetIndex(i);
            }
        }
    }

    public GameObject[] GetSoldiersAtt()
    {
        return soldiersAtt;
    }
}
