using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierControler : MonoBehaviour
{
    public ConfigScriptableObject configScripttableObject;

    //public GameObject wall;
    Vector3 wallPos = new Vector3(10.0f, 0.0f, 20.0f);

    float reactivateTime = 0;
    float curSpeed;
    Vector3 targetMove;
    int index;

    // Start is called before the first frame update
    void Start()
    {
        reactivateTime = configScripttableObject.reactivateTimeAtt;
        curSpeed = configScripttableObject.normalSpeedAtt;
        targetMove = wallPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (reactivateTime <= 0)
        {
            wallPos.x = this.transform.position.x;
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetMove,
                                                                curSpeed * Time.deltaTime);
        }
        else
            reactivateTime -= Time.deltaTime;
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

    public void SetIndex(int i)
    {
        index = i;
    }
    public int GetIndex()
    {
        return index;
    }

}
