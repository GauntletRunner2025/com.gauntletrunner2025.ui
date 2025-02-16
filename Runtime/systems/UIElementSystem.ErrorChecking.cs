using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

public abstract partial class UIElementSystem<T, U> : SystemBase
{
    protected void LogError(string message)
    {
        if (WaitMode == WaitModeEnum.DoNotWait)
        {
            Log(message, LogType.Error);
            this.Enabled = false;
        }
    }

    private bool GetUIDocument(out UIDocument uiDocument, out VisualElement root)
    {
        uiDocument = GameObject.FindAnyObjectByType<UIDocument>();
        if (uiDocument == null)
        {
            LogError($"[{GetType().Name}] UIDocument not found in the scene.");
            root = null;
            return false;
        }

        root = uiDocument.rootVisualElement;
        return true;
    }

    private bool LoadFromFile()
    {
        Element = new T();
        if (Element == null)
        {
            LogError($"Failed to create new instance of UI Element type {typeof(T).Name}");
            return false;
        }
        return true;
    }

    private bool GetUIElement(VisualElement root)
    {
        Element = root.Q<T>(Name);
        if (Element == null)
        {
            LogError($"UI Element '{Name}' of type {typeof(T).Name} not found in the tree.");
            return false;
        }
        return true;
    }
}