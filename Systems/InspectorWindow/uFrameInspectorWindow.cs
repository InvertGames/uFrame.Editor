using Invert.Core;
using Invert.Core.GraphDesigner;
using UnityEditor;
using UnityEngine;

public class uFrameInspectorWindow : EditorWindow {
    private Vector2 _scrollPosition;
    private Vector2 _scrollPosition2;

    [MenuItem("uFrame/Inspector #&i")]
    internal static void ShowWindow()
    {
        var window = GetWindow<uFrameInspectorWindow>();
        window.title = "uFrame Inspector";
        Instance = window;
        window.Show();
    }

    public static uFrameInspectorWindow Instance { get; set; }

    public void OnGUI()
    {
        Instance = this;
        var rect = new Rect(0f, 0f, Screen.width, Screen.height);
        var left = rect.LeftHalf();
        var right = rect.RightHalf();

        GUILayout.BeginArea(left);
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
        InvertApplication.SignalEvent<IDrawInspector>(_ => _.DrawInspector(left));
        GUILayout.EndScrollView();
        GUILayout.EndArea();

        GUILayout.BeginArea(right);
        _scrollPosition2 = GUILayout.BeginScrollView(_scrollPosition2);
        InvertApplication.SignalEvent<IDrawErrorsList>(_ => _.DrawErrors(right));
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    
    }
}