using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Profiling;

public class Utils : MonoBehaviour
{
    static Utils instance;

    private void Awake()
    {
        instance = this;
    }

    public static void Run(string command, bool waitForExit = true)
    {
        Process myProcess = new Process();

        myProcess.StartInfo.FileName = "C:\\Windows\\system32\\cmd.exe";
        myProcess.StartInfo.CreateNoWindow = true;
        myProcess.StartInfo.UseShellExecute = false;
        myProcess.StartInfo.Arguments = "/c" + command;

        myProcess.Start();

        if (waitForExit)
            myProcess.WaitForExit();
    }
}
