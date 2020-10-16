using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    Vector3 targetMove;
    float curSpeed;
    public int indexSoldierAtt_Chasing = -1;

    // Start is called before the first frame update
    void Start()
    {
        targetMove = Vector3.zero;
        indexSoldierAtt_Chasing = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetMove != Vector3.zero)
        {
            //Debug.Log("ball move to : " + targetMove);
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetMove, curSpeed * Time.deltaTime);
        }
    }
    
    public void HideTheBall()
    {
        this.gameObject.transform.position = new Vector3(99.0f, 99.0f, 99.0f); //move it out of the screen
        targetMove = Vector3.zero;
        curSpeed = 0.0f;
    }

    public void MoveToNearestAttacker(Vector3 curAttackerPos)
    {
        this.gameObject.transform.position = curAttackerPos;
        targetMove = FindNearestAttacker();
        curSpeed = GameManager.Instance.configScripttableObject.ballSpeedAtt;

        if (targetMove == Vector3.zero)
            GameManager.Instance.GameEnd();
    }

    Vector3 FindNearestAttacker()
    {
        Vector3 found = Vector3.zero;
        float minDist = 9999.0f;
        foreach (GameObject soldierAtt in GameManager.Instance.GetSoldiersAtt())
        {
            if (soldierAtt != null && soldierAtt.GetComponent<SoldierAttacker>().reactivateTime <= 0)
            {
                float dist = Vector3.Distance(this.transform.position, soldierAtt.transform.position);
                if (dist <= minDist)
                {
                    minDist = dist;
                    found = soldierAtt.transform.position;
                    indexSoldierAtt_Chasing = soldierAtt.GetComponent<SoldierAttacker>().index;
                }
            }
        }
        return found;
    }
}
