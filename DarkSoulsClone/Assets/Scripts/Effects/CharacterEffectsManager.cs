using UnityEngine;

namespace SG
{
  public class CharacterEffectsManager : MonoBehaviour
  {
    public WeaponFX rightWeaponFX, leftWeaponFX;
    public virtual void PlayWeaponFX(bool isLeft)
    {
      if (!isLeft)
      {
        if (rightWeaponFX != null)
        {
          rightWeaponFX.PlayWeaponFX();
        }
      }
      else
      {
        if (leftWeaponFX != null)
        {
          leftWeaponFX.PlayWeaponFX();
        }
      }
    }
  }
}
