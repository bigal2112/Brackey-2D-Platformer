using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{

  public string gameOverSound = "GameOverSound";

  private AudioManager audioManager;

  private void Start()
  {
    audioManager = AudioManager.audioManagerInstance;
    if (audioManager == null)
    {
      Debug.LogError("No audio manager found");
    }
    else
    {
      StartCoroutine(PlayGameOverSound());
    }
  }

  //  I done the game over sound like this so I could put a slight delay in before the sound it played
  //  as it was coming in too quickly when played without a delay.
  IEnumerator PlayGameOverSound()
  {
    yield return new WaitForSeconds(0.8f);
    audioManager.PlaySound(gameOverSound);
  }
}
