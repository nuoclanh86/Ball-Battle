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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "SoldierAtt" && collision.gameObject.GetComponent<SoldierAttacker>().index == indexSoldierAtt_Chasing
            && collision.gameObject.GetComponent<SoldierAttacker>().team == Team.Attacker)
        {
            //collision.gameObject.GetComponent<SoldierAttacker>().SoldiersAttCatchTheBall(collision.gameObject);
        }
        else
        {
            Physics.IgnoreCollision(this.GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
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
    }

    Vector3 FindNearestAttacker()
    {
        Vector3 found = Vector3.zero;
        float minDist = 9999.0f;
        for (int i = 0; i <= GameManager.Instance.configScripttableObject.maxArray; i++)
        {
            if (GameManager.Instance.GetSoldiersAtt()[i] == null)
                break;
            if (GameManager.Instance.GetSoldiersAtt()[i].GetComponent<SoldierAttacker>().reactivateTime <= 0)
            {
                float dist = Vector3.Distance(this.transform.position, GameManager.Instance.GetSoldiersAtt()[i].transform.position);
                if (dist <= minDist)
                {
                    minDist = dist;
                    found = GameManager.Instance.GetSoldiersAtt()[i].transform.position;
                    indexSoldierAtt_Chasing = GameManager.Instance.GetSoldiersAtt()[i].GetComponent<SoldierAttacker>().index;
                }
            }
        }
        if (found == Vector3.zero)
            print("FindNearestAttacker fail.");
        return found;
    }
}
