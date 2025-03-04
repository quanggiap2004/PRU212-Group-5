using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirebaseLoginManager : MonoBehaviour
{
    // c?n m?t � �? nh?p v�o email, v� m?t � �? nh?p v�o password 
    [Header("Register")]
    public InputField isRegisterEmail;
    public InputField isRegisterPassword;

    public Button buttonRegister;

    // Firebase Authentication --> login, register 
    private FirebaseAuth auth;

    // ��ng nh?p 
    [Header("Login")]

    public InputField isLoginEmail;
    public InputField isLoginPassword;

    public Button buttonLogin;

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        buttonRegister.onClick.AddListener(RegisterAccountWithFirebase);
        buttonLogin.onClick.AddListener(SignInAccountWithFirebase);
    }
    public void RegisterAccountWithFirebase()
    {
        string email = isRegisterEmail.text;
        string password = isRegisterPassword.text;
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if(task.IsCanceled)
            {
                Debug.Log("Dang ky bi huy");
                return;
            }
            if (task.IsCompleted)
            {
                Debug.Log("dang ky thanh cong ");
                return;

            }
            if (task.IsFaulted)
            {
                Debug.Log("Dang ky bi that bai");
                return;
            }
        });
        // async laf hamf dong bo nen se tra lai ket qua cho minh
        // continuewithonmainhthread: de ho tro chuyen ui, bat buoc no nhay vao luong chinh de tac dong den ui cua game
    }

    public void SignInAccountWithFirebase()
    {
        string email = isLoginEmail.text;
        string password = isLoginPassword.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Dang nhap bi huy");
                return;
            }
            if (task.IsFaulted)
            {
                foreach (var exception in task.Exception.Flatten().InnerExceptions)
                {
                    FirebaseException firebaseEx = exception as FirebaseException;
                    if (firebaseEx != null)
                    {
                        Debug.LogError($"L?i ��ng nh?p: {firebaseEx.Message} | M? l?i: {firebaseEx.ErrorCode}");
                    }
                    else
                    {
                        Debug.LogError($"L?i kh�c: {exception.Message}");
                    }
                }
                return;
            }
            if (task.IsCompleted)
            {
                Debug.Log("Dang nhap thanh cong");
                FirebaseUser user = task.Result.User;
                // chuy?n m�n ch�i sau khi ��ng nh?p th�nh c�ng
                // c� 2 c�ch chuy?n sence, m?t l� theo s? th? t? c?a m�n ch�i hai l� chuy?n sence theo t�n. 
                SceneManager.LoadScene("Main Menu");
            }

        });
            }


    public string GetUserId()
    {
        FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            return user.UserId;
        }
        return null;
    }
}
