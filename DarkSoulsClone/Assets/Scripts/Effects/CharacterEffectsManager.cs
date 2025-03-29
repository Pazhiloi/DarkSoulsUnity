using UnityEngine;

namespace SG
{
  public class CharacterEffectsManager : MonoBehaviour
  {

    [Header("Damage FX")]
    public GameObject bloodSplatterFX;
    [Header("Weapon FX")]
    public WeaponFX rightWeaponFX;
    public WeaponFX  leftWeaponFX;

    [Header("Weapon FX")]
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

    public virtual void PlayBloodSplatterFX(Vector3 bloodSplatterLocation)
    {
      GameObject blood  = Instantiate(bloodSplatterFX, bloodSplatterLocation, Quaternion.identity);
    }
  }
}
