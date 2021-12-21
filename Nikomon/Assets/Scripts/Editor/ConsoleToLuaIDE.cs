using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;
using UnityEditor.Callbacks;
using System;
 
public class ConsoleToLuaIDE : UnityEditor.Editor
{
    private const string LUA_IDE_PATH_KEY = "mTv8";
    private const string LUA_PROJECT_PATH_KEY = "obUd";
 
    [MenuItem("Tools/Set Lua IDE Path")]
    static void SetLuaIDEPath()
    {
        string path = EditorUserSettings.GetConfigValue(LUA_IDE_PATH_KEY);
        path = EditorUtility.OpenFilePanel("Select Lua IDE Path", path, "exe");
 
        if (path != "")
        {
            EditorUserSettings.SetConfigValue(LUA_IDE_PATH_KEY, path);
            Debug.Log("Set Lua IDE Path: " + path);
        }
    }
 
    [MenuItem("Tools/Set Lua Project Path")]
    static void SetLuaProjectPath()
    {
        string path = EditorUserSettings.GetConfigValue(LUA_PROJECT_PATH_KEY);
        path = EditorUtility.OpenFolderPanel("Select Lua Project Path", path, "");
 
        if (path != "")
        {
            EditorUserSettings.SetConfigValue(LUA_PROJECT_PATH_KEY, path);
            Debug.Log("Set Lua Project Path: " + path);
        }
    }
 
    // 双击控制台的回调,true按照自己方式处理,false按照unity默认方式处理
    [OnOpenAssetAttribute(2)]
    public static bool OnOpen(int instanceID, int line)
    {
        if (!GetConsoleWindowListView() || (object)EditorWindow.focusedWindow != consoleWindow)
        {
            return false;
        }
 
        string fileName = HandleLuaLog(ref line);
        if (fileName != null)
        {
            return OpenLuaAsset(fileName, line);
        }
 
        return false;
    }
 
    public static bool OpenLuaAsset(string fileName, int line)
    {
        // 设置ide路径
        string idePath = EditorUserSettings.GetConfigValue(LUA_IDE_PATH_KEY);
        if (string.IsNullOrEmpty(idePath) || !File.Exists(idePath))
        {
            SetLuaIDEPath();
            idePath = EditorUserSettings.GetConfigValue(LUA_IDE_PATH_KEY);
        }
 
        // 设置project path
        string projectPath = EditorUserSettings.GetConfigValue(LUA_PROJECT_PATH_KEY);
        if (string.IsNullOrEmpty(projectPath))
        {
            SetLuaProjectPath();
            projectPath = EditorUserSettings.GetConfigValue(LUA_PROJECT_PATH_KEY);
        }
        string filePath = projectPath.Trim() + "/src/" + fileName.Trim();
 
        // 启动ide打开filePath对应文件，并定位到line对应行
        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        proc.StartInfo.FileName = idePath;
        string procArgument = "";
        if (idePath.IndexOf("idea") != -1)
        {
            procArgument = string.Format("{0} --line {1} {2}", projectPath, line, filePath);
        }
        else
        {
            procArgument = string.Format("{0}:{1}:0", filePath, line);
        }
        proc.StartInfo.Arguments = procArgument;
        proc.Start();
 
        return true;
    }
 
    // 窗口对象
    private static object consoleWindow;
    // 视图列表对象
    private static object logListView;
    // 视图列表指定行
    private static FieldInfo logListViewCurrentRow;
    // 获取日志对象方法
    private static MethodInfo LogEntriesGetEntry;
    // 日志对象
    private static object logEntry;
    // 日志内容
    private static FieldInfo logEntryCondition;
 
    private static bool GetConsoleWindowListView()
    {
        if (logListView == null)
        {
            Assembly unityEditorAssembly = Assembly.GetAssembly(typeof(EditorWindow));
            Type consoleWindowType = unityEditorAssembly.GetType("UnityEditor.ConsoleWindow");
            FieldInfo fieldInfo = consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
            consoleWindow = fieldInfo.GetValue(null);
 
            if (consoleWindow == null)
            {
                logListView = null;
                return false;
            }
 
            FieldInfo listViewFieldInfo = consoleWindowType.GetField("m_ListView", BindingFlags.Instance | BindingFlags.NonPublic);
            logListView = listViewFieldInfo.GetValue(consoleWindow);
            logListViewCurrentRow = listViewFieldInfo.FieldType.GetField("row", BindingFlags.Instance | BindingFlags.Public);
 
#if UNITY_2017_1_OR_NEWER
            Type logEntriesType = unityEditorAssembly.GetType("UnityEditor.LogEntries");
            LogEntriesGetEntry = logEntriesType.GetMethod("GetEntryInternal", BindingFlags.Static | BindingFlags.Public);
            Type logEntryType = unityEditorAssembly.GetType("UnityEditor.LogEntry");
#else
            Type logEntriesType = unityEditorAssembly.GetType("UnityEditorInternal.LogEntries");
            LogEntriesGetEntry = logEntriesType.GetMethod("GetEntryInternal", BindingFlags.Static | BindingFlags.Public);
            Type logEntryType = unityEditorAssembly.GetType("UnityEditorInternal.LogEntry");
#endif
 
            logEntry = Activator.CreateInstance(logEntryType);
            logEntryCondition = logEntryType.GetField("condition", BindingFlags.Instance | BindingFlags.Public);
        }
 
        return true;
    }
 
    private static string GetLog()
    {
        // 获取当前选中控制台指定行
        int row = (int)logListViewCurrentRow.GetValue(logListView);
        // 获取指定行日志对象
        LogEntriesGetEntry.Invoke(null, new object[] { row, logEntry });
        // 获取指定日志对象内容
        return logEntryCondition.GetValue(logEntry) as string;
    }
 
    // tolua日志处理
    private static string HandleLuaLog(ref int line)
    {
        string condition = GetLog();
        condition = condition.Substring(0, condition.IndexOf('\n'));
 
        int index = condition.IndexOf(".lua:");
        if (index >= 0)
        {
            int start = condition.IndexOf("[");
            int end = condition.IndexOf("]:");
            string _line = condition.Substring(index + 5, end - index - 5);
            Int32.TryParse(_line, out line);
            return condition.Substring(start + 1, index + 3 - start);
        }
 
        return null;
    }
}