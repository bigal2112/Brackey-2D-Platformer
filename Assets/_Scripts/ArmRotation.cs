using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour
{

  public int rotationOffset = 0;

  void Update()
  {
    //  find the difference between our character and the mouse (this will be a point exactly between the 2 objects)
    //  i.e. if player is at (1, 1) and mouse is at (3, 3) then this will gives us (2, 2).
    Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    //  normalize the vector so that x+y+z = 1 but all the ratios are kept the same (basically just makes the values smaller in equal proportions)
    difference.Normalize();

    //  find the angle between the X axis and a line from (0,0) to the (x,y) of the difference point (i.e (2, 2)) and convert to degrees.
    float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

    //  now apply the transformation to the arm
    transform.rotation = Quaternion.Euler(0f, 0f, rotationZ + rotationOffset);
  }
}
