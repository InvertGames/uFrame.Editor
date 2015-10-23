﻿using System;
using System.Text;
using Mono.CSharp;
using UnityEditor;
using UnityEngine;

namespace Invert.Core.GraphDesigner.Unity
{
    public class UnityPlatform : DiagramPlugin, IPlatformOperations, IDebugLogger
    {

        //public void ShowFileDialog(string title)
        //{
        //    EditorUtility.OpenFilePanel(title,directory,)
        //}

        public void OpenScriptFile(string filePath)
        {
            var scriptAsset = AssetDatabase.LoadAssetAtPath(filePath, typeof(TextAsset));
            AssetDatabase.OpenAsset(scriptAsset);
        }

        public void OpenLink(string link)
        {
            Application.OpenURL(link);
        }

        public string GetAssetPath(object graphData)
        {
            return AssetDatabase.GetAssetPath(graphData as UnityEngine.Object);
        }
        public bool MessageBox(string title, string message, string ok)
        {
            return EditorUtility.DisplayDialog(title, message, ok);
        }
        public bool MessageBox(string title, string message, string ok, string cancel)
        {
            return EditorUtility.DisplayDialog(title, message, ok, cancel);
        }

        public void SaveAssets()
        {
            AssetDatabase.SaveAssets();
        }

        public void RefreshAssets()
        {
            AssetDatabase.Refresh();
            //AssetDatabase.Refresh();
        }

        public void Progress(float progress, string message)
        {
            try
            {
                InvertApplication.SignalEvent<ITaskProgressHandler>(_=>_.Progress(progress, message));
                //if (progress > 100f)
                //{
                //    EditorUtility.ClearProgressBar();
                //    return;
                //}
                //EditorUtility.DisplayProgressBar("Generating", message, progress/1f);
            }
            catch (Exception ex)
            {
                
            }
        }

        public void Log(string message)
        {
            Debug.Log(message); 
        }

        public void LogException(Exception ex)
        {
            Debug.LogException(ex);
            if (ex.InnerException != null)
            {
                Debug.LogException(ex.InnerException);
            }
        }
    }
}
