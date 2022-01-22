using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour
{
  //  create a class for the player statistics
  [System.Serializable]
  public class EnemyStats
  {
    public int maxHealth = 100;

    private int _currentHealth;
    public int currentHealth
    {
      get { return _currentHealth; }
      set { _currentHealth = Mathf.Clamp(value, 0, maxHealth); }
    }

    public int damage = 40;

    public void Init()
    {
      currentHealth = maxHealth;
    }

  }

  //  create a new instance of the EnemyStats class that we can use in this script.
  public EnemyStats stats = new EnemyStats();

  public Transform deathParticles;
  public float deathShakeAmount = 0.1f;
  public float deathShakeLength = 0.1f;
  public string deathSound = "Explosion";

  //  create a status indicator instance but this is optional as not all enemies may have a status indicator
  [Header("Optional: ")]      //  show in Inspector that this is optional.
  [SerializeField]
  private StatusIndicator statusIndicator;


  private void Start()
  {
    stats.Init();
    if (statusIndicator != null)
      statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);

    if (deathParticles == null)
      Debug.LogError(("No enemy death particles referenced!!"));

    //  add the OnUpgradeMenuToggle method to the UpgradeMenu delegate
    GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

  }

  void OnUpgradeMenuToggle(bool active)
  {
    //  handle what happens when the update menu is toggled.
    GetComponent<EnemyAI>().enabled = !active;      //  enable/disable the enemy AI
  }


  //    damage player method
  public void DamageEnemy(int damageAmount)
  {

    //  take the damage amount away from the enemies health value
    stats.currentHealth -= damageAmount;

    //  if the enemies helth is 0 we've killed it
    if (stats.currentHealth <= 0)
    {
      GameMaster.KillEnemy(this);
    }

    //  otherwise, if the enemy has a health status, update it.
    else
    {
      if (statusIndicator != null)
        statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
    }


  }

  private void OnCollisionEnter2D(Collision2D _collision)
  {

    //  try and get the Player component of what we just collided into
    Player _player = _collision.collider.GetComponent<Player>();

    //  if it was a player then inflict damage and blow up
    if (_player != null)
    {
      _player.DamagePlayer(stats.damage);
      DamageEnemy(1000000);
    }
  }

  private void OnDestroy()
  {
    //  when this enemy get killed then unsubscribe the OnUpgradeMenuToggle method from the UpgradeMenu delegate
    GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
  }

}
