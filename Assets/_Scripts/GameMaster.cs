using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

  public static GameMaster gm;

  //  internal remaining lives parameter and externally accessible getter
  private static int _remainingLives = 3;
  public static int RemainingLives
  {
    set { _remainingLives = value; }
    get { return _remainingLives; }
  }

  [SerializeField]
  private int startingMoney;
  public static int Money;

  public Transform playerPrefab;
  public Transform particlePrefab;
  public Transform spawnPoint;
  public float spawnDelay = 3.5f;
  public string spawnSoundName;

  public CameraShake cameraShake;

  [SerializeField]
  private GameObject gameOverUI;

  [SerializeField]
  private GameObject updgradeMenuUI;

  public delegate void UpgradeMenuCallback(bool active);
  public UpgradeMenuCallback onToggleUpgradeMenu;

  //  cache
  private AudioManager audioManager;

  private void Awake()
  {
    if (gm == null)
      gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
  }

  private void Start()
  {
    if (cameraShake == null)
      Debug.LogError("No camera shake referenced in GameMaster");

    //  caching
    audioManager = AudioManager.audioManagerInstance;
    if (audioManager == null)
      Debug.LogError("No audio manager found in this scene");

    Money = startingMoney;
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.U))
    {
      ToggleUpgradeMenu();
    }
  }

  private void ToggleUpgradeMenu()
  {
    updgradeMenuUI.SetActive(!updgradeMenuUI.activeSelf);
    onToggleUpgradeMenu.Invoke(updgradeMenuUI.activeSelf);
  }

  IEnumerator _RespawnPlayer()
  {


    yield return new WaitForSeconds(spawnDelay);

    audioManager.PlaySound(spawnSoundName);
    Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    Transform clone = Instantiate(particlePrefab, spawnPoint.position, spawnPoint.rotation);
    Destroy(clone.gameObject, 3);

  }

  public static void KillPlayer(Player player)
  {
    Destroy(player.gameObject);
    _remainingLives--;
    if (_remainingLives <= 0)
      gm.EndGame();
    else
      gm.StartCoroutine(gm._RespawnPlayer());
  }

  //  this is a static instance that can be called from outside of this class
  public static void KillEnemy(Enemy enemy)
  {
    gm._KillEnemy(enemy);
  }

  //  internal instance that can make sure of all the variable and transfors that are within the class.
  public void _KillEnemy(Enemy _enemy)
  {
    //  add particles
    Transform clone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity);

    //  shake the camers
    cameraShake.ShakeCamera(_enemy.deathShakeAmount, _enemy.deathShakeLength);

    //  play explosion sound
    audioManager.PlaySound(_enemy.deathSound);

    //  clean up
    Destroy(_enemy.gameObject);
    Destroy(clone.gameObject, 5);
  }

  public void EndGame()
  {
    gameOverUI.SetActive(true);
  }

}
