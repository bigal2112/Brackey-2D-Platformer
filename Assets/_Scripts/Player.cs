using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour
{

  //  create a class for the player statistics
  [System.Serializable]
  public class PlayerStats
  {
    public int maxHealth = 100;

    private int _currentHealth;
    public int currentHealth
    {
      get { return _currentHealth; }
      set { _currentHealth = Mathf.Clamp(value, 0, maxHealth); }
    }

    public void Init()
    {
      currentHealth = maxHealth;
    }

  }

  //  create a new instance of the PlayerStats class that we can use in this script.
  public PlayerStats playerStats = new PlayerStats();
  public int fallBoundary = -20;

  [SerializeField]
  private StatusIndicator statusIndicator;

  public string deathSoundName = "PlayerDeathSound";
  public string damageSoundName = "PlayerHitSound";

  private AudioManager audioManager;

  private void Start()
  {
    //  initialise the player ststs
    playerStats.Init();

    //  update the status bar
    if (statusIndicator != null)
      statusIndicator.SetHealth(playerStats.currentHealth, playerStats.maxHealth);
    else
      Debug.LogError("No status indicator attached to the player");

    audioManager = AudioManager.audioManagerInstance;
    if (audioManager == null)
    {
      Debug.LogError("No audio manager found");
    }

    //  add the OnUpgradeMenuToggle method to the UpgradeMenu delegate
    GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

  }

  private void Update()
  {
    //  if the y position of the player is below the fallBoundry then we've died so inflict maximum damage to us!!
    if (transform.position.y <= fallBoundary)
      DamagePlayer(100000000);
  }

  void OnUpgradeMenuToggle(bool active)
  {
    //  handle what happens when the update menu is toggled.
    GetComponent<Platformer2DUserControl>().enabled = !active;      //  enable/disable the player controls
    Weapon _weapon = GetComponentInChildren<Weapon>();
    if (_weapon != null)
      _weapon.enabled = !active;                                    //  enable/disable the weapon (if found)
  }

  //    damage player method
  public void DamagePlayer(int damageAmount)
  {

    audioManager.PlaySound(damageSoundName);

    //  inflict damage
    playerStats.currentHealth -= damageAmount;

    //  update the status bar
    if (statusIndicator != null)
      statusIndicator.SetHealth(playerStats.currentHealth, playerStats.maxHealth);
    else
      Debug.LogError("No status indicator attached to the player");

    //  if there's no health left then kill player
    if (playerStats.currentHealth <= 0)
    {
      audioManager.PlaySound(deathSoundName);
      GameMaster.KillPlayer(this);
    }
  }

  private void OnDestroy()
  {
    //  when this enemy get killed then unsubscribe the OnUpgradeMenuToggle method from the UpgradeMenu delegate
    GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
  }

}
