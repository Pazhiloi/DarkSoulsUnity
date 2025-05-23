using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public class WorldCharacterEffectsManager : MonoBehaviour
  {
    public static WorldCharacterEffectsManager instance;

    public PoisonBuildUpEffect poisonBuildUpEffect;
    public PoisonedEffect poisonedEffect;

    private void Awake()
    {
      HandleInstance();
    }


    private void HandleInstance()
    {
      if (instance == null)
      {
        instance = this;
      }
      else
      {
        Destroy(gameObject);
      }
    }
  }
}