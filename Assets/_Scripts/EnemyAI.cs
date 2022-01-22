using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]

public class EnemyAI : MonoBehaviour
{
  //  What we're chasing
  public Transform target;

  //    How many times a second we update our path
  public float updateRate = 2f;

  //  Caching
  private Seeker seeker;
  private Rigidbody2D rb;

  //  The calculated path
  public Path path;

  //  The AI's speed per second
  public float speed = 300f;
  public ForceMode2D fMode;

  [HideInInspector]
  public bool pathIsEnded = false;

  //  The max distance form the AI to a waypoint for it to continue to the next waypoint
  public float nextWaypointDistance = 3f;

  //    The waypoint we are currently moving towards
  private int currentWaypoint = 0;

  private bool searchingForPlayer = false;

  private void Start()
  {
    seeker = GetComponent<Seeker>();
    rb = GetComponent<Rigidbody2D>();

    if (target == null)
    {
      if (!searchingForPlayer)
      {
        searchingForPlayer = true;
        StartCoroutine(SearchForPlayer());
      }
      return;
    }

    //  start a new path to the target position and return the result to the OnPathComplete method.
    seeker.StartPath(transform.position, target.position, OnPathComplete);

    StartCoroutine(UpdatePath());

  }

  IEnumerator SearchForPlayer()
  {
    //  try and find the object with the Player tag
    GameObject searchResult = GameObject.FindGameObjectWithTag("Player");

    //  if we find the player
    if (searchResult != null)
    {
      //  update the target, set the searching flag to false and start the UpdatePath co-routine again as it will have been stopped by the yeild break.
      target = searchResult.transform;
      searchingForPlayer = false;
      StartCoroutine(UpdatePath());
      yield break;
    }
    else
    {
      //  otherwise wait for 0.5s and try again
      yield return new WaitForSeconds(0.5f);
      StartCoroutine(SearchForPlayer());
    }
  }


  IEnumerator UpdatePath()
  {
    if (target == null)
    {
      if (!searchingForPlayer)
      {
        searchingForPlayer = true;
        StartCoroutine(SearchForPlayer());
      }
      yield break;
    }

    //  start a new path to the target position and return the result to the OnPathComplete method.
    seeker.StartPath(transform.position, target.position, OnPathComplete);
    yield return new WaitForSeconds(1f / updateRate);
    StartCoroutine(UpdatePath());
  }

  public void OnPathComplete(Path p)
  {
    Debug.Log("We got a path. Did it have an error? " + p.error);
    if (!p.error)
    {
      path = p;
      currentWaypoint = 0;
    }
  }

  //  like update but is fixed and is great for doing anything to do with physics.
  private void FixedUpdate()
  {
    if (target == null)
    {
      if (!searchingForPlayer)
      {
        searchingForPlayer = true;
        StartCoroutine(SearchForPlayer());
      }
      return;
    }

    //  TODO: Always look at player - good for missiles etc.

    if (path == null)
      return;

    if (currentWaypoint >= path.vectorPath.Count)
    {
      if (pathIsEnded)
        return;

      // Debug.Log("End of path reached.");
      pathIsEnded = true;
      return;
    }

    pathIsEnded = false;

    //  Direction to the next waypoint
    Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
    dir *= speed * Time.fixedDeltaTime;

    //  we can now move the AI
    rb.AddForce(dir, fMode);

    float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
    if (dist < nextWaypointDistance)
    {
      currentWaypoint++;
      return;
    }

  }

}
