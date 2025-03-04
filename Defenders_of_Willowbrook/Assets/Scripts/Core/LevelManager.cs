using UnityEngine;
using System.Collections.Generic;
using Firebase.Database;

public class LevelManager : MonoBehaviour
{
    private DatabaseReference databaseReference;
    public static LevelManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SaveLevelData(string userId, int level, int currentMoney, int waveNumber, int health)
    {
        Dictionary<string, object> levelData = new Dictionary<string, object>
        {
            { "currentMoney", currentMoney },
            { "waveNumber", waveNumber },
            { "health", health }
        };

        databaseReference.Child("users").Child(userId).Child($"level{level}").SetValueAsync(levelData)
            .ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log($"Dữ liệu Level {level} đã lưu thành công!");
                }
                else
                {
                    Debug.LogError("Lỗi khi lưu dữ liệu: " + task.Exception);
                }
            });
    }
}
