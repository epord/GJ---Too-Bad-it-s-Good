using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = "Total time: " + Mathf.CeilToInt(PlayerPrefs.GetFloat("time")) + " seconds"
            + "\nTotal jumps: " + PlayerPrefs.GetInt("jumps")
            + "\nScore: " + PlayerPrefs.GetInt("score") + "/100";
    }
}
