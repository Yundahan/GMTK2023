using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image hpBar;
    public Image pauseImage;
    public TextMeshProUGUI killcount;

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

    public void ShowPauseImage()
    {
        pauseImage.gameObject.SetActive(true);
    }

    public void HidePauseImage()
    {
        pauseImage.gameObject.SetActive(false);
    }

    public void SetKillCount(int value)
    {
        killcount.text = value.ToString();
    }
}
