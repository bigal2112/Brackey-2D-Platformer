using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LivesCounterUI : MonoBehaviour
{

  [SerializeField]
  private TextMeshProUGUI livesText;

  // Start is called before the first frame update
  void Awake()
  {
    livesText = GetComponent<TextMeshProUGUI>();
  }

  // Update is called once per frame
  void Update()
  {
    livesText.text = "Lives: " + GameMaster.RemainingLives;

  }
}
