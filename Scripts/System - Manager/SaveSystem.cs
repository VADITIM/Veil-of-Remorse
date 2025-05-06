using System.IO; // Für Dateioperationen wie Lesen und Schreiben
using System.Runtime.Serialization.Formatters.Binary; // Für binäre Serialisierung von Objekten
using UnityEngine;

public static class SaveSystem
{
    // Speichert den Spielzustand aller relevanten Systeme in eine Datei
    public static void SaveGame(Movement movement, LevelSystem levelSystem, SkillTreeManager skillTreeManager, Player player, SoundManager soundManager)
    {  

        
        BinaryFormatter formatter = new BinaryFormatter(); // Serialisierer für binäre Daten
        string path = Application.persistentDataPath + "/player.sav"; // Speicherort für das Savegame

        FileStream stream = new FileStream(path, FileMode.Create); // Neue Datei zum Schreiben öffnen

        try
        {
            // Verpackt alle benötigten Daten in ein serialisierbares Objekt
            PlayerData data = new PlayerData(movement, levelSystem, skillTreeManager, player, soundManager);
            formatter.Serialize(stream, data); // Speichert die Daten binär
            Debug.LogWarning("SAVED\n");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Save Failed: " + e.Message); // Fehler beim Speichern
        }
        finally
        {
            stream.Close(); // Stream wird immer geschlossen, egal ob Erfolg oder Fehler
        }
    }

    // Lädt den Spielstand aus der Datei und gibt das gespeicherte Datenobjekt zurück
    public static PlayerData LoadGame()
    {
        string path = Application.persistentDataPath + "/player.sav";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            try 
            { 
                PlayerData data = formatter.Deserialize(stream) as PlayerData; 
                return data; // Gibt gespeicherte Daten zurück
            }
            catch (System.Exception e) 
            { 
                Debug.LogError("Load Failed: " + e.Message); 
                return null; 
            }
            finally 
            { 
                stream.Close(); 
            }
        }
        else return null; // Kein Savegame vorhanden
    }

    // Speichert ein boolesches Flag in den PlayerPrefs (z. B. ob eine Tür geöffnet wurde)
    public static void SaveBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0); // Umwandlung von bool zu int (0/1)
        PlayerPrefs.Save();
    }

    // Lädt ein boolesches Flag aus den PlayerPrefs (Standard: false)
    public static bool LoadBool(string key)
    {
        return PlayerPrefs.GetInt(key, 0) == 1;
    }

    // Entfernt ein boolesches Flag aus den PlayerPrefs
    public static void ResetBool(string key)
    {
        PlayerPrefs.DeleteKey(key);
        PlayerPrefs.Save();
    }

    // Löscht das komplette Savegame und setzt alle Schlüsselvariablen zurück
    public static void ResetSave()
    {
        string path = Application.persistentDataPath + "/player.sav";
        if (File.Exists(path))
        {
            File.Delete(path); // Speicherdatei löschen

            Key.ResetAllKeys(); // Alle gesammelten Schlüssel zurücksetzen
            SaveSystem.SaveBool("Key_Collected", false); // Beispiel-Flag neu setzen
            KeyDoor.ResetAllDoors(); // Alle Türen zurücksetzen

            // Fähigkeiten-Flags zurücksetzen
            ResetBool("AbilityOneUnlocked");
            ResetBool("AbilityTwoUnlocked");
            ResetBool("AbilityThreeUnlocked");

            Debug.LogWarning("SAVE DATA RESET\n");
        }
        else 
        {
            Debug.LogWarning("No save data to reset."); // Keine Datei vorhanden
        }
    }
}
