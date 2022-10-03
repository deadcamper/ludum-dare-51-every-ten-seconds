using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnyKeyUI : MonoBehaviour
{
    public string nextSceneName;
    public TMP_Text anyKeyText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Flicker());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && Time.time > 3)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
            Destroy(this); // kills the coroutine
        }
    }

    IEnumerator Flicker()
    {
        anyKeyText.alpha = 0;

        if (Time.time < 3)
        {
            yield return new WaitForSeconds(3);
        }

        while(true)
        {
            for (float a = 0; a < 1; a += 0.01f)
            {
                anyKeyText.alpha = a;
                yield return new WaitForFixedUpdate();
            }
            for (float a = 1; a > 0; a -= 0.01f)
            {
                anyKeyText.alpha = a;
                yield return new WaitForFixedUpdate();
            }
        }

        
    }
}
