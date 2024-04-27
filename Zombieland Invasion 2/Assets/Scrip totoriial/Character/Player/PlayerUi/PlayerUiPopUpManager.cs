using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUiPopUpManager : MonoBehaviour
{
    [Header("Message Pop Up")]
    [SerializeField] TextMeshProUGUI popUpMessageText;
    [SerializeField] GameObject popUpMessageGameObject;

    // if you plan on making all of those popups function identically you could just make 1 pop up gameobject and change the text values as needed
    // instead of making several different groups for pop up functionality
    [Header("you died Pop Up")]
    [SerializeField] GameObject youDiedPopUpGameObject;
    [SerializeField] TextMeshProUGUI youDiedPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI youDiedPopUpText;
    [SerializeField] CanvasGroup youDiedPopUpCanvasGroup; // allows us to set alpha fade over time 

    [Header("Boss Defeated Pop Up")]
    [SerializeField] GameObject bossDefeatedPopUpGameObject;
    [SerializeField] TextMeshProUGUI bossDefeatedPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI bossDefeatedPopUpText;
    [SerializeField] CanvasGroup bossDefeatedPopUpCanvasGroup; // allows us to set alpha fade over time 

    [Header("Site Of Grace Pop Up")]
    [SerializeField] GameObject graceRestoredPopUpGameObject;
    [SerializeField] TextMeshProUGUI graceRestoredPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI graceRestoredPopUpText;
    [SerializeField] CanvasGroup graceRestoredPopUpCanvasGroup; // allows us to set alpha fade over time 

    public void CloseAllPopUpWindows()
    {
        popUpMessageGameObject.SetActive(false);

        PlayerUiManager.instance.popUpWindowIsOpen = false;
    }

    public void SendPlayerMessagePopUp(string messageText)
    {
        PlayerUiManager.instance.popUpWindowIsOpen = true;
        popUpMessageText.text = messageText;
        popUpMessageGameObject.SetActive(true);
    }
    public void SendYouDiedPopUp()
    {
        // activate post processing effects

        youDiedPopUpGameObject.SetActive(true);
        youDiedPopUpBackgroundText.characterSpacing = 0;
        // strech out the pop up
        StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgroundText, 8, 19));
        // fade in the pop up
        StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 5));
        // wait then fade out the pop up
        StartCoroutine(WaitThenFadeOutPopUpOverTime(youDiedPopUpCanvasGroup, 2, 5));
    }

    public void SendBossDefeatedPopUp(string bossDefeatedMessage)
    {
        // activate post processing effects
        bossDefeatedPopUpText.text = bossDefeatedMessage;
        bossDefeatedPopUpBackgroundText.text = bossDefeatedMessage;
        bossDefeatedPopUpGameObject.SetActive(true);
        bossDefeatedPopUpBackgroundText.characterSpacing = 0;
        // strech out the pop up
        StartCoroutine(StretchPopUpTextOverTime(bossDefeatedPopUpBackgroundText, 8, 19));
        // fade in the pop up
        StartCoroutine(FadeInPopUpOverTime(bossDefeatedPopUpCanvasGroup, 5));
        // wait then fade out the pop up
        StartCoroutine(WaitThenFadeOutPopUpOverTime(bossDefeatedPopUpCanvasGroup, 2, 5));
    }

    public void SendGraceRestoredPopUp(string graceRestoredMessage)
    {
        // activate post processing effects
        graceRestoredPopUpText.text = graceRestoredMessage;
        graceRestoredPopUpBackgroundText.text = graceRestoredMessage;
        graceRestoredPopUpGameObject.SetActive(true);
        graceRestoredPopUpBackgroundText.characterSpacing = 0;
        // strech out the pop up
        StartCoroutine(StretchPopUpTextOverTime(graceRestoredPopUpBackgroundText, 8, 19));
        // fade in the pop up
        StartCoroutine(FadeInPopUpOverTime(graceRestoredPopUpCanvasGroup, 5));
        // wait then fade out the pop up
        StartCoroutine(WaitThenFadeOutPopUpOverTime(graceRestoredPopUpCanvasGroup, 2, 5));
    }

    private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount) 
    { 
        if(duration > 0f)
        {
            text.characterSpacing = 0; // resets our character spacing
            float timer = 0;
            yield return null;

            while(timer < duration)
            {
                timer = timer + Time.deltaTime;
                text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                yield return null;
            }
        }
    }

    private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration) 
    {
        if(duration > 0)
        {
            canvas.alpha = 0;

            float timer = 0;
            yield return null;

            while (timer < duration)
            {
                timer = timer + Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);
                yield return null;
            }
        }

        canvas.alpha = 1;

        yield return null;
    }

    private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvas, float duration, float delay)
    {
        if (duration > 0)
        {

            while(delay > 0)
            {
                delay = delay - Time.deltaTime;
                yield return null;
            }

            canvas.alpha = 1;

            float timer = 0;
            yield return null;

            while (timer < duration)
            {
                timer = timer + Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                yield return null;
            }
        }

        canvas.alpha = 0;

        yield return null;
    }
}
