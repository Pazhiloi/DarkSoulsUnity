using UnityEngine;
namespace SG
{
  public class DamagePlayer : MonoBehaviour
{
    public int damage = 25;

    private void OnTriggerEnter(Collider other) {
      PlayerStatsManager PlayerStatsManager = other.GetComponent<PlayerStatsManager>();

      if (PlayerStatsManager != null)
      {
        PlayerStatsManager.TakeDamage(damage);
        // aboba pro
      }
    }
}
}
