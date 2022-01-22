using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
  public Camera mainCamera;
  float shakeAmount = 0;

  private void Awake()
  {
    if (mainCamera == null)
      mainCamera = Camera.main;
  }


  public void ShakeCamera(float amt, float length)
  {
    shakeAmount = amt;
    InvokeRepeating("DoShake", 0, 0.01f);
    Invoke("StopShake", length);
  }

  void DoShake()
  {
    if (shakeAmount > 0)
    {
      Vector3 camPos = mainCamera.transform.position;

      float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
      float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
      camPos.x += offsetX;
      camPos.y += offsetY;

      mainCamera.transform.position = camPos;

    }
  }

  void StopShake()
  {
    CancelInvoke("DoShake");
    mainCamera.transform.localPosition = Vector3.zero;
  }

}
