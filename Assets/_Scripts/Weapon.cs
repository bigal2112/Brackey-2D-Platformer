using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

  public float fireRate = 0;
  public int damage = 10;
  public LayerMask whatToHit;
  public Transform bulletTrailPrefab;
  public Transform muzzleFlashPrefab;
  public Transform hitPrefab;

  //  handle camera shake
  public float camShakeAmount = 0.1f;
  public float camShakeLength = 0.1f;
  CameraShake camShake;

  public string weaponShootSound = "DefaultShot";

  //  Caching
  private AudioManager audioManager;

  float timeToFire = 0;                     //  time between bursts of fire for a multiple fire weapon
  float timeToSpawnEffect = 0;
  public float effectSpawnRate = 10;        //  how often we run the Effect. This is a simple way to pool objects and 

  Transform firePoint;

  private void Awake()
  {
    //    use this method of getting the transform of the object as it's quicker
    firePoint = transform.Find("FirePoint");
    if (firePoint == null)
      Debug.LogError("No FirePoint component in the Player");
  }

  private void Start()
  {
    camShake = GameMaster.gm.GetComponent<CameraShake>();
    if (camShake == null)
      Debug.LogError("No Camera Shake component found on GameMaster object");

    audioManager = AudioManager.audioManagerInstance;
    if (audioManager == null)
    {
      Debug.LogError("No audio manager found");
    }
  }


  void Update()
  {
    //  if a single fire weapon then and the fire button is pressed then shoot
    if (fireRate == 0)
    {

      if (Input.GetButtonDown("Fire1"))
      {
        Shoot();
      }
    }
    else
    {
      //  if it's a multiple fire weapon and the fire button is being HELD DOWN and the time is right for the next (or first) shot then shoot
      if (Input.GetButton("Fire1") && Time.time > timeToFire)
      {
        //    update the timeToFire so we wait until it's time to fire before firing the next bullet.
        timeToFire = Time.time + 1 / fireRate;
        Shoot();
      }
    }
  }

  void Shoot()
  {
    //    create a new Vector2 for the mouse co-ordinates and convert them to the position in the world. You need to do this as it's very difficult to simply //     use the x,y co-ordinates to do the following.
    Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);

    //  create a raycast. The Raycast uses origin (firePointPosition) and direction (mousePosition - firePointPosition)
    RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);



    Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);
    if (hit.collider != null)
    {
      Debug.DrawLine(firePointPosition, hit.point, Color.red);
      Enemy enemy = hit.collider.GetComponent<Enemy>();
      if (enemy != null)
      {
        enemy.DamageEnemy(damage);
        // Debug.Log("Enemy hit. " + damage + " damage. " + enemy. + " left.");

      }
    }

    if (Time.time >= timeToSpawnEffect)
    {
      //  see if we've hit something.
      Vector3 hitPos;
      Vector3 hitNormal;

      if (hit.collider == null)
      {
        //  no hit then make the end point somewhere far away but in the correct direction.
        hitPos = (mousePosition - firePointPosition) * 30;
        hitNormal = new Vector3(9999, 9999, 9999);
      }
      else
      {
        //  if we hit something then get the hit point.
        hitPos = hit.point;
        hitNormal = hit.normal;
      }

      //  call the effect method
      Effect(hitPos, hitNormal);

      //  and set the next effect spawn time.
      timeToSpawnEffect = Time.time + 1 / effectSpawnRate;

    }

  }

  void Effect(Vector3 hitPos, Vector3 hitNormal)
  {
    //  get a reference to the bullet trail
    Transform trail = (Transform)Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation);
    LineRenderer lr = trail.GetComponent<LineRenderer>();
    if (lr != null)
    {
      //  set start and end positions. These are the element number of the Positions array (look in the BulletTrail prefab object)
      lr.SetPosition(0, firePoint.position);
      lr.SetPosition(1, hitPos);
    }
    Destroy(trail.gameObject, 0.04f);                                  //  and destroy it after 0.04 of a second.

    if (hitNormal != new Vector3(9999, 9999, 9999))
    {
      //  shake the camera
      camShake.ShakeCamera(camShakeAmount, camShakeLength);

      //  show the hit effect
      Transform impact = (Transform)Instantiate(hitPrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal));

      //  get rid of the effect
      Destroy(impact.gameObject, 1.1f);
    }

    //  create an instance of the muzzle flash and store it
    Transform clone = (Transform)Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
    clone.parent = firePoint.parent;                        //  make sure the parent is the same as the firePoint
    float size = Random.Range(0.6f, 0.9f);                  //  make the muzzle flash a random size
    clone.localScale = new Vector3(size, size, size);

    //  play shoot sound
    audioManager.PlaySound(weaponShootSound);

    //  and destroy the muzzle flash after 0.02 of a second.
    Destroy(clone.gameObject, 0.02f);
  }
}
