using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public class BonfireInteractable : Interactable
  {
    [Header("Bonfire Teleport Transform")]
    public Transform bonfireTeleportTransform;

    [Header("Activation Status")]
    public bool hasBeenActivated;


    [Header("Bonfire FX")]
    public ParticleSystem activationFX;
    public ParticleSystem fireFX;
    public AudioClip bonfireActiationSoundFX;
    AudioSource audioSource;

    private void Awake()
    {

      if (hasBeenActivated)
      {
        fireFX.gameObject.SetActive(true);
        fireFX.Play();
        interactableText = "Rest";
      }
      else{
        interactableText = "Light Bonfire";
      }
      audioSource = GetComponent<AudioSource>();
    }
    public override void Interact(PlayerManager playerManager)
    {
      if (hasBeenActivated)
      {
      }
      else
      {
        playerManager.playerAnimatorManager.PlayTargetAnimation("Bonfire_Activate", true);
        playerManager.uiManager.ActivateBonfireLitPopUp();
        hasBeenActivated = true;
        interactableText = "Rest";
        activationFX.gameObject.SetActive(true);
        activationFX.Play();
        fireFX.gameObject.SetActive(true);
        fireFX.Play();
        audioSource.PlayOneShot(bonfireActiationSoundFX);
      }
      
    }

  }
}
