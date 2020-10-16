using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAttacker : SoldierControler
{
    Color AttackerColor = Color.cyan * 0.9f;
    public bool isHoldTheBall = false;

    public void SetSoldierInfo(int m_index)
    {
        index = m_index;
        InitDefaultValue(); // reset value when spawn
    }

    protected override void InitDefaultValue()
    {
        reactivateTime = GameManager.Instance.configScripttableObject.reactivateTimeAtt;
        curSpeed = GameManager.Instance.configScripttableObject.normalSpeedAtt;
        targetMove = GameManager.Instance.GetWallTop().transform.position;
        isHoldTheBall = false;
        team = Team.Attacker;
    }

    public void KillSoldiersAtt(GameObject sld)
    {
        //Debug.Log("Kill Soldier index = " + index);
        GameManager.Instance.GetSoldiersAtt()[index] = null;
        index = -1;
        sld.gameObject.tag = "SoldierAtt";
        sld.gameObject.Kill();
    }

    public void PassTheBallToOTher()
    {
        isHoldTheBall = false;
        reactivateTime = GameManager.Instance.configScripttableObject.reactivateTimeAtt;
        curSpeed = GameManager.Instance.configScripttableObject.normalSpeedAtt;
        this.gameObject.tag = "SoldierAtt";
        targetMove = GameManager.Instance.GetWallTop().transform.position;
        this.transform.localScale /= 1.5f;

        GameManager.Instance.GetTheBall().gameObject.GetComponent<BallController>().MoveToNearestAttacker(this.transform.position);
    }

    public void SoldiersAttCatchTheBall()
    {
        Debug.Log("SoldiersAttCatchTheBall");
        GameManager.Instance.GetTheBall().GetComponent<BallController>().HideTheBall();
        this.transform.localScale *= 1.5f;
        print("sld.transform.localScale = " + this.transform.localScale);
        curSpeed = GameManager.Instance.configScripttableObject.carryingSpeedAtt;
        isHoldTheBall = true;
        targetMove = GameManager.Instance.GetGateBaseR().transform.position;
        this.gameObject.tag = "SoldierBall";

        GameManager.Instance.GetTheBall().GetComponent<BallController>().indexSoldierAtt_Chasing = -1;
    }

    protected override void SetSoldierMaterial()
    {
        Color statusColor;
        statusColor = AttackerColor;
        if (reactivateTime > 0)
        {
            statusColor *= 0.2f;
        }
        this.GetComponent<Renderer>().material.SetColor("_Color", statusColor);
    }

    protected override void SoldierMove()
    {
        if (reactivateTime <= 0)
        {
            if (index != GameManager.Instance.GetTheBall().GetComponent<BallController>().indexSoldierAtt_Chasing)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, targetMove, curSpeed * Time.deltaTime);
            }
        }
        else
        {
            reactivateTime -= Time.deltaTime;
        }

        if (reactivateTime <= 0 && isHoldTheBall == false)
        {
            float dist = Vector3.Distance(this.transform.position, GameManager.Instance.GetTheBall().transform.position);
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

    protected override void ProcessCollision(Collision collision)
    {
        //Debug.Log("collision.tag = " + collision.gameObject.tag);
        //Debug.Log("sld.tag = " + sld.gameObject.tag);
        if (collision.gameObject.tag == "Wall")
        {
            KillSoldiersAtt(this.gameObject);
        }

        if (collision.gameObject.tag == "Ball")
        {
            if (reactivateTime <= 0)
                SoldiersAttCatchTheBall();
            else
                Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), this.GetComponent<Collider>());
        }
        if (collision.gameObject.tag == "SoldierDef" && this.gameObject.tag == "SoldierAtt")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), this.GetComponent<Collider>());
        }
    }
}
