using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLvl : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public GameObject transition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transition.SetActive(true);
            Invoke("ChangeLevel", 1);
        }
    }

    private void ChangeLevel()
    {
        SceneManager.LoadScene(sceneName);
    }
}
