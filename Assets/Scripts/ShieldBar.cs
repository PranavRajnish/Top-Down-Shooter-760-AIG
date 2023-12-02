using Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{
    public Slider slider;
    public GameObject playerGO;

    private CharacterDefenseStats playerStats;

    private void Start()
    {
        CharacterDefenseStats stats = playerGO.GetComponent<CharacterDefenseStats>();
        if(stats)
        {
            stats.OnCharacterStatUpdate += OnCharacterHit;
            playerStats = stats;
        }
    }
    void Update()
    {
        
    }


    private void OnCharacterHit()
    {
        slider.value = playerStats.NormalizedArmor;
    }

    private void OnDestroy()
    {
        if(playerStats)
        {
            playerStats.OnCharacterStatUpdate -= OnCharacterHit;
        }
    }
}
