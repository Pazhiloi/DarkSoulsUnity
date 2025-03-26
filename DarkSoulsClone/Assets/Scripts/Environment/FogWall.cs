using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
  public class FogWall : MonoBehaviour
  {
    private void Awake()
    {
      DeactivateFogWall();
    }

    public void ActivateFogWall()
    {
      gameObject.SetActive(true);
    }
    public void DeactivateFogWall()
    {
      gameObject.SetActive(false);
    }
  }
}
