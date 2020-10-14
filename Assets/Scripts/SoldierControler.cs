using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    Attacker,
    Defender
}

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
        reactivateTime = GameManager.Instance.configScripttableObject.reactivateTimeAtt;
        curSpeed = GameManager.Instance.configScripttableObject.normalSpeedAtt;
        targetMove = GameManager.Instance.GetWallTop().transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (reactivateTime <= 0)
        {
            if (team == Team.Attacker)
            {
                targetMove.x = this.transform.position.x;
                this.transform.position = Vector3.MoveTowards(this.transform.position, targetMove,
                                                                    curSpeed * Time.deltaTime);
            }
        }
        else
        {
            reactivateTime -= Time.deltaTime;
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
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("Kill Soldier index = " + index);
            GameManager.Instance.GetSoldiersAtt()[index] = null;
            index = -1;
            this.gameObject.Kill();
        }
    }

    public void SetTargetMove(Vector3 targetPos)
    {
        targetMove = targetPos;
    }

    public void SetSoldierInfo(int m_index, Team m_team)
    {
        index = m_index;
        team = m_team;
    }
}
