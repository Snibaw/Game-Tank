// using System.IO;
// using System.Runtime.Serialization.Formatters.Binary;
// using UnityEngine;

// public static class SaveSystem 
// {
//     public static void SavePlayer(PlayerStats playerStats )
//     {
//         BinaryFormatter formatter = new BinaryFormatter();
//         string path = Application.persistentDataPath + "/player.tank"; // Create a path to the file
//         FileStream stream = new FileStream(path, FileMode.Create); // Create a binary file
        
//         PlayerData data = new PlayerData(playerStats); // Create a new PlayerData object
        
//         formatter.Serialize(stream, data); // Serialize the data into the file
//         stream.Close(); 
//     }
//     public static PlayerData LoadPlayer()
//     {
//         string path = Application.persistentDataPath + "/player.tank"; // Create a path to the file
//         if(File.Exists(path))
//         {
//             BinaryFormatter formatter = new BinaryFormatter();
//             FileStream stream = new FileStream(path, FileMode.Open); // Open the file
            
//             PlayerData data = formatter.Deserialize(stream) as PlayerData; // Deserialize the data from the file
//             stream.Close();
            
//             return data;
//         }
//         else
//         {
//             Debug.LogError("Save file not found in " + path);
//             return null;
//         }
//     }
// }
