using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public class CharacterEffect : ScriptableObject
  {
    public int effectID;
    public virtual void ProcessEffect(CharacterManager character)
    {
      // This is a virtual method that can be overridden by child classes
      // to implement specific character effects.
    }
    }
  }
