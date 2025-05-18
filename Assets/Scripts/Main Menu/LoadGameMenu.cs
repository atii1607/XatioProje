using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameMenu : MonoBehaviour
{
    private FileSlot[] fileSlots;
    private bool isLoadingGame = false;
    private string sceneToLoad;

    private void Awake()
    {
        fileSlots = this.GetComponentsInChildren<FileSlot>();
    }

    public void OnFileSlotsClicked(FileSlot fileSlot)
    {
        DisableMenuButtons();
        DataPersistenceManager.instance.ChangeSelectedProfileId(fileSlot.GetProfilesId());

        if (isLoadingGame)
        {
            DataPersistenceManager.instance.LoadGame();

            if (DataPersistenceManager.instance.gameData != null)
            {
                sceneToLoad = DataPersistenceManager.instance.gameData.lastSceneName;
            }

            else
            {
                sceneToLoad = "Level 1";
            }
        }

        else
        {
            DataPersistenceManager.instance.NewGame();
            sceneToLoad = "Level 1";
        }
        SceneManager.LoadSceneAsync(sceneToLoad);
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        this.gameObject.SetActive(true);
        this.isLoadingGame = isLoadingGame;

        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();
        foreach (FileSlot fileSlot in fileSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(fileSlot.GetProfilesId(), out profileData);
            fileSlot.SetData(profileData);

            if(profileData == null && isLoadingGame)
            {
                fileSlot.SetInteractable(false);
            }

            else
            {
                fileSlot.SetInteractable(true);
            }
        }

    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    public void DisableMenuButtons()
    {
        foreach(FileSlot fileSlot in fileSlots)
        {
            fileSlot.SetInteractable(false);
        }
    }
}
