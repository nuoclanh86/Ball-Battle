using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierDefender : MonoBehaviour
{
    Vector3 originalPos;
    Color DefenderColor = Color.red * 0.9f;

    [HideInInspector]
    public float reactivateTime = 0;
    protected float curSpeed;
    protected Vector3 targetMove;
    [HideInInspector]
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        InitDefaultValue();
    }

    // Update is called once per frame
    void Update()
    {
        SoldierMove();
    }

    private void LateUpdate()
    {
        SetSoldierMaterial();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("collision.tag = " + collision.gameObject.tag);
        //Debug.Log("sld.tag = " + sld.gameObject.tag);
        if (collision.gameObject.tag == "SoldierAtt" && collision.gameObject.GetComponent<SoldierAttacker>().isHoldTheBall == true)
        {
            Debug.Log("collision with Soldier holding Ball");
            reactivateTime = GameManager.Instance.configScripttableObject.reactivateTimeDef;
            targetMove = originalPos;
            collision.gameObject.GetComponent<SoldierAttacker>().PassTheBallToOTher();
        }
    }

    public void SetSoldierInfo(int m_index, Vector3 pos)
    {
        index = m_index;
        originalPos = pos;
        InitDefaultValue(); // reset value when spawn
    }

    void InitDefaultValue()
    {
        reactivateTime = GameManager.Instance.configScripttableObject.reactivateTimeDef;
        curSpeed = GameManager.Instance.configScripttableObject.normalSpeedDef;
        targetMove = originalPos;
        this.gameObject.tag = "SoldierDef";
    }

    void SetSoldierMaterial()
    {
        Color statusColor;
        statusColor = DefenderColor;
        if (reactivateTime > 0)
        {
            statusColor *= 0.2f;
        }
        this.GetComponent<Renderer>().material.SetColor("_Color", statusColor);
    }

    void SoldierMove()
    {
        if (reactivateTime <= 0)
        {
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
        foreach (GameObject soldierAtt in GameManager.Instance.GetSoldiersAtt())
        {
            if (soldierAtt != null && soldierAtt.GetComponent<SoldierAttacker>().isHoldTheBall)
            {
                //Debug.Log("FindNearestEnemyInRange attacker " + i + " isHoldTheBall");
                float dist = Vector3.Distance(this.transform.position, soldierAtt.transform.position);
                if (dist <= GameManager.Instance.detectionRangeDefFloat)
                {
                    found = soldierAtt.transform.position;
                }
            }
        }
        return found;
    }
}