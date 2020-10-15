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
    }

    protected override void SetSoldierMaterial(GameObject sld)
    {
        Color statusColor;
        statusColor = DefenderColor;
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
            Vector3 nearestEnemyPos = FindNearestEnemyInRange(sld);
            if (nearestEnemyPos != Vector3.zero)
            {
                targetMove = nearestEnemyPos;
                sld.transform.position = Vector3.MoveTowards(sld.transform.position, targetMove, curSpeed * Time.deltaTime);
            }
        }
        else
        {
            reactivateTime -= Time.deltaTime;
        }
    }

    Vector3 FindNearestEnemyInRange(GameObject sld)
    {
        Vector3 found = Vector3.zero;
        for (int i = 0; i <= GameManager.Instance.configScripttableObject.maxArray; i++)
        {
            if (GameManager.Instance.GetSoldiersAtt()[i] == null)
                break;
            if (GameManager.Instance.GetSoldiersAtt()[i].gameObject.GetComponent<SoldierAttacker>().isHoldTheBall)
            {
                float dist = Vector3.Distance(sld.transform.position, GameManager.Instance.GetSoldiersAtt()[i].transform.position);
                if (dist <= GameManager.Instance.detectionRangeDefFloat)
                {
                    found = GameManager.Instance.GetSoldiersAtt()[i].transform.position;
                }
            }
        }
        return found;
    }

    protected override void ProcessCollision(Collision collision, GameObject sld)
    {
        //Debug.Log("collision.tag = " + collision.gameObject.tag);
        //Debug.Log("sld.tag = " + sld.gameObject.tag);
        if (collision.gameObject.tag == "SoldierBall")
        {
            reactivateTime = GameManager.Instance.configScripttableObject.reactivateTimeDef;
            targetMove = originalPos;
        }
    }
}
