using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusIndicator : MonoBehaviour
{
  [SerializeField]
  private RectTransform healthBarRect;
  [SerializeField]
  private TextMeshProUGUI healtText;

  private void Start()
  {
    if (healthBarRect == null)
      Debug.LogError("STATUS INDICATOR: No health bar object referenced!");

    if (healtText == null)
      Debug.LogError("STATUS INDICATOR: No health text object referenced!");
  }

  public void SetHealth(int _cur, int _max)
  {
    float _value = (float)_cur / _max;
    healthBarRect.localScale = new Vector3(_value, healthBarRect.localScale.y, healthBarRect.localScale.z);
    healtText.text = _cur + "/" + _max + " HP";

  }

}
