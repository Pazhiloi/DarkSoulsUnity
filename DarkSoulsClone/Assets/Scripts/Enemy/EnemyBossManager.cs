using UnityEngine;

namespace SG
{
  public class EnemyBossManager : MonoBehaviour
  {
    public string bossName;

    UIBossHealthBar bossHealthBar;
    EnemyStats enemyStats;
    EnemyAnimatorManager enemyAnimatorManager;
    BossCombatStanceState bossCombatStanceState;
    [Header("Second Phase FX")]
    public GameObject particleFX;

    private void Awake()
    {
      bossHealthBar = FindObjectOfType<UIBossHealthBar>();
      enemyStats = GetComponent<EnemyStats>();
      enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
      bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
    }

    private void Start()
    {
      bossHealthBar.SetBossName(bossName);
      bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
    }

    public void UpdateBossHealthBar(int currentHealth, int maxHealth)
    {
      bossHealthBar.SetBossCurrentHealth(currentHealth);
      if (currentHealth <= maxHealth / 2)
      {
        ShiftToSecondPhase();
      }
    }

    public void ShiftToSecondPhase()
    {
      enemyAnimatorManager.PlayTargetAnimation("PhaseShift", true);
      bossCombatStanceState.hasPhaseShifted = true;
    }
  }
}
