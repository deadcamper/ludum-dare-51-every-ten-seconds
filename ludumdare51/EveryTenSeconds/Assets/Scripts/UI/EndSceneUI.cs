using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndSceneUI : MonoBehaviour
{

    public TMP_Text everyTenSecondsText;
    public TMP_Text everyTenSeconds2Text;

    public TMP_Text escKeyText;


    private void Start()
    {
        everyTenSecondsText.alpha = 0;
        everyTenSeconds2Text.alpha = 0;
        escKeyText.alpha = 0;
        StartCoroutine(UIFlow());
    }

    IEnumerator UIFlow()
    {
        everyTenSecondsText.alpha = 0;
        everyTenSeconds2Text.alpha = 0;
        escKeyText.alpha = 0;

        yield return new WaitForSeconds(1);

        for (float a = 0; a < 1; a += 0.02f)
        {
            everyTenSecondsText.alpha = a;
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.25f);

        for (float a = 0; a < 1; a += 0.02f)
        {
            everyTenSeconds2Text.alpha = a;
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.5f);

        for (float a = 0; a < 1; a += 0.01f)
        {
            escKeyText.alpha = a;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForEndOfFrame();
    }

}
