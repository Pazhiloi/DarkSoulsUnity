using UnityEngine;

namespace SG
{
  public class EnemyBossManager : MonoBehaviour
  {
    public string bossName;

    UIBossHealthBar bossHealthBar;
    EnemyStatsManager enemyStatsManager;
    EnemyAnimatorManager enemyAnimatorManager;
    BossCombatStanceState bossCombatStanceState;
    [Header("Second Phase FX")]
    public GameObject particleFX;

    private void Awake()
    {
      bossHealthBar = FindObjectOfType<UIBossHealthBar>();
      enemyStatsManager = GetComponent<EnemyStatsManager>();
      enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
      bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
    }

    private void Start()
    {
      bossHealthBar.SetBossName(bossName);
      bossHealthBar.SetBossMaxHealth(enemyStatsManager.maxHealth);
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
      enemyAnimatorManager.animator.SetBool("isInvulnerable", true);
      enemyAnimatorManager.animator.SetBool("isPhaseShifting", true);
      enemyAnimatorManager.PlayTargetAnimation("Phase Shift", true);
      bossCombatStanceState.hasPhaseShifted = true;
    }
  }
}
