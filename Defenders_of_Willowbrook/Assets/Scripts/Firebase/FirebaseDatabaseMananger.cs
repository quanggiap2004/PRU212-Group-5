//using Firebase;
//using Firebase.Database;
//using Firebase.Extensions;
//using UnityEngine;

//public class FirebaseDatabaseMananger : MonoBehaviour
//{
//    private DatabaseReference reference;
//    // Phần này sẽ luôn luôn được gọi trước khi game gọi
//    private void Awake()
//    {
//        FirebaseApp app = FirebaseApp.DefaultInstance;
//        reference = FirebaseDatabase.DefaultInstance.RootReference;

//    }
//    private void Start()
//    {
//        //WriteDatabase("123", "Xin chào thế giới");
//        ReadDatabase("123");
//        // Thay vì để là ID thì mình có thể để nguyên đường dẫn luôn để trỏ đến thư mục mình muốn nó ghi vào. 
//    }
//    public void WriteDatabase(string id, string message)
//    {
//        //  Giải thích cho dòng lệnh này: 
//        reference.Child("Users").Child(id).SetValueAsync(message).ContinueWithOnMainThread(task =>
//        {
//            if (task.IsCompleted)
//            {
//                Debug.Log("Recorded successfully");

//            }
//            else
//            {
//                Debug.Log("Recorded Data unsuccessfully");
//            }
//        }

//        );
//    }
//    public void ReadDatabase(string id)
//    {
//        reference.Child("Users").Child(id).GetValueAsync().ContinueWithOnMainThread(task =>
//        {
//            if (task.IsCompleted)
//            {
//                DataSnapshot snapshot = task.Result;
//                Debug.Log("Read Data successfully" + snapshot.Value.ToString());

//            }
//            else
//            {
//                Debug.Log("Read Data Unsuccesffully" + task.Exception);
//            }
//        });

//    }
//}




//using Firebase;
//using Firebase.Database;
//using Firebase.Extensions;
//using UnityEngine;
//using System.Collections.Generic;

//public class FirebaseDatabaseMananger : MonoBehaviour
//{
//    private DatabaseReference reference;

//    private void Awake()
//    {
//        FirebaseApp app = FirebaseApp.DefaultInstance;
//        reference = FirebaseDatabase.DefaultInstance.RootReference;
//    }

//    public void WriteDatabase(string path, Dictionary<string, object> data)
//    {
//        reference.Child(path).UpdateChildrenAsync(data).ContinueWithOnMainThread(task =>
//        {
//            if (task.IsCompleted)
//            {
//                Debug.Log($"Data saved successfully at {path}");
//            }
//            else
//            {
//                Debug.LogError($"Failed to save data at {path}: {task.Exception}");
//            }
//        });
//    }

//    public void ReadDatabase(string path, System.Action<Dictionary<string, object>> onComplete)
//    {
//        reference.Child(path).GetValueAsync().ContinueWithOnMainThread(task =>
//        {
//            if (task.IsCompleted && task.Result.Exists)
//            {
//                Dictionary<string, object> data = new Dictionary<string, object>();
//                foreach (var child in task.Result.Children)
//                {
//                    data[child.Key] = child.Value;
//                }
//                onComplete?.Invoke(data);
//            }
//            else
//            {
//                Debug.Log($"No data found at {path}");
//                onComplete?.Invoke(null);
//            }
//        });
//    }
//}





//using Firebase;
//using Firebase.Database;
//using Firebase.Extensions;
//using UnityEngine;
//using System.Collections.Generic;

//public class FirebaseDatabaseMananger : MonoBehaviour
//{
//    private DatabaseReference reference;
//    private FirebaseLoginManager loginManager;

//    private void Awake()
//    {
//        FirebaseApp app = FirebaseApp.DefaultInstance;
//        reference = FirebaseDatabase.DefaultInstance.RootReference;
//        loginManager = FindObjectOfType<FirebaseLoginManager>();
//    }

//    private string GetCurrentUserId()
//    {
//        if (loginManager != null)
//        {
//            string userId = loginManager.GetUserId();
//            return !string.IsNullOrEmpty(userId) ? userId : "Guest"; // Nếu chưa login, dùng "Guest"
//        }
//        return "Guest";
//    }

//    public void WriteDatabase(string path, Dictionary<string, object> data)
//    {
//        string userId = GetCurrentUserId();
//        string fullPath = $"players/{userId}/{path}";

//        reference.Child(fullPath).UpdateChildrenAsync(data).ContinueWithOnMainThread(task =>
//        {
//            if (task.IsCompleted)
//            {
//                Debug.Log($"Data saved successfully at {fullPath}");
//            }
//            else
//            {
//                Debug.LogError($"Failed to save data at {fullPath}: {task.Exception}");
//            }
//        });
//    }

//    public void ReadDatabase(string path, System.Action<Dictionary<string, object>> onComplete)
//    {
//        string userId = GetCurrentUserId();
//        string fullPath = $"players/{userId}/{path}";

//        reference.Child(fullPath).GetValueAsync().ContinueWithOnMainThread(task =>
//        {
//            if (task.IsCompleted && task.Result.Exists)
//            {
//                Dictionary<string, object> data = new Dictionary<string, object>();
//                foreach (var child in task.Result.Children)
//                {
//                    data[child.Key] = child.Value;
//                }
//                onComplete?.Invoke(data);
//            }
//            else
//            {
//                Debug.Log($"No data found at {fullPath}");
//                onComplete?.Invoke(null);
//            }
//        });
//    }
//}
