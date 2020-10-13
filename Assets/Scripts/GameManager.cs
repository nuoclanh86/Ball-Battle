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
    public GameObject soldierPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
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
                //Instantiate(soldierPrefab, hit.point, Quaternion.identity);
                soldierPrefab.Spawn(hit.point);
            }
        }
    }
}
