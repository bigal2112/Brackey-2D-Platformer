using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{

  [SerializeField]
  string hoverOverSound = "ButtonHover";
  [SerializeField]
  string buttonPressSound = "ButtonPress";

  private AudioManager audioManager;

  private void Start()
  {
    audioManager = AudioManager.audioManagerInstance;
    if (audioManager == null)
    {
      Debug.LogError("No audio manager found");
    }
  }

  public void PlayGame()
  {
    GameMaster.RemainingLives = 3;
    SceneManager.LoadScene(1);
  }

  public void ExitGame()
  {
    Application.Quit();
    Debug.Log("EXIT GAME");
  }

  public void OnMouseOver()
  {
    audioManager.PlaySound(hoverOverSound);
  }

  public void OnButtonPress()
  {
    audioManager.PlaySound(buttonPressSound);
  }
}
