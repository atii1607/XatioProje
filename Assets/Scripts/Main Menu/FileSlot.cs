using UnityEngine;
using UnityEngine.UI;

public class FileSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noData;
    [SerializeField] private GameObject hasData;
    private Button emptyButton;

    private void Awake()
    {
        emptyButton = this.GetComponent<Button>();
    }

    public void SetData(GameData gameData)
    {
        if(gameData == null)
        {
            noData.SetActive(true);
            hasData.SetActive(false);
        }

        else
        {
            noData.SetActive(false);
            hasData.SetActive(true);
        }
    }

    public string GetProfilesId()
    {
        return this.profileId;
    }

    public void SetInteractable(bool interactable)
    {
        emptyButton.interactable = interactable;
    }
}
