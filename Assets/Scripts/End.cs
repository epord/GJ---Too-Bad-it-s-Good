using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private float totalTime = 0f;
    private int jumpsCount = 0;

    IEnumerator EndLevel()
    {
        int timeScore = 100 - (int)(Mathf.Max(0, (totalTime - 30f)) / 600f * 100f);
        int jumpScore = 100 - (int)(Mathf.Max(0, (jumpsCount - 80f)) / 2000f * 100f);
        int score = Mathf.Min(100, Mathf.Max(0, (timeScore + jumpScore) / 2));

        yield return new WaitForSeconds(2);
        PlayerPrefs.SetFloat("time", totalTime);
        PlayerPrefs.SetInt("jumps", jumpsCount);
        PlayerPrefs.SetInt("score", score);
        SceneManager.LoadScene("End");
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Player")
        {
            // End level
            StartCoroutine(EndLevel());
        }
    }

    private void Update()
    {
        totalTime += Time.deltaTime;

        if (
            Input.GetButtonDown("bottomLeftJump")
            || Input.GetButtonDown("topLeftJump")
            || Input.GetButtonDown("bottomRightJump")
            || Input.GetButtonDown("topRightJump")
        ) jumpsCount++;
    }
}
