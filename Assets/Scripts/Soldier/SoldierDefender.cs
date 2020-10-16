using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierDefender : SoldierControler
{
    Vector3 originalPos;
    Color DefenderColor = Color.red * 0.9f;

    public void SetSoldierInfo(int m_index, Vector3 pos)
    {
        index = m_index;
        originalPos = pos;
        InitDefaultValue(); // reset value when spawn
    }

    protected override void InitDefaultValue()
    {
        reactivateTime = GameManager.Instance.configScripttableObject.reactivateTimeDef;
        curSpeed = GameManager.Instance.configScripttableObject.normalSpeedDef;
        targetMove = originalPos;
        team = Team.Defender;
    }

    protected override void SetSoldierMaterial()
    {
        Color statusColor;
        statusColor = DefenderColor;
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
            this.gameObject.tag = "SoldierDef";
            Vector3 nearestEnemyPos = FindNearestEnemyInRange();
            if (nearestEnemyPos != Vector3.zero)
            {
                targetMove = nearestEnemyPos;
                curSpeed = GameManager.Instance.configScripttableObject.normalSpeedDef;
                this.transform.position = Vector3.MoveTowards(this.transform.position, targetMove, curSpeed * Time.deltaTime);
            }
        }
        else
        {
            reactivateTime -= Time.deltaTime;
            if (this.transform.position != originalPos)
            {
                originalPos.y = this.transform.position.y; // dont care if it's higher
                // move back to originalPos
                curSpeed = GameManager.Instance.configScripttableObject.returnSpeedDef;
                this.transform.position = Vector3.MoveTowards(this.transform.position, originalPos, curSpeed * Time.deltaTime);
            }
        }
    }

    Vector3 FindNearestEnemyInRange()
    {
        Vector3 found = Vector3.zero;
        for (int i = 0; i <= GameManager.Instance.configScripttableObject.maxArray; i++)
        {
            //Debug.Log("FindNearestEnemyInRange " + i);
            if (GameManager.Instance.GetSoldiersAtt()[i] == null)
                break;
            if (GameManager.Instance.GetSoldiersAtt()[i].gameObject.GetComponent<SoldierAttacker>().isHoldTheBall)
            {
                Debug.Log("FindNearestEnemyInRange attacker " + i + " isHoldTheBall");
                float dist = Vector3.Distance(this.transform.position, GameManager.Instance.GetSoldiersAtt()[i].transform.position);
                if (dist <= GameManager.Instance.detectionRangeDefFloat)
                {
                    found = GameManager.Instance.GetSoldiersAtt()[i].transform.position;
                }
            }
        }
        return found;
    }

    protected override void ProcessCollision(Collision collision)
    {
        //Debug.Log("collision.tag = " + collision.gameObject.tag);
        //Debug.Log("sld.tag = " + sld.gameObject.tag);
        if (collision.gameObject.tag == "SoldierBall")
        {
            Debug.Log("collision with SoldierBall");
            reactivateTime = GameManager.Instance.configScripttableObject.reactivateTimeDef;
            targetMove = originalPos;
            collision.gameObject.GetComponent<SoldierAttacker>().PassTheBallToOTher();
        }
    }
}
