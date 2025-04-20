using UnityEngine;

namespace MR
{
  public class EnemyBossManager : MonoBehaviour
  {
    public string bossName;

    UIBossHealthBar bossHealthBar;
    EnemyManager enemy;
    BossCombatStanceState bossCombatStanceState;
    [Header("Second Phase FX")]
    public GameObject particleFX;

    private void Awake()
    {
      bossHealthBar = FindObjectOfType<UIBossHealthBar>();
      enemy = GetComponent<EnemyManager>();
      bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
    }

    private void Start()
    {
      bossHealthBar.SetBossName(bossName);
      bossHealthBar.SetBossMaxHealth(enemy.enemyStatsManager.maxHealth);
    }

    public void UpdateBossHealthBar(int currentHealth, int maxHealth)
    {
      bossHealthBar.SetBossCurrentHealth(currentHealth);
      if (currentHealth <= maxHealth / 2 && !bossCombatStanceState.hasPhaseShifted)
      {
        bossCombatStanceState.hasPhaseShifted = true;
        ShiftToSecondPhase();
      }
    }

    public void ShiftToSecondPhase()
    {
      enemy.animator.SetBool("isInvulnerable", true);
      enemy.animator.SetBool("isPhaseShifting", true);
      enemy.enemyAnimatorManager.PlayTargetAnimation("Phase Shift", true);
      bossCombatStanceState.hasPhaseShifted = true;
    }
  }
}
