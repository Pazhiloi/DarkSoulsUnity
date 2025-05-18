using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public class StaticCharacterEffect : ScriptableObject
  {
    public int effectID;
    public virtual void AddStaticEffect(CharacterManager character)
    {
      // This is a virtual method that can be overridden by child classes
      // to implement the addition of a static character effect.
    }

    public virtual void RemoveStaticEffect(CharacterManager character)
    {
      // This is a virtual method that can be overridden by child classes
      // to implement the removal of a static character effect.
    }

  }
}
