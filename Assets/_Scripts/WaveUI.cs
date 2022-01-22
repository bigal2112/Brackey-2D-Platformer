using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveUI : MonoBehaviour
{
  [SerializeField]
  WaveSpawner spawner;

  [SerializeField]
  Animator waveAnimator;

  [SerializeField]
  TextMeshProUGUI waveCountdownText;

  [SerializeField]
  TextMeshProUGUI waveCountText;

  private WaveSpawner.SpawnState previousState;



  // Start is called before the first frame update
  void Start()
  {
    if (spawner == null)
    {
      Debug.LogError("No WaveSpawer referenced");
      this.enabled = false;
    }
    if (waveAnimator == null)
    {
      Debug.LogError("No Animator referenced");
      this.enabled = false;
    }
    if (waveCountdownText == null)
    {
      Debug.LogError("No waveCountdownText referenced");
      this.enabled = false;
    }
    if (waveCountText == null)
    {
      Debug.LogError("No waveCountText referenced");
      this.enabled = false;
    }
  }


  // Update is called once per frame
  void Update()
  {
    switch (spawner.State)
    {
      case WaveSpawner.SpawnState.COUNTING:
        UpdateCountingUI();
        break;
      case WaveSpawner.SpawnState.SPAWNING:
        UpdateSpawningUI();
        break;
    }

    previousState = spawner.State;
  }

  void UpdateCountingUI()
  {
    if (previousState != WaveSpawner.SpawnState.COUNTING)
    {
      waveAnimator.SetBool("WaveIncoming", false);
      waveAnimator.SetBool("WaveCountdown", true);
    }

    waveCountdownText.text = ((int)spawner.WaveCountdown).ToString();

  }

  void UpdateSpawningUI()
  {
    if (previousState != WaveSpawner.SpawnState.SPAWNING)
    {
      waveAnimator.SetBool("WaveCountdown", false);
      waveAnimator.SetBool("WaveIncoming", true);

      waveCountText.text = (spawner.NextWave + 1).ToString();
    }

  }
}
