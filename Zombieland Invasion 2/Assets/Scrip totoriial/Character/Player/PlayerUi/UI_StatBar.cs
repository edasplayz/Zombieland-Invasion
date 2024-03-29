using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatBar : MonoBehaviour
{
    private Slider slider;
    // variable to scale bar size depending on stats (higher stat = longer bar acros screen)
    // secondary bar behind may bar for polish effect (yellow bar that shows how mutcxh an action/damage takes away from current stat)
    
    protected virtual void Awake()
    {
        if(slider == null)
        {
            slider = GetComponent<Slider>();
        }
    }

    public virtual void SetStat(int newValue)
    {
        slider.value = newValue;
    }

    public virtual void SetMaxStat(int maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = maxValue;
    }
}
