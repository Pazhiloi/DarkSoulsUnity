using UnityEngine;

namespace SG
{
  public class WorldEventManager : MonoBehaviour
  {
    public UIBossHealthBar bossHealthBar;
    public EnemyBossManager boss;

    public bool bossFightIsActive, bossHasBeenAwakened, bossHasBeenDefeated;

    private void Awake() {
      bossHealthBar = FindObjectOfType<UIBossHealthBar>();
    }

    public void ActivateBossFight() {
      bossFightIsActive = true;
      bossHasBeenAwakened = true;
      bossHealthBar.SetUIHealthBarToActive();
    }

    public void BossHasBeenDefeated(){
      bossHasBeenDefeated = true;
      bossHasBeenAwakened = false;
    }
  }
}
