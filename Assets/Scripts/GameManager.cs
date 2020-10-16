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
    public GameObject soldierAttPrefab;
    public GameObject soldierDefPrefab;
    public GameObject theBallPrefab;

    GameObject theBall;
    GameObject[] soldiersAtt;
    GameObject[] soldiersDef;
    GameObject wallTop;
    GameObject wallMid;
    GameObject wallLeft;
    GameObject wallRight;
    GameObject gateBaseR;

    [HideInInspector]
    public float minDistance_Soldier_Ball = 99999;
    [HideInInspector]
    public float detectionRangeDefFloat;

    // Start is called before the first frame update
    void Start()
    {
        wallTop = battleFieldPrefab.transform.GetChild(0).transform.GetChild(1).gameObject;
        wallMid = battleFieldPrefab.transform.GetChild(0).transform.GetChild(2).gameObject;
        wallRight = battleFieldPrefab.transform.GetChild(0).transform.GetChild(3).gameObject;
        wallLeft = battleFieldPrefab.transform.GetChild(0).transform.GetChild(4).gameObject;
        gateBaseR = battleFieldPrefab.transform.GetChild(2).gameObject;
        float deltaRange = 0.5f; // do not spawn too nearly walls
        float ran_x = Random.Range(wallLeft.transform.position.x + deltaRange, wallRight.transform.position.x - deltaRange);
        float ran_z = Random.Range(-wallTop.transform.position.z + deltaRange, 0.0f - deltaRange);
        theBall = Instantiate(theBallPrefab, new Vector3(ran_x, 0.5f, ran_z), Quaternion.identity);

        soldiersAtt = new GameObject[configScripttableObject.maxArray];
        soldiersDef = new GameObject[configScripttableObject.maxArray];

        detectionRangeDefFloat = configScripttableObject.detectionRangeDef * Vector3.Distance(wallLeft.transform.position, wallRight.transform.position);
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
                //Debug.Log("spawnPos at " + spawnPos);
                SpawnSoldier(spawnPos);
            }
        }
    }

    public GameObject[] GetSoldiersAtt()
    {
        return soldiersAtt;
    }

    public GameObject[] GetsoldiersDef()
    {
        return soldiersDef;
    }

    public GameObject GetWallTop()
    {
        return wallTop;
    }

    public GameObject GetTheBall()
    {
        return theBall;
    }

    public GameObject GetGateBaseR()
    {
        return gateBaseR;
    }

    public void SetMinDistanceSoldierBall(float dis)
    {
        minDistance_Soldier_Ball = dis;
    }

    void SpawnSoldier(Vector3 spawnPos)
    {
        int i = 0;
        spawnPos.y = 1.0f; // spawn on the plane
        if (spawnPos.z < wallMid.transform.position.z)
        {
            for (i = 0; i < configScripttableObject.maxArray; i++)
            {
                if (soldiersAtt[i] == null)
                {
                    break;
                }
            }
            soldiersAtt[i] = soldierAttPrefab.Spawn(spawnPos);
            soldiersAtt[i].gameObject.GetComponent<SoldierAttacker>().SetSoldierInfo(i);
        }
        else
        {
            for (i = 0; i < configScripttableObject.maxArray; i++)
            {
                if (soldiersDef[i] == null)
                {
                    break;
                }
            }
            soldiersDef[i] = soldierDefPrefab.Spawn(spawnPos);
            soldiersDef[i].gameObject.GetComponent<SoldierDefender>().SetSoldierInfo(i, soldiersDef[i].transform.position);
        }
    }

    public void GameEnd()
    {
        Debug.Log("GameEnd");
        theBall.gameObject.GetComponent<BallController>().HideTheBall();
        foreach (GameObject soldierAtt in soldiersAtt)
        {
            if (soldierAtt != null)
                soldierAtt.GetComponent<SoldierAttacker>().reactivateTime = 99.0f;
        }
        foreach (GameObject soldierDef in soldiersDef)
        {
            if (soldierDef != null)
                soldierDef.GetComponent<SoldierDefender>().reactivateTime = 99.0f;
        }
    }
}
