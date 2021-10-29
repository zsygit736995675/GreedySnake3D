﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LGToolEditor : EditorWindow
{
    [MenuItem("LG_Tool/搜索文本内容")]
    public static void Sreach()
    {
        InputSearch Is = GetWindow<InputSearch>("文本搜索工具");
        Is.Show();
    }

    [MenuItem("LG_Tool/宏定义")]
    public static void Settings()
    {
        SettingWindows sw = GetWindow<SettingWindows>("宏定义工具");
        sw.Show();
    }

    [MenuItem("LG_Tool/一键打包")]
    public static void showBuildPanel()
    {
        BuildTools bt = GetWindow<BuildTools>("游戏打包工具");
        bt.Show();
        bt.SetPass();
    }

    [MenuItem("LG_Tool/打开缓存文件夹")]
    public static void OpenFile()
    {
        System.Diagnostics.Process.Start(Application.persistentDataPath);

    }

    [MenuItem("LG_Tool/一键添加文件夹")]
    public static void AddFileInfo()
    {
        CreateDirectory("Atlas");
        CreateDirectory("Scripts");
        CreateDirectory("Scenes");
        CreateDirectory("Model");
        CreateDirectory("Plugins");
        CreateDirectory("Editor");
        CreateDirectory("Prefabs");
        CreateDirectory("Resources");
        CreateDirectory("Thread");
        //AssetDatabase. Refresh();
    }

    private static bool CreateDirectory(string name)
    {

        if (!Directory.Exists(name))
        {
            Debug.Log("创建文件路径==" + GetPath + name);
            Directory.CreateDirectory(GetPath + name);
            AssetDatabase.Refresh();
            return false;
        }
        else
        {
            Debug.LogError(name + " 文件夹已存在!");
            return true;
        }
    }

    private static string GetPath
    {
        get { return Application.dataPath + "/"; }
    }

    [MenuItem("LG_Tool/一键图片改精灵 %#F1")]
    public static void EditTexture()
    {
        Transform [] SeleGameObj = Selection.transforms;
        Debug.LogError("SeleGameObj.length" + SeleGameObj.Length);
        foreach (var item in SeleGameObj)
        {
            if (item.GetComponent<Texture>() == null)
            {
                Debug.LogError(item + " 不是texture！");
                continue;
            }
            string path = AssetDatabase.GetAssetPath(item);
            TextureImporter texture = AssetImporter.GetAtPath(path) as TextureImporter;
            texture.textureType = TextureImporterType.Sprite;
            //texture. spritePackingTag = "allAtlas";
            texture.spritePixelsPerUnit = 1;
            texture.filterMode = FilterMode.Trilinear;
            texture.mipmapEnabled = false;
            Debug.LogError(texture.name + "Success");
            AssetDatabase.ImportAsset(path);
            AssetDatabase.Refresh();
        }
    }

    [MenuItem("LG_Tool/一键更换字体 %#F2")]
    public static void Open()
    {
        GetWindow(typeof(LGToolEditor));
    }

    private Font toChange;
    private static Font toChangeFont;
    private FontStyle toFontStyle;
    private static string toFontSize;
    private static FontStyle toChangeFontStyle;

    private void OnGUI()
    {
        toChange = (Font)EditorGUILayout.ObjectField(toChange, typeof(Font), true, GUILayout.MinWidth(100f));
        toChangeFont = toChange;
        toFontStyle = (FontStyle)EditorGUILayout.EnumPopup(toFontStyle, GUILayout.MinWidth(100f));
        toFontSize = EditorGUILayout.TextField("字体大小", toFontSize, GUILayout.MinWidth(100f));
        toChangeFontStyle = toFontStyle;
        if (GUILayout.Button("更换"))
        {
            Change();
        }

    }

    public static void Change()
    {
        Transform canvas = Selection.activeTransform;
        if (!canvas)
        {
            Debug.Log("NO Canvas");
            return;
        }
        Transform [] tArray = canvas.GetComponentsInChildren<Transform>(true);
        Debug.Log(tArray.Length);
        for (int i = 0; i < tArray.Length; i++)
        {
            Text t = tArray [ i ].GetComponent<Text>();
            if (t)
            {
                Undo.RecordObject(t, t.gameObject.name);
                t.font = toChangeFont;
                t.fontStyle = toChangeFontStyle;
                t.fontSize = int.Parse(toFontSize);
                //相当于让他刷新下 不然unity显示界面还不知道自己的东西被换掉了  还会呆呆的显示之前的东西
                EditorUtility.SetDirty(t);
            }
        }
        Debug.Log("Succed");
    }
    [MenuItem("LG_Tool/一键添加描边 %#F11")]
    public static void AddOutlin()
    {
        Transform canvas = Selection.activeTransform;
        if (!canvas)
        {
            Debug.Log("NO Canvas");
            return;
        }
        Transform [] tArray = canvas.GetComponentsInChildren<Transform>(true);
        Debug.Log(tArray.Length);
        for (int i = 0; i < tArray.Length; i++)
        {
            Text t = tArray [ i ].GetComponent<Text>();
            if (t)
            {
                //t.transform.GetOrAddComponent<Outline>(). effectDistance = new Vector2(3, -3);
                if (t.GetComponent<Outline>() == null)
                {
                    Outline outline = t.gameObject.AddComponent<Outline>();
                    outline.effectDistance = new Vector2(3, -3);
                    outline.effectColor = new Color(0.07f, 0.01f, 0.13f);
                }
                //else 
                //{
                //    t.GetComponent<Outline>().effectDistance = new Vector2(3, -3);
                //}
            }

        }
        Debug.Log("Succed");
    }
    [MenuItem("LG_Tool/一键制作预制物 %#F3")]
    private static void AddPrefabs()
    {
        GameObject [] SeleGameObj = Selection.gameObjects;
        GameObject TempObj;
        foreach (GameObject item in SeleGameObj)
        {
            string DirectoryName = item.name.Split('_') [ 0 ];

            string Prefabpath = "Resources/" + DirectoryName;
            string Scriptpath = "Scripts/" + DirectoryName;
            CreateDirectory(Prefabpath);
            bool isExists = CreateDirectory(Scriptpath);
            TempObj = PrefabUtility.SaveAsPrefabAsset(item, GetPath + Prefabpath + "/" + item.name + ".prefab");
            TempObj.SetActive(true);
            ArrayList lists =  IOperate.Instance.ReadFile(DirectoryName + "Base.cs", "/Framework/Script/Core/View/");
            bool isBaseExists = false;
            for (int i = 0; i < lists.Count; i++)
            {
                if (lists[i].ToString().Contains(item.name))
                {
                    isBaseExists = true;
                    break;
                }
            }
            if (!isBaseExists)
                IOperate.Instance.ReplaceFile(DirectoryName + "Base.cs", "None = 0,", "None = 0,\r\n    " + item.name + ",", "/Framework/Script/Core/View/");
            if (!isExists)
            {
                IOperate.Instance.CreateFile(CreateScripts((UIType)Enum.Parse(typeof(UIType), DirectoryName), item.name), item.name + ".cs", "/" + Scriptpath + "/");
                AssetDatabase.Refresh();
            }

        }
    }

    [MenuItem("LG_Tool/一键空物体重置 %#F4")]
    private static void SetGameObject()
    {
        GameObject [] SeleGameObj = Selection.gameObjects;
        foreach (GameObject item in SeleGameObj)
        {
            item.transform.localPosition = Vector3.zero;
            item.transform.localEulerAngles = Vector3.zero;
            item.transform.localScale = Vector3.one;
        }
    }
    [MenuItem("LG_Tool/一键添加Box碰撞")]
    private static void AddBoxCollider()
    {
        GameObject [] SeleGameObj = Selection.gameObjects;
        foreach (GameObject item in SeleGameObj)
        {
            if (item.GetComponent<BoxCollider>() == null)
            {
                item.AddComponent<BoxCollider>();
                item.GetComponent<BoxCollider>().center = Vector3.zero;
            }
        }
    }
    [MenuItem("LG_Tool/生成子物体包围盒")]
    private static void CreateChildObjBox()
    {
        GameObject [] SeleGameObj = Selection.gameObjects;
        GameObject NewGameObj = new GameObject();
        NewGameObj.name = "ParentBox";
        foreach (GameObject item in SeleGameObj)
        {
            item.transform.parent = NewGameObj.transform;
        }

        Transform parent = NewGameObj.transform;
        Vector3 postion = parent.position;
        Quaternion rotation = parent.rotation;
        Vector3 scale = parent.localScale;
        Collider [] colliders = parent.GetComponentsInChildren<Collider>();
        foreach (Collider child in colliders)
        {
            DestroyImmediate(child);
        }

        Vector3 center = Vector3.zero;
        Renderer [] renders = parent.GetComponentsInChildren<Renderer>();
        foreach (Renderer child in renders)
        {
            center += child.bounds.center;
        }

        center /= parent.childCount;
        Bounds bounds = new Bounds(center, Vector3.zero);
        foreach (Renderer child in renders)
        {
            bounds.Encapsulate(child.bounds);
        }

        BoxCollider boxCollider = parent.gameObject.AddComponent<BoxCollider>();
        boxCollider.center = bounds.center - parent.position;
        boxCollider.size = bounds.size;
        parent.position = postion;
        parent.rotation = rotation;
        parent.localScale = scale;
    }
    [MenuItem("LG_Tool/一键设置物体Tag")]
    private static void SetGameObjTag()
    {
        GameObject [] SeleGameObj = Selection.gameObjects;
        foreach (GameObject item in SeleGameObj)
        {
            AddTag("", item);
        }
    }

    private static void AddTag(string tag, GameObject go)
    {
        if (!isHasTag(tag))
        {
            SerializedObject tagManager = new SerializedObject(go);
            SerializedProperty it = tagManager.GetIterator();
            while (it.NextVisible(true))
            {
                if (it.name == "m_TagString")
                {
                    it.stringValue = tag;
                    tagManager.ApplyModifiedProperties();
                }
            }
        }
    }

    private static bool isHasTag(string tag)
    {
        for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.tags.Length; i++)
        {
            if (UnityEditorInternal.InternalEditorUtility.tags [ i ].Contains(tag))
            {
                return true;
            }
        }
        return false;
    }

    #region 创建脚本

    [MenuItem("Assets/Create/创建Panel", false, 1)]
    public static void CreatePanel()
    {
        IOperate.Instance.CreateFile(CreateScripts(UIType.Panel, "Panel_Script"), "Panel_Script.cs", "/Scripts/"+ UIType.Panel+"/");
        AssetDatabase.Refresh();

    }
    [MenuItem("Assets/Create/创建Scene", false, 1)]
    public static void CreateSCene()
    {

        IOperate.Instance.CreateFile(CreateScripts(UIType.Scene, "Scene_Script"),  "Scene_Script.cs", "/Scripts/"+ UIType.Scene+"/");
        AssetDatabase.Refresh();

    }

    private static string CreateScripts() {
        StringBuilder builder = new StringBuilder();
        AddTitle(builder);
        return builder.ToString();
    }

    private static string CreateScripts(UIType type, string name)
    {
        StringBuilder builder = new StringBuilder();
        AddTitle(builder);
        builder.Append("using System;\r\n");
        builder.Append("using System.Collections;\r\n");
        builder.Append("using System.Collections.Generic;\r\n");
        builder.Append("using UnityEngine;\r\n");
        builder.Append("using UnityEngine.UI;\r\n");
        builder.Append("using DG.Tweening;\r\n");
        builder.Append("\r\n");
        builder.Append("public class " + name + "  : " + type + "Base\r\n");
        builder.Append("{\r\n");
        builder.Append("    #region 界面加载\r\n");
        builder.Append("\r\n");
        builder.Append("    protected override void OnInitFront()\r\n");
        builder.Append("    {\r\n");
        builder.Append("        base.OnInitFront();\r\n");
        builder.Append("        _type=" + type + "Name." + name + ";\r\n");
        AddInitFront(builder, type);
        builder.Append("    }\r\n");
        builder.Append("    protected override void OnInitSkinFront()\r\n");
        builder.Append("    {\r\n");
        builder.Append("        base.OnInitSkinFront();\r\n");
        builder.Append("        SetMainSkinPath(\"" + type + "/" + name + "\");\r\n");
        builder.Append("    }\r\n");
        builder.Append("    public override void OnInit(params object[] " + type.ToString().ToLower() + "Args)\r\n");
        builder.Append("    {\r\n");
        builder.Append("        base.OnInit(" + type.ToString().ToLower() + "Args);\r\n");
        builder.Append("        InitData();\r\n");
        builder.Append("    }\r\n");
        builder.Append("    #endregion\r\n");
        builder.Append("\r\n");
        builder.Append("    #region 数据定义\r\n");
        builder.Append("\r\n");
        builder.Append("    #endregion\r\n");
        builder.Append("\r\n");
        builder.Append("    #region 逻辑\r\n");
        builder.Append("    /// <summary>初始化</summary>\r\n");
        builder.Append("    private void InitData()\r\n");
        builder.Append("    {\r\n");
        builder.Append("        if (" + type.ToString().ToLower() + "Args.Length!=0)\r\n");
        builder.Append("        {\r\n");
        builder.Append("        }\r\n");
        builder.Append("        FindObj();\r\n");
        builder.Append("        AddEvent();\r\n");
        builder.Append("    }\r\n");
        builder.Append("\r\n");
        builder.Append("    /// <summary>查找物体</summary>\r\n");
        builder.Append("    private void FindObj()\r\n");
        builder.Append("    {\r\n");
        builder.Append("    }\r\n");
        builder.Append("\r\n");
        builder.Append("    /// <summary>添加事件</summary>\r\n");
        builder.Append("    private void AddEvent()\r\n");
        builder.Append("    {\r\n");
        builder.Append("    }\r\n");
        builder.Append("\r\n");
        builder.Append("    /// <summary>按钮点击事件</summary>\r\n");
        builder.Append("    protected override void OnClick(Transform _target)\r\n");
        builder.Append("    {\r\n");
        builder.Append("        switch (_target.name)\r\n");
        builder.Append("        {\r\n");
        AddClick(builder, type);
        builder.Append("        }\r\n");
        builder.Append("    }\r\n");
        builder.Append("    #endregion\r\n");
        builder.Append("}\r\n");
        return builder.ToString();
    }

    private static void AddTitle(StringBuilder builder)
    {
        builder.Append("//-------------------------------------------\r\n");
        builder.Append("//作者：马超\r\n");
        builder.Append("//时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "\r\n");
        builder.Append("//作用：\r\n");
        builder.Append("//-------------------------------------------\r\n");
    }

    private static void AddInitFront(StringBuilder builder, UIType type)
    {
        switch (type)
        {
            case UIType.Panel:
                builder.Append("        _openDuration=0.5f;\r\n");
                builder.Append("        _alpha=0f;\r\n");
                builder.Append("        _showStyle=PanelMgr.PanelShowStyle.Nomal;//修改打开风格\r\n");
                builder.Append("        _maskStyle=PanelMgr.PanelMaskSytle.None;//修改遮罩方式\r\n");
                builder.Append("        _cache=false;\r\n");
                break;
        }
    }

    private static void AddClick(StringBuilder builder, UIType type)
    {
        switch (type)
        {
            case UIType.Panel:
                builder.Append("            case \"Btn_Close\":\r\n");
                builder.Append("                Close();\r\n");
                builder.Append("                break;\r\n");
                break;
        }
    }

    private enum UIType
    {
        Panel,
        Scene
    }

    #endregion

}






















































