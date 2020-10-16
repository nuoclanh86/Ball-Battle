using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    Vector3 targetMove;
    float curSpeed;
    [HideInInspector]
    public int indexSoldierAtt_Chasing = -1;
    [HideInInspector]
    GameObject soldierAtt_Chasing;

    // Start is called before the first frame update
    void Start()
    {
        targetMove = Vector3.zero;
        indexSoldierAtt_Chasing = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (indexSoldierAtt_Chasing != -1 && soldierAtt_Chasing!= null)
        {
            targetMove = soldierAtt_Chasing.transform.position;
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetMove, curSpeed * Time.deltaTime);
        }
        if (indexSoldierAtt_Chasing != -1 && soldierAtt_Chasing == null)
        {
            Debug.Log("BallController has error !!!");
        }
    }

    public void HideTheBall()
    {
        this.gameObject.transform.position = new Vector3(99.0f, 99.0f, 99.0f); //move it out of the screen
        targetMove = Vector3.zero;
        curSpeed = 0.0f;
        indexSoldierAtt_Chasing = -1;
        soldierAtt_Chasing = null;
    }

    public void MoveToNearestAttacker(Vector3 curAttackerPos)
    {
        this.gameObject.transform.position = curAttackerPos;
        soldierAtt_Chasing = FindNearestAttacker();
        curSpeed = GameManager.Instance.configScripttableObject.ballSpeedAtt;

        if (soldierAtt_Chasing == null)
        {
            Debug.Log("BallController cant found any Attacker");
            GameManager.Instance.GameEnd();
        }
    }

    GameObject FindNearestAttacker()
    {
        GameObject found = null;
        float minDist = 9999.0f;
        foreach (GameObject soldierAtt in GameManager.Instance.GetSoldiersAtt())
        {
            if (soldierAtt != null && soldierAtt.GetComponent<SoldierAttacker>().reactivateTime <= 0)
            {
                float dist = Vector3.Distance(this.transform.position, soldierAtt.transform.position);
                if (dist <= minDist)
                {
                    minDist = dist;
                    found = soldierAtt;
                    indexSoldierAtt_Chasing = soldierAtt.GetComponent<SoldierAttacker>().index;
                }
            }
        }
        return found;
    }
}
