using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    Attacker,
    Defender
}

//should have 
    //class Soldier
    //class SoldierAttacker : Soldier
    //class SoldierDefender : Soldier

public class SoldierControler : MonoBehaviour
{
    float reactivateTime = 0;
    float curSpeed;
    Vector3 targetMove;
    int index;
    Team team;
    Color AttackerColor = Color.cyan * 0.9f;    
    Color DefenderColor = Color.red * 0.9f;

    // Start is called before the first frame update
    void Start()
    {
        InitDefaultValue();
    }

    // Update is called once per frame
    void Update()
    {
        if (reactivateTime <= 0)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetMove, curSpeed * Time.deltaTime);
        }
        else
        {
            reactivateTime -= Time.deltaTime;
        }

        float dist = Vector3.Distance(this.transform.position, GameManager.Instance.GetTheBall().transform.position);

        if (reactivateTime <= 0)
        {
            if (dist <= GameManager.Instance.minDistance_Soldier_Ball)
            {
                GameManager.Instance.SetMinDistanceSoldierBall(dist);
                targetMove = GameManager.Instance.GetTheBall().transform.position;
            }
            else
            {
                targetMove = GameManager.Instance.GetWallTop().transform.position;
                targetMove.x = this.transform.position.x; // go straight to the wall
            }
        }
    }

    private void LateUpdate()
    {
        Color statusColor;
        if (team == Team.Attacker)
        {
            statusColor = AttackerColor;
        }
        else
        {
            statusColor = DefenderColor;
        }
        if (reactivateTime > 0)
        {
            statusColor *= 0.2f;
        }
        this.GetComponent<Renderer>().material.SetColor("_Color", statusColor);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("OnCollisionEnter : " + collision.gameObject.name);
        if (team == Team.Attacker)
        {
            if (collision.gameObject.tag == "Wall")
            {
                Debug.Log("Kill Soldier index = " + index);
                GameManager.Instance.GetSoldiersAtt()[index] = null;
                index = -1;
                this.gameObject.Kill();
            }

            if (collision.gameObject.tag == "Ball")
            {
                GameManager.Instance.HideTheBall();
                this.transform.localScale *= 1.5f;
                curSpeed = GameManager.Instance.configScripttableObject.carryingSpeedAtt;
            }
        }
    }

    void InitDefaultValue()
    {
        reactivateTime = GameManager.Instance.configScripttableObject.reactivateTimeAtt;
        curSpeed = GameManager.Instance.configScripttableObject.normalSpeedAtt;
        targetMove = GameManager.Instance.GetWallTop().transform.position;
    }

    public void SetSoldierInfo(int m_index, Team m_team)
    {
        index = m_index;
        team = m_team;
        InitDefaultValue(); // reset value when spawn
    }
}
