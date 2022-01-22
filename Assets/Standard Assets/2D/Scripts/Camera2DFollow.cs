using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
  public class Camera2DFollow : MonoBehaviour
  {
    public Transform target;
    public float damping = 1;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;
    public float yPosRestriction = -1;

    private float m_OffsetZ;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    private Vector3 m_LookAheadPos;

    float nextTimeToSearchForPlayer = 0;

    // Use this for initialization
    private void Start()
    {
      m_LastTargetPosition = target.position;
      m_OffsetZ = (transform.position - target.position).z;
      transform.parent = null;
    }


    // Update is called once per frame
    private void Update()
    {

      //  if target is null it means our player die and will be respawned. Once respawned the camera will not be attached to it so
      //  we need to try and find it so we can re-attached the camera to it.
      if (target == null)
      {
        FindPlayer();
        return;
      }


      // only update lookahead pos if accelerating or changed direction
      float xMoveDelta = (target.position - m_LastTargetPosition).x;

      bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

      if (updateLookAheadTarget)
      {
        m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
      }
      else
      {
        m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
      }

      Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward * m_OffsetZ;
      Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

      newPos = new Vector3(newPos.x, Mathf.Clamp(newPos.y, yPosRestriction, Mathf.Infinity), newPos.z);

      transform.position = newPos;

      m_LastTargetPosition = target.position;
    }


    void FindPlayer()
    {
      //  if we're ready to look for the player
      if (nextTimeToSearchForPlayer <= Time.time)
      {
        //  try and find the object with the Player tag
        GameObject searchResult = GameObject.FindGameObjectWithTag("Player");

        //  if we find it then set the target variable to it's transform
        if (searchResult != null)
          target = searchResult.transform;

        //  we only want to search for the player a couple of times a second as it's very CPU intensive.
        nextTimeToSearchForPlayer = Time.time + 0.5f;
      }
    }
  }

}
