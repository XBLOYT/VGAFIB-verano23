using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    public PlayerMovement player;

    public void Continue()
    {
        player.Pause();
    }

    public void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}
