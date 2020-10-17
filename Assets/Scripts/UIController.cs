using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    Text txtTimeLeft;
    float valTimeLeft;
    GameObject resultDisplay;

    [HideInInspector]
    public float energyEnemyValue, energyPlayerValue;
    GameObject energyEnemy, energyPlayer;

    // Start is called before the first frame update
    void Start()
    {
        InitUICanvas();
    }

    // Update is called once per frame
    void Update()
    {
        if (valTimeLeft > 0)
        {
            valTimeLeft -= Time.deltaTime;
            txtTimeLeft.text = ((int)valTimeLeft).ToString();
        }
        else
        {
            Debug.Log("Time out . Game end with Draw");
            GameManager.Instance.GameEnd(GameStates.Draw);
        }

        UpdateEnergy(ref energyPlayerValue, energyPlayer, GameManager.Instance.configScripttableObject.energyRegenerationAtt);
        UpdateEnergy(ref energyEnemyValue, energyEnemy, GameManager.Instance.configScripttableObject.energyRegenerationDef);
    }

    void InitUICanvas()
    {
        foreach (Transform child in this.transform)
        {
            //Debug.Log("child name = " + child.name);
            if (child.name == "TimePanel")
            {
                txtTimeLeft = child.GetChild(0).GetComponent<UnityEngine.UI.Text>();
                valTimeLeft = GameManager.Instance.configScripttableObject.timeLimit;
                txtTimeLeft.text = ((int)valTimeLeft).ToString();
            }
            else if (child.name == "ResultDisplay")
            {
                resultDisplay = child.gameObject;
                resultDisplay.SetActive(false);
            }
            else if (child.name == "EnergyEnemy")
            {
                energyEnemy = child.gameObject;
                energyEnemyValue = 0;
            }
            else if (child.name == "EnergyPlayer")
            {
                energyPlayer = child.gameObject;
                energyPlayerValue = 0;
            }
        }
    }

    void UpdateEnergy(ref float energyValue, GameObject energyBar, float energyRegeneration)
    {
        if (energyValue < 6.0f)
            energyValue += (Time.deltaTime * energyRegeneration);

        float temp = energyValue;
        int i = 0;
        float[] val = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
        while (temp >= 0.0f && i < 6)
        {
            if (temp >= 1)
                val[i] = 1.0f;
            else
                val[i] = temp;
            temp -= 1.0f;
            i++;
        }
        for (i = 0; i < 6; i++)
            energyBar.transform.GetChild(i).GetComponent<Slider>().value = val[i];
    }

    public void ShowResultDisplay(GameStates gs)
    {
        if (gs == GameStates.AttackerWin)
        {
            resultDisplay.SetActive(true);
            Sprite sp = Resources.Load<Sprite>("Textures/youwin") as Sprite;
            resultDisplay.GetComponent<Image>().sprite = sp;
        }
        else if (gs == GameStates.AttackerLose)
        {
            resultDisplay.SetActive(true);
            Sprite sp = Resources.Load<Sprite>("Textures/youlose") as Sprite;
            resultDisplay.GetComponent<Image>().sprite = sp;
        }
        else // if (gs == GameStates.Draw)
        {
            resultDisplay.SetActive(true);
            Sprite sp = Resources.Load<Sprite>("Textures/draw") as Sprite;
            resultDisplay.GetComponent<Image>().sprite = sp;
        }
    }
}
