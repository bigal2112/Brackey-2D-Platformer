using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBulletTrail : MonoBehaviour
{
  public int moveSpeed = 230;

  void Update()
  {
    //    if the object is simply going in a straight line and does not react to gravity you can dispense with a RigidBody and use the following to move it.
    //    It's a lot less power hungry than using a RigidBody.
    transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    Destroy(gameObject, (0.5f));        //  destory the trail after 0.5 seconds so we don't clutter up the hierarchy with clones.
  }

}
