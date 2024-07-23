using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
  public class OpenChest : Interactable
  {
    Animator animator;
    OpenChest openChest;
    public GameObject itemSpawner;
    public WeaponItem itemInChest;
    public Transform playerStandingPosition;

    private void Awake()
    {
      animator = GetComponentInChildren<Animator>();
      openChest = this;
    }
    public override void Interact(PlayerManager playerManager)
    {
      Debug.Log("Interacting with chest");
      // rotate player towards the chest
      Vector3 rotationDIrection = transform.position - playerManager.transform.position;
      rotationDIrection.y = 0;
      rotationDIrection.Normalize();

      Quaternion tr = Quaternion.LookRotation(rotationDIrection);
      Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300f * Time.deltaTime);
      playerManager.transform.rotation = targetRotation;

      // lock the player transform to a certain point in front of chest
      playerManager.OpenChestInteraction(playerStandingPosition);
      animator.Play("Chest Open");

      StartCoroutine(SpawnItemInChest());
      WeaponPickUp weaponPickup = itemSpawner.GetComponent<WeaponPickUp>();
      if (weaponPickup != null)
      {
        weaponPickup.weapon = itemInChest;
      }
    }

    private IEnumerator SpawnItemInChest()
    {
      yield return new WaitForSeconds(1f);
      Instantiate(itemSpawner, transform);
      Destroy(openChest);
    }
  }
}
