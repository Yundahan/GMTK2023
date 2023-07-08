using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image timerBar;
    public Image hpBar;

    private float timerBarAnchor;
    private float hpBarAnchor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        RectTransform canvas = GetComponent<RectTransform>();
        timerBarAnchor = canvas.rect.width / 2 - timerBar.rectTransform.rect.width / 2;
        hpBarAnchor = canvas.rect.width / 2 - hpBar.rectTransform.rect.width / 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTimerBar(float lastShotTime, float attackCD)
    {
        float timerFraction = (Time.time - lastShotTime) / attackCD;
        timerFraction = Mathf.Clamp01(timerFraction);

        timerBar.transform.localScale = new Vector3((1 - timerFraction), 1, 1);
        timerBar.transform.position = new Vector3(timerBarAnchor + (1 - timerFraction) * timerBar.rectTransform.rect.width / 2, timerBar.transform.position.y, 0);
    }

    public void SetHPBar(int hitPoints, int maxHP)
    {
        float hpFraction = ((float) maxHP - (float) hitPoints) / (float) maxHP;
        hpFraction = Mathf.Clamp01(hpFraction);

        hpBar.transform.localScale = new Vector3((1 - hpFraction), 1, 1);
        hpBar.transform.position = new Vector3(hpBarAnchor + (1 - hpFraction) * hpBar.rectTransform.rect.width / 2, hpBar.transform.position.y, 0);
    }
}
