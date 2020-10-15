using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAttacker : SoldierControler
{
    Color AttackerColor = Color.cyan * 0.9f;

    public void SetSoldierInfo(int m_index)
    {
        index = m_index;
        InitDefaultValue(); // reset value when spawn
    }

    public void KillSoldiersAtt(GameObject sld)
    {
        Debug.Log("Kill Soldier index = " + index);
        GameManager.Instance.GetSoldiersAtt()[index] = null;
        index = -1;
        sld.gameObject.Kill();
    }

    public void SoldiersAttCatchTheBall(GameObject sld)
    {
        GameManager.Instance.HideTheBall();
        sld.transform.localScale *= 1.5f;
        curSpeed = GameManager.Instance.configScripttableObject.carryingSpeedAtt;
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
            sld.transform.position = Vector3.MoveTowards(sld.transform.position, targetMove, curSpeed * Time.deltaTime);
        }
        else
        {
            reactivateTime -= Time.deltaTime;
        }

        float dist = Vector3.Distance(sld.transform.position, GameManager.Instance.GetTheBall().transform.position);

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
                targetMove.x = sld.transform.position.x; // go straight to the wall
            }
        }
    }

    protected override void InitDefaultValue()
    {
        reactivateTime = GameManager.Instance.configScripttableObject.reactivateTimeAtt;
        curSpeed = GameManager.Instance.configScripttableObject.normalSpeedAtt;
        targetMove = GameManager.Instance.GetWallTop().transform.position;
    }

    protected override void ProcessCollision(Collision collision, GameObject sld)
    {
        if (collision.gameObject.tag == "Wall")
        {
            KillSoldiersAtt(sld.gameObject);
        }

        if (collision.gameObject.tag == "Ball")
        {
            SoldiersAttCatchTheBall(sld.gameObject);
        }
    }
}
