using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressKeyToQuit : MonoBehaviour
{
    public KeyCode keyToLookFor = KeyCode.Escape;

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToLookFor))
        {
#if UNITY_STANDALONE
            Application.Quit();
#endif
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
