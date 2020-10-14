using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameSetting")]
public class ConfigScriptableObject : ScriptableObject
{
    [Header("GameConfig")]
    public int matchPerGame;
    public int timeLimit;
    public int maxEnergyBar;
    
    [Header("Attacker")]
    public float energyRegenerationAtt;
    public float energyCostAtt;
    public float spawnTimeAtt;
    public float reactivateTimeAtt;
    public float normalSpeedAtt;
    public float carryingSpeedAtt;
    public float ballSpeedAtt;
    //public float returnSpeedAtt;
    //public float detectionRangeAtt;

	[Header("Defender")]
    public float energyRegenerationDef;
    public float energyCostDef;
    public float spawnTimeDef;
    public float reactivateTimeDef;
    public float normalSpeedDef;
    //public float carryingSpeedDef;
    //public float ballSpeedDef;
    public float returnSpeedDef;
    public float detectionRangeDef;

    [Header("DeveloperSetting")]
    public int maxArray;
}
