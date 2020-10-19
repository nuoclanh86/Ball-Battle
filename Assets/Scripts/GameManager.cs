using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates
{
    Idle,
    Playing,
    AttackerLose,
    AttackerWin,
    Draw
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
    public GameObject uiCanVas;

    bool isInAR = false;

    public GameObject mazeLoader;

    GameObject theBall;
    GameObject[] soldiersAtt;
    GameObject[] soldiersDef;
    GameObject wallTop;
    GameObject wallMid;
    GameObject wallLeft;
    GameObject wallRight;
    GameObject gateBaseR;

    GameStates gameState;

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

        gameState = GameStates.Idle;

        isInAR = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) &&
            (gameState != GameStates.AttackerLose || gameState != GameStates.AttackerWin || gameState != GameStates.Draw))
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
            if (uiCanVas.GetComponent<UIController>().energyPlayerValue > this.configScripttableObject.energyCostAtt)
            {
                for (i = 0; i < configScripttableObject.maxArray; i++)
                {
                    if (soldiersAtt[i] == null)
                    {
                        break;
                    }
                }
                soldiersAtt[i] = soldierAttPrefab.Spawn(spawnPos);
                soldiersAtt[i].GetComponent<SoldierAttacker>().SetSoldierInfo(i);
                uiCanVas.GetComponent<UIController>().energyPlayerValue -= this.configScripttableObject.energyCostAtt;
            }
        }
        else
        {
            if (uiCanVas.GetComponent<UIController>().energyEnemyValue > this.configScripttableObject.energyCostDef)
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
                uiCanVas.GetComponent<UIController>().energyEnemyValue -= this.configScripttableObject.energyCostDef;
            }
        }
        gameState = GameStates.Playing;
    }

    public void GameEnd(GameStates gs)
    {
        Debug.Log("GameEnd : " + gs);
        gameState = gs;
        theBall.GetComponent<BallController>().HideTheBall();
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
        uiCanVas.GetComponent<UIController>().ShowResultDisplay(gs);
    }

    public void ChangeToMaze()
    {
        Debug.Log("ChangeToMaze");
        foreach (GameObject soldierAtt in soldiersAtt)
        {
            if (soldierAtt != null)
                soldierAtt.Kill();
        }
        foreach (GameObject soldierDef in soldiersDef)
        {
            if (soldierDef != null)
                soldierDef.Kill();
        }
        uiCanVas.SetActive(false);
        wallMid.SetActive(false);
        gameState = GameStates.Draw;

        mazeLoader.GetComponent<MazeLoader>().CreateMaze();
        float deltaRange = 2.5f; // do not spawn too nearly walls
        float ran_x = Random.Range(wallLeft.transform.position.x + deltaRange, wallRight.transform.position.x - deltaRange);
        float ran_z = Random.Range(-wallTop.transform.position.z + deltaRange, 0.0f - deltaRange);
        theBall.transform.position = new Vector3(ran_x, 0.5f, ran_z);
        ran_x = Random.Range(wallLeft.transform.position.x + deltaRange, wallRight.transform.position.x - deltaRange);
        ran_z = Random.Range(-wallTop.transform.position.z + deltaRange, 0.0f - deltaRange);
        GameObject soldiersInMaze = soldierAttPrefab.Spawn(new Vector3(ran_x, 2.5f, ran_z));
    }

    public void ChangeToAR()
    {
        Debug.Log("ChangeToAR");
        if (isInAR == true)
        {
            SceneManager.LoadScene(0);
            isInAR = false;
        }
        else
        {
            SceneManager.LoadScene(1);
            isInAR = true;
        }
    }
}
