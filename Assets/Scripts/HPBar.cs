using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    const float TIME = 0.1f;
    float maxLength;
    Vector3 defaultScale;

    Transform currentHPBar;
    Transform currentShieldPlusHPBar;

    bool updating = false;
    float remainTime;
    float speed;
    float shieldSpeed;
    int maxHP;
    float currentDisplayedHP;
    float currentDisplayedShield;
    int targetHP;
    int targetShield;

    public void UpdateHP(int hp)
    {
        updating = true;
        remainTime = TIME;
        targetHP = hp;
        speed = (currentDisplayedHP - hp) / TIME;
    }
    public void UpdateShield(int shield)
    {
        updating = true;
        remainTime = TIME;
        targetShield = shield;
        shieldSpeed = (currentDisplayedShield - shield) / TIME;
    }

    public void Init(int maxHP, int currentHP)
    {
        currentHPBar = transform.Find("CurrentHPBar");
        currentShieldPlusHPBar = transform.Find("CurrentHPPlusShield");
        defaultScale = currentHPBar.localScale;
        this.maxHP = maxHP;
        this.currentDisplayedHP = currentHP;
        this.currentDisplayedShield = 0;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (remainTime > 0)
        {
            remainTime -= Time.deltaTime;
            currentDisplayedHP -= speed * Time.deltaTime;
            currentDisplayedShield -= shieldSpeed * Time.deltaTime;
            UpdateDisplayedBarLength();
        }
        else
        {
            if (updating)
            {
                updating = false;
                remainTime = 0;
                currentDisplayedHP = targetHP;
                currentDisplayedShield = targetShield;
                UpdateDisplayedBarLength();
            }
        }
    }

    void UpdateDisplayedBarLength()
    {
        float currentScale = defaultScale.x * (currentDisplayedHP / (float)maxHP);
        currentHPBar.localScale = new Vector3(currentScale, defaultScale.y, defaultScale.z);
        Vector3 curHPBarPos = currentHPBar.localPosition;
        currentHPBar.localPosition = new Vector3((currentScale - defaultScale.x) / 2, curHPBarPos.y, curHPBarPos.z);

        float currentHPPlusShieldScale = defaultScale.x * ((currentDisplayedHP + currentDisplayedShield) / (float)maxHP);
        Debug.Log(currentHPPlusShieldScale);
        currentShieldPlusHPBar.localScale = new Vector3(currentHPPlusShieldScale, defaultScale.y, defaultScale.z);
        Vector3 curShieldBarPos = currentShieldPlusHPBar.localPosition;
        currentShieldPlusHPBar.localPosition = new Vector3((currentHPPlusShieldScale - defaultScale.x) / 2, curShieldBarPos.y, curShieldBarPos.z);
    }
}
