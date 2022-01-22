using UnityEngine;
using TMPro;

public class MoneyCounterUI : MonoBehaviour
{

  [SerializeField]
  private TextMeshProUGUI moneyText;

  // Start is called before the first frame update
  void Awake()
  {
    moneyText = GetComponent<TextMeshProUGUI>();
  }

  // Update is called once per frame
  void Update()
  {
    moneyText.text = "Money: " + GameMaster.Money;

  }
}
