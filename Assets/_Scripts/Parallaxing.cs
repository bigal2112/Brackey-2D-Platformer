using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
  public Transform[] backgrounds;       //  Array of all the back and foregrounds to be parallaxed
  private float[] parallaxScales;        //  Proportion of the cameras movement to move the backgrounds by
  public float smoothing;                //  How smooth the parallaxing is. Make sure to set this above 0.

  private Transform cam;                 //  Reference to the main camera transform
  private Vector3 previousCamPostition;  //  Stores the camera position in the previous frame.

  private void Awake()
  {
    //  set up the camera reference
    cam = Camera.main.transform;
  }

  void Start()
  {

    //  the previous frame had the current frames camera position
    previousCamPostition = cam.position;

    //  assigning corresponding parallax scales
    parallaxScales = new float[backgrounds.Length];
    for (int i = 0; i < backgrounds.Length; i++)
    {
      parallaxScales[i] = backgrounds[i].position.z * -1;
    }

  }


  void Update()
  {

    // for each background
    for (int i = 0; i < backgrounds.Length; i++)
    {
      //  the parallax is the opposite of the camera movement because the previous frame multiplied by the scale
      float parallax = (previousCamPostition.x - cam.position.x) * parallaxScales[i];

      //  set a target x position which is the current position plus the calculated parallax
      float backgroundTargetPositionX = backgrounds[i].position.x + parallax;

      //  create a target position which is the background's current position with it's target x position
      Vector3 backgroundTargetPosition = new Vector3(backgroundTargetPositionX, backgrounds[i].position.y, backgrounds[i].position.z);

      //  fade between the current position and the taget position using LERP.
      backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPosition, smoothing * Time.deltaTime);

    }

    //   set previousCamPostition to the camera's position at the end of the frame
    previousCamPostition = cam.position;
  }
}
