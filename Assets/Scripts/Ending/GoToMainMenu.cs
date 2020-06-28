using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToMainMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown)
        {
            Debug.Log("GO TO MAIN MENU");
        }
    }
}
