using UnityEngine;

namespace SG
{
  public class DestroyAfterTime : MonoBehaviour
  {
    public float timeUntilDestroyed = 3f;

    private void Awake()
    {
      Destroy(gameObject, timeUntilDestroyed);
    }
  }
}
