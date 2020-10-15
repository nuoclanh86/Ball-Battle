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
    }

    public void KillSoldiersAtt(GameObject sld)
    {
        //Debug.Log("Kill Soldier index = " + index);
        GameManager.Instance.GetSoldiersAtt()[index] = null;
        index = -1;
        sld.gameObject.tag = "Soldier";
        sld.gameObject.Kill();
    }

    public void PassTheBallToOTher(GameObject sld)
    {
        isHoldTheBall = false;
        reactivateTime = GameManager.Instance.configScripttableObject.reactivateTimeAtt;
        curSpeed = GameManager.Instance.configScripttableObject.normalSpeedAtt;
        sld.gameObject.tag = "Soldier";
        targetMove = GameManager.Instance.GetWallTop().transform.position;
        sld.transform.localScale /= 1.5f;

        GameManager.Instance.GetTheBall().gameObject.GetComponent<BallController>().MoveToNearestAttacker(sld.transform.position);
    }

    public void SoldiersAttCatchTheBall(GameObject sld)
    {
        Debug.Log("SoldiersAttCatchTheBall");
        GameManager.Instance.GetTheBall().GetComponent<BallController>().HideTheBall();
        sld.transform.localScale *= 1.5f;
        curSpeed = GameManager.Instance.configScripttableObject.carryingSpeedAtt;
        isHoldTheBall = true;
        targetMove = GameManager.Instance.GetGateBaseR().transform.position;
        sld.gameObject.tag = "SoldierBall";

        GameManager.Instance.GetTheBall().GetComponent<BallController>().indexSoldierAtt_Chasing = -1;
    }

    protected override void SetSoldierMaterial(GameObject sld)
    {
        Color statusColor;
        statusColor = AttackerColor;
        if (reactivateTime > 0)
        {
            statusColor *= 0.2f;
        }
        sld.GetComponent<Renderer>().material.SetColor("_Color", statusColor);
    }

    protected override void SoldierMove(GameObject sld)
    {
        if (reactivateTime <= 0)
        {
            if (index != GameManager.Instance.GetTheBall().GetComponent<BallController>().indexSoldierAtt_Chasing)
            {
                sld.transform.position = Vector3.MoveTowards(sld.transform.position, targetMove, curSpeed * Time.deltaTime);
            }
        }
        else
        {
            reactivateTime -= Time.deltaTime;
        }

        if (reactivateTime <= 0 && isHoldTheBall == false)
        {
            float dist = Vector3.Distance(sld.transform.position, GameManager.Instance.GetTheBall().transform.position);
            if (dist <= GameManager.Instance.minDistance_Soldier_Ball)
            {
                GameManager.Instance.SetMinDistanceSoldierBall(dist);
                targetMove = GameManager.Instance.GetTheBall().transform.position;
            }
            else
            {
                targetMove = GameManager.Instance.GetWallTop().transform.position;
                targetMove.x = sld.transform.position.x; // go straight to the wall
            }
        }
    }

    protected override void ProcessCollision(Collision collision, GameObject sld)
    {
        //Debug.Log("collision.tag = " + collision.gameObject.tag);
        //Debug.Log("sld.tag = " + sld.gameObject.tag);
        if (collision.gameObject.tag == "Wall")
        {
            KillSoldiersAtt(sld.gameObject);
        }

        if (collision.gameObject.tag == "Ball")
        {
            if (reactivateTime <= 0)
                SoldiersAttCatchTheBall(sld.gameObject);
            else
                Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), sld.GetComponent<Collider>());
        }
        if (collision.gameObject.tag == "Soldier" && sld.gameObject.tag == "Soldier")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), sld.GetComponent<Collider>());
        }
    }
}
