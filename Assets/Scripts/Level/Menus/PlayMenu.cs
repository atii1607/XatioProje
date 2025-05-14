using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private LoadGameMenu loadGameMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    
    public void OnNewGameClicked()
    {
        loadGameMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }

    public void OnLoadGameClicked()
    {
        loadGameMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

}
