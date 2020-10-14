using System.Collections;
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
    public GameObject battleFieldPrefab;
    public GameObject soldierPrefab;
    public GameObject theBallPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameObject wallBot = battleFieldPrefab.transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject wallRight = battleFieldPrefab.transform.GetChild(0).transform.GetChild(3).gameObject;
        float deltaRange = 0.5f; // do not spawn too nearly walls
        float ran_x = Random.Range(-wallRight.transform.position.x + deltaRange, wallRight.transform.position.x - deltaRange);
        float ran_z = Random.Range(wallBot.transform.position.z + deltaRange, 0.0f - deltaRange);
        Instantiate(theBallPrefab, new Vector3(ran_x, 0.5f, ran_z), Quaternion.identity);
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
                soldierPrefab.Spawn(spawnPos);
            }
        }
    }
}
