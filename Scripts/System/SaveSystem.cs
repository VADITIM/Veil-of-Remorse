using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveGame(Movement movement, LevelSystem levelSystem, AbilityManager abilityManager, Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.sav";

        FileStream stream = new FileStream(path, FileMode.Create);

        try { PlayerData data = new PlayerData(movement, levelSystem, abilityManager, player); formatter.Serialize(stream, data); Debug.LogWarning("SAVED\n"); }
        catch (System.Exception e) { Debug.LogError("Save Failed: " + e.Message); }
        finally { stream.Close(); }
    }

    public static PlayerData LoadGame()
    {
        string path = Application.persistentDataPath + "/player.sav";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            try { PlayerData data = formatter.Deserialize(stream) as PlayerData; return data; }
            catch (System.Exception e) { Debug.LogError("Load Failed: " + e.Message); return null; }
            finally { stream.Close(); }
        }
        else return null;
    }

    public static void SaveBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static bool LoadBool(string key)
    {
        return PlayerPrefs.GetInt(key, 0) == 1;
    }

    public static void ResetBool(string key)
    {
        PlayerPrefs.DeleteKey(key);
        PlayerPrefs.Save();
    }

    public static void ResetSave()
    {
        string path = Application.persistentDataPath + "/player.sav";
        if (File.Exists(path))
        {
            File.Delete(path);
            ResetBool("AbilityOneUnlocked");
            ResetBool("AbilityTwoUnlocked");
            ResetBool("AbilityThreeUnlocked");
            Debug.LogWarning("SAVE DATA RESET\n");
        }
        else Debug.LogWarning("No save data to reset.");
    }
}