using Invert.Core;
using Invert.Core.GraphDesigner;
using UnityEditor;
using UnityEngine;

public class uFrameInspectorWindow : EditorWindow {
    private Vector2 _scrollPosition;

    [MenuItem("Window/uFrame/Inspector #&i")]
    internal static void ShowWindow()
    {
        var window = GetWindow<uFrameInspectorWindow>();
        window.title = "Inspector";
        Instance = window;
        window.Show();
    }

    public static uFrameInspectorWindow Instance { get; set; }

    public void OnGUI()
    {
        Instance = this;
        var rect = new Rect(0f, 0f, Screen.width, Screen.height).Pad(0,0,4,20);

        GUILayout.BeginArea(rect);
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
        InvertApplication.SignalEvent<IDrawInspector>(_ => _.DrawInspector(rect));
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    
    }

    public void Update()
    {
        Repaint();
    }

}

public class uFrameIssuesWindow : EditorWindow
{
    private Vector2 _scrollPosition;

    [MenuItem("Window/uFrame/Issues #&u")]
    internal static void ShowWindow()
    {
        var window = GetWindow<uFrameIssuesWindow>();
        window.title = "Issues";
        Instance = window;
        window.Show();
    }

    public static uFrameIssuesWindow Instance { get; set; }

    public void OnGUI()
    {
        Instance = this;
        var rect = new Rect(0f, 0f, Screen.width, Screen.height);

        GUILayout.BeginArea(rect);
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
        InvertApplication.SignalEvent<IDrawErrorsList>(_ => _.DrawErrors(rect));
        GUILayout.EndScrollView();
        GUILayout.EndArea();

    }

    public void Update()
    {
        Repaint();
    }

}

public class uFrameNavigationHistoryWindow : EditorWindow
{
    private Vector2 _scrollPosition;

    [MenuItem("Window/uFrame/Navigation History #&l")]
    internal static void ShowWindow()
    {
        var window = GetWindow<uFrameNavigationHistoryWindow>();
        window.title = "Nav History";
        Instance = window;
        window.Show();
    }

    public static uFrameNavigationHistoryWindow Instance { get; set; }

    public void OnGUI()
    {
        Instance = this;
        var rect = new Rect(0f, 0f, this.position.width, this.position.height);

        GUILayout.BeginArea(rect);
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
        InvertApplication.SignalEvent<IDrawNavigationHistory>(_ => _.DrawNavigationHistory(rect));
        GUILayout.EndScrollView();
        GUILayout.EndArea();

    }

    public void Update()
    {
        Repaint();
    }

}

public interface IDrawNavigationHistory
{
    void DrawNavigationHistory(Rect rect);
}

