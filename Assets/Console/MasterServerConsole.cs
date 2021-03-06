﻿using UnityEngine;
using System.Collections;

public class MasterServerConsole : MonoBehaviour
{
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

    Windows.ConsoleWindow console = new Windows.ConsoleWindow();
    Windows.ConsoleInput input = new Windows.ConsoleInput();

    string strInput;

    public NetworkMasterServer server;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        console.Initialize();
        console.SetTitle("Celtic Survival Master Server");

        input.OnInputText += OnInputText;

        Application.logMessageReceived += HandleLog;

        Debug.Log("**************************************************");
        Debug.Log("*        Celtic Survival Master Server 1.0       *");
        Debug.Log("*        Copyright 2016 Idiosyncratic Games      *");
        Debug.Log("**************************************************");

        server.InitializeServer();
    }

    void OnInputText(string obj)
    {
        obj = obj.ToLower();
        //Debug.Log("On Input: " + obj);
        
        if(obj.Equals("stop"))
        {
            Debug.Log("Stopping");
            server.ResetServer();
        }
        else if (obj.Equals("start"))
        {
            Debug.Log("Starting");
            server.InitializeServer();
        }
    }

    void HandleLog(string message, string stackTrace, LogType type)
    {
        if (type == LogType.Warning)
            System.Console.ForegroundColor = System.ConsoleColor.Yellow;
        else if (type == LogType.Error)
            System.Console.ForegroundColor = System.ConsoleColor.Red;
        else
            System.Console.ForegroundColor = System.ConsoleColor.White;

        // We're half way through typing something, so clear this line ..
        if (System.Console.CursorLeft != 0)
        {
            input.ClearLine();
        }

        System.Console.WriteLine(message);

        // If we were typing something re-add it.
        input.RedrawInputLine();
    }

    //
    // Update the input every frame
    // This gets new key input and calls the OnInputText callback
    //
    void Update()
    {
        input.Update();
    }

    //
    // It's important to call console.ShutDown in OnDestroy
    // because compiling will error out in the editor if you don't
    // because we redirected output. This sets it back to normal.
    //
    void OnDestroy()
    {
        console.Shutdown();
    }

#endif
}
