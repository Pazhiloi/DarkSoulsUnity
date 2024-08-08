using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG{
public class TorsoModelChanger : MonoBehaviour
{
    public List<GameObject> tosroModels;

    private void Awake()
    {
      GetAllTorsoModels();
    }

    private void GetAllTorsoModels()
    {
      int childrenGameObjects = transform.childCount;
      for (int i = 0; i < childrenGameObjects; i++)
      {
        tosroModels.Add(transform.GetChild(i).gameObject);
      }
    }
    public void UnequipAllTorsoModels()
    {
      foreach (GameObject torsoModel in tosroModels)
      {
        torsoModel.SetActive(false);
      }
    }

    public void EquipTorsoModelByName(string torsoName)
    {
      for (int i = 0; i < tosroModels.Count; i++)
      {
        if (tosroModels[i].name == torsoName)
        {
          tosroModels[i].SetActive(true);
        }
      }
    }
  }
}
