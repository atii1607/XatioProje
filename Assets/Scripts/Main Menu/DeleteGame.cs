using UnityEngine;

public class DeleteGame : MonoBehaviour
{
    public void OnClearClicked(FileSlot fileSlot)
    {
        DataPersistenceManager.instance.DeleteProfileData(fileSlot.GetProfilesId());
    }

}
