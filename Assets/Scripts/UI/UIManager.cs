using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image hpBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHPBar(int hitPoints, int maxHP)
    {
        float hpFraction = (float) hitPoints / (float) maxHP;
        hpFraction = Mathf.Clamp01(hpFraction);
        hpBar.fillAmount = hpFraction;
    }
}
