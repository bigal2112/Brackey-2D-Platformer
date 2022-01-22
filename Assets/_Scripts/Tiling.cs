using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  this ensures we always have a SpriteRendered attached to this GameObject so we don't get any errors.
[RequireComponent(typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour
{

  public int offsetX = 2;  //  offset so we don't ge any weird errors

  public bool hasRightBuddy = false;  //  used for checking if we need to instantiate a new buddy to the right
  public bool hasLeftBuddy = false;  //  used for checking if we need to instantiate a new buddy to the left
  public bool reverseScale = false;  //  used if the object is not tilable
  private float spriteWidth = 0f;  //  the width of the element

  private Camera cam;
  private Transform myTransform;  //  faster and better practice to do it this way.

  private void Awake()
  {
    cam = Camera.main;
    myTransform = transform;
  }


  void Start()
  {
    SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
    spriteWidth = sRenderer.sprite.bounds.size.x;                    //  gets the width of our element.
  }

  // Update is called once per frame
  void Update()
  {

    //  do we need to create buddies?
    if (hasLeftBuddy == false || hasRightBuddy == false)
    {
      //  calculate the cameras extend (half the width) of what the camera can see in world co-ordinates
      float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

      //  calculate the x positions where the camera can see the edge of the sprite (element)
      float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth) / 2 - camHorizontalExtend;
      float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth) / 2 + camHorizontalExtend;

      // Debug.Log("cam.transform.position.x: " + cam.transform.position.x + " edgeVisiblePositionRight:" + edgeVisiblePositionRight);
      //  if the position of the camera is past the right edge of this element (including the offset) and there's no buddy already on that side
      if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasRightBuddy == false)
      {
        MakeNewBuddy(1);
        hasRightBuddy = true;
      }
      //  else if the position of the camera is past the left edge of this element (including the offset) and there's no buddy already on that side
      else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasLeftBuddy == false)
      {
        MakeNewBuddy(-1);
        hasLeftBuddy = true;
      }

    }
  }

  //  a function that creates a buddy on the required side
  //  rightOrLeft: -1 = create buddy on the left, +1 = create buddy on the right
  void MakeNewBuddy(int rightOrLeft)
  {

    //  calculating the new position for our new buddy
    // Debug.Log("New x: " + myTransform.position.x + spriteWidth * rightOrLeft);
    Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);

    //  instantiate a new object and cast it to a Transform variable
    Transform newBuddy = (Transform)Instantiate(myTransform, newPosition, myTransform.rotation, myTransform.parent);

    //  if the element is not tilable (the right and left hand side don't match up nicely so tiling would look terrible) then use this trick which
    //  simply multiplies the x scale by -1 which flips it horizontally.
    if (reverseScale == true)
      newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);

    if (rightOrLeft > 0)
      newBuddy.GetComponent<Tiling>().hasRightBuddy = true;
    else
      newBuddy.GetComponent<Tiling>().hasLeftBuddy = true;
  }
}
