using UnityEngine;

namespace MR
{
  public class EnemyBossManager : MonoBehaviour
  {
    public string bossName;

    UIBossHealthBar bossHealthBar;
    AICharacterManager aiCharacter;
    BossCombatStanceState bossCombatStanceState;
    [Header("Second Phase FX")]
    public GameObject particleFX;

    private void Awake()
    {
      bossHealthBar = FindObjectOfType<UIBossHealthBar>();
      aiCharacter = GetComponent<AICharacterManager>();
      bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
    }

    private void Start()
    {
      bossHealthBar.SetBossName(bossName);
      bossHealthBar.SetBossMaxHealth(aiCharacter.enemyStatsManager.maxHealth);
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
      aiCharacter.animator.SetBool("isInvulnerable", true);
      aiCharacter.animator.SetBool("isPhaseShifting", true);
      aiCharacter.enemyAnimatorManager.PlayTargetAnimation("Phase Shift", true);
      bossCombatStanceState.hasPhaseShifted = true;
    }
  }
}
