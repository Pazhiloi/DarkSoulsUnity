using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MR
{
  public class DestroyAfterCastingSpell : MonoBehaviour
  {
    CharacterManager characterCastingSpell;

    private void Awake()
    {
      characterCastingSpell = GetComponentInParent<CharacterManager>();
    }
    void Update()
    {
      if (characterCastingSpell.isFiringSpell)
      {
        Destroy(this.gameObject);
      }
    }
  }
}
