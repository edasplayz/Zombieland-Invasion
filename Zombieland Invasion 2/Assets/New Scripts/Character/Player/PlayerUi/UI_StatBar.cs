using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatBar : MonoBehaviour
{
    protected Slider slider;
    protected RectTransform rectTransform;

    // variable to scale bar size depending on stats (higher stat = longer bar acros screen)
    [SerializeField] protected bool scaleBarLenghtWithStats = true;
    [SerializeField] protected float widthScaleMultiplier = 1;
    // secondary bar behind may bar for polish effect (yellow bar that shows how mutcxh an action/damage takes away from current stat)
    
    protected virtual void Awake()
    {
        if(slider == null)
        {
            slider = GetComponent<Slider>();
            rectTransform = GetComponent<RectTransform>();
        }
    }

    protected virtual void Start()
    {

    }

    public virtual void SetStat(int newValue)
    {
        slider.value = newValue;
    }

    public virtual void SetMaxStat(int maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = maxValue;

        if (scaleBarLenghtWithStats)
        {
            // scale the transform of this object
            rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);

            // resets the position of the bars based on their layout groups settings
            PlayerUiManager.instance.playerUIHudManager.RefreshHUD();
        }
    }
}
