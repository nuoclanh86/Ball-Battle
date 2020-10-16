using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAttacker : MonoBehaviour
{
    Color AttackerColor = Color.cyan * 0.9f;
    [HideInInspector]
    public bool isHoldTheBall = false;
    [HideInInspector]
    public float reactivateTime = 0;
    float curSpeed;
    Vector3 targetMove;
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

        //for debug
        this.gameObject.transform.GetChild(0).GetComponent<TextMesh>().text = index + ":" + reactivateTime.ToString("0.0");
    }

    private void LateUpdate()
    {
        SetSoldierMaterial();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("ProcessCollision collision.tag = " + collision.gameObject.tag + " Attacker tag = " + this.gameObject.tag);
        if (collision.gameObject.tag == "Wall" ||
            (collision.gameObject.tag == "GatebaseR" && isHoldTheBall == false))
        {
            KillSoldiersAtt();
        }

        if (collision.gameObject.tag == "Ball")
        {
            int indexSoldierAttChasing = GameManager.Instance.GetTheBall().gameObject.GetComponent<BallController>().indexSoldierAtt_Chasing;
            Debug.Log("ProcessCollision tag=" + this.gameObject.tag + " collision with the ball");
            if ((reactivateTime <= 0 && indexSoldierAttChasing == -1) || indexSoldierAttChasing == index)
            {
                Debug.Log("SoldiersAttCatchTheBall");
                SoldiersAttCatchTheBall();
            }
        }

        if (collision.gameObject.tag == "SoldierDef" && this.gameObject.tag == "SoldierAtt" && isHoldTheBall == false)
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), this.GetComponent<Collider>());
        }

        if (collision.gameObject.tag == "GatebaseR" && this.gameObject.tag == "SoldierAtt" && isHoldTheBall == true)
        {
            GameManager.Instance.GameEnd();
        }
    }

    public void SetSoldierInfo(int m_index)
    {
        index = m_index;
        InitDefaultValue(); // reset value when spawn
    }

    void InitDefaultValue()
    {
        reactivateTime = GameManager.Instance.configScripttableObject.reactivateTimeAtt;
        curSpeed = GameManager.Instance.configScripttableObject.normalSpeedAtt;
        targetMove = GameManager.Instance.GetWallTop().transform.position;
        isHoldTheBall = false;
        this.gameObject.tag = "SoldierAtt";
    }

    public void KillSoldiersAtt()
    {
        //Debug.Log("Kill Soldier index = " + index);
        GameManager.Instance.GetSoldiersAtt()[index] = null;
        index = -1;
        this.gameObject.Kill();
    }

    public void PassTheBallToOTher()
    {
        isHoldTheBall = false;
        reactivateTime = GameManager.Instance.configScripttableObject.reactivateTimeAtt;
        curSpeed = GameManager.Instance.configScripttableObject.normalSpeedAtt;
        targetMove = GameManager.Instance.GetWallTop().transform.position;
        this.transform.localScale /= 1.5f;

        GameManager.Instance.GetTheBall().gameObject.GetComponent<BallController>().MoveToNearestAttacker(this.transform.position);
    }

    public void SoldiersAttCatchTheBall()
    {
        Debug.Log("SoldiersAttCatchTheBall this.tag : " + this.gameObject.tag);
        GameManager.Instance.GetTheBall().GetComponent<BallController>().HideTheBall();
        this.transform.localScale *= 1.5f;
        curSpeed = GameManager.Instance.configScripttableObject.carryingSpeedAtt;
        isHoldTheBall = true;
        targetMove = GameManager.Instance.GetGateBaseR().transform.position;

        GameManager.Instance.GetTheBall().GetComponent<BallController>().indexSoldierAtt_Chasing = -1;
    }

    void SetSoldierMaterial()
    {
        Color statusColor;
        statusColor = AttackerColor;
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
            //Debug.Log("index : " + index + "-reactivateTime = " + reactivateTime);
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
}
