using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

  public enum SpawnState { SPAWNING, WAITING, COUNTING };

  [System.Serializable]           //  allows us to change the value is instances of this class in the Inspector
  public class Wave
  {
    public string name;
    public Transform enemy;
    public int count;
    public float rate;
  }

  public Wave[] waves;
  private int nextWave = 0;
  public int NextWave { get { return nextWave; } }

  public Transform[] spawnPoints;

  public float timeBetweenWaves = 5f;

  private float waveCountdown;
  public float WaveCountdown { get { return waveCountdown; } }

  private float searchCountdown = 1f;


  private SpawnState state = SpawnState.COUNTING;
  public SpawnState State { get { return state; } }

  private void Start()
  {
    waveCountdown = timeBetweenWaves;

    if (spawnPoints.Length == 0)
      Debug.LogError("No spawn points referenced");

    if (waves.Length == 0)
      Debug.LogError("No waves referenced");
  }

  private void Update()
  {

    if (state == SpawnState.WAITING)
    {
      //    If there are no enemies left then start a new wave countdown otherwise return without going any further
      if (!EnemyIsAlive())
      {

        //    Begin a new wave countdown
        WaveCompleted();
      }
      else
      {
        return;
      }
    }

    if (waveCountdown <= 0)
    {
      if (state != SpawnState.SPAWNING)
      {
        StartCoroutine(SpawnWave(waves[nextWave]));
      }
    }
    else
    {
      waveCountdown -= Time.deltaTime;
    }
  }

  //  checks whether there are any game objects with a TAG of "Enemy"
  //    note, this methid is quite taxing on the system so we have put in on a fixed timer.
  bool EnemyIsAlive()
  {
    searchCountdown -= Time.deltaTime;
    if (searchCountdown <= 0)

      //  if no "Enemy" object found then return false otherwise restart countdown;
      if (GameObject.FindGameObjectWithTag("Enemy") == null)
        return false;
      else
        searchCountdown = 1f;

    return true;
  }

  IEnumerator SpawnWave(Wave _wave)
  {
    Debug.Log("Spawing wave: " + _wave.name);

    state = SpawnState.SPAWNING;

    for (int i = 0; i < _wave.count; i++)
    {
      SpawnEnemy(_wave.enemy);
      yield return new WaitForSeconds(1f / _wave.rate);
    }

    state = SpawnState.WAITING;

    yield break;
  }

  void SpawnEnemy(Transform _enemy)
  {
    Debug.Log("Spawning enemy: " + _enemy.name);


    Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
    Instantiate(_enemy, _sp.position, _sp.rotation);
  }


  void WaveCompleted()
  {
    Debug.Log("Wave completed");
    state = SpawnState.COUNTING;
    waveCountdown = timeBetweenWaves;

    nextWave++;

    if (nextWave >= waves.Length)
    {
      nextWave = 0;
      Debug.Log("COMPLETED ALL WAVES....looping");
    }

  }

}
