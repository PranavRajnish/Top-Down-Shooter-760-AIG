using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyDebugState : MonoBehaviour
{
    // Start is called before the first frame update

    public EnemyStateManager stateManager;
    public TextMeshProUGUI debugText;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(stateManager.debugStateOn)
        {
            gameObject.SetActive(true);
            debugText.text = stateManager.GetCurrentState();
        }
        else
        {
            gameObject.SetActive(false);
        }
        
    }
}
