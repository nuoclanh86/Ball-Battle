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
            //sld.transform.position = Vector3.MoveTowards(sld.transform.position, targetMove, curSpeed * Time.deltaTime);
        }
        else
        {
            reactivateTime -= Time.deltaTime;
        }
    }
}
