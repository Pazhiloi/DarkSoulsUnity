using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MR
{
  public class WorldSaveGameManager : MonoBehaviour
  {
    public static WorldSaveGameManager instance;
    public PlayerManager player;
    [Header("Save Data Writer")]
    SaveGameDataWriter saveGameDataWriter;

    [Header("Current Character Data")]
    //CHARACTER SLOT #
    public CharacterSaveData currentCharacterSaveData;
    [SerializeField] private string fileName;

    [Header("SAVE/LOAD")]
    [SerializeField] bool saveGame;
    [SerializeField] bool loadGame;

    private void Awake()
    {
      HandleInstance();
    }

    private void Start()
    {
      DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
      if (saveGame)
      {
        saveGame = false;
        SaveGame();
      }
      else if (loadGame)
      {
        loadGame = false;
        LoadGame();
      }
    }


    public void SaveGame()
    {
      saveGameDataWriter = new SaveGameDataWriter();
      saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
      saveGameDataWriter.dataSaveFileName = fileName;

      // Pass along our characters data to the current save file
      player.SaveCharacterDataToCurrentSaveData(ref currentCharacterSaveData);

      // Write the current character data to a json file and save it on this device
      saveGameDataWriter.WriteCharacterDataToSaveFile(currentCharacterSaveData);

      Debug.Log("SAVING GAME...");
      Debug.Log("FILE SAVED AS: " + fileName);
    }

    public void  LoadGame(){
      saveGameDataWriter = new SaveGameDataWriter();
      saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
      saveGameDataWriter.dataSaveFileName = fileName;
      currentCharacterSaveData = saveGameDataWriter.LoadCharacterDataFromJson();

      StartCoroutine(LoadWorldSceneAsynchronously());
    }

    private IEnumerator LoadWorldSceneAsynchronously()
    {
      if (player == null)
      {
        player = FindObjectOfType<PlayerManager>();
      }


      AsyncOperation loadOperation = SceneManager.LoadSceneAsync(0); // Assuming index 0 is your world scene

      while (!loadOperation.isDone)
      {
        // You can update a loading bar here if you have one
        float loadingProgress = Mathf.Clamp01(loadOperation.progress / 0.9f);
        yield return null;
      }

      player.LoadCharacterDataFromCurrentCharacterSaveData(ref currentCharacterSaveData);

    }















    private void HandleInstance()
    {
      if (instance == null)
      {
        instance = this;
      }
      else
      {
        Destroy(gameObject);
      }
    }
  }
}
