using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirectoryPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirectoryPath, string dataFileName)
    {
        this.dataDirectoryPath = dataDirectoryPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load(string profileId)
    {
        if(profileId == null)
        {
            return null;
        }

        string fullPath = Path.Combine(dataDirectoryPath, profileId, dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error occured when trying to load data" + fullPath + "\n" + ex);
            }
        }
        return loadedData;
    }

    public void Save(GameData gameData, string profileId)
    {
        if(profileId == null)
        {
            return;
        }

        string fullPath = Path.Combine(dataDirectoryPath, profileId, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(gameData, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error occured when trying to save data" + fullPath + "\n" + ex);
        }
    }
    public void Delete(string profileId)
    {
        if (profileId == null)
        {
            return;
        }

        string fullPath = Path.Combine(dataDirectoryPath, profileId, dataFileName);
        try
        {
            if (File.Exists(fullPath))
            {
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            }
            else
            {
                Debug.LogWarning("Tried to delete profile data, but data was not found at path: " + fullPath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to delete profile data for profileId: "
                + profileId + " at path: " + fullPath + "\n" + e);
        }
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirectoryPath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;
            string fullPath = Path.Combine(dataDirectoryPath, profileId, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.Log("Skipping directory when loading all profiles because it does not contain data: " + profileId);
                continue;
            }
            GameData profileData = Load(profileId);
            if(profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {
                Debug.LogError("Something went horribly wrong in profile: " + profileId);
            }
        }
        return profileDictionary;
    }

    public string GetProfileId()
    {
        string newProfileId = null;
        Dictionary<string, GameData> profileGameData = LoadAllProfiles();
        foreach(KeyValuePair<string, GameData> pair in profileGameData)
        {
            string profileId =pair.Key;
            GameData gameData = pair.Value;

            if(gameData == null)
            {
                continue;
            }

            if(newProfileId == null)
            {
                newProfileId = profileId;
            }
        }
        return newProfileId;
    }
}