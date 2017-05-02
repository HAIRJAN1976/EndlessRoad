using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DebugBox : MonoBehaviour
{
    public int leftOffset = 40;
    public int topOffset = 20;

    float xTextPrintOffset = 3.0f;
    Vector2 scrollPosition = Vector2.zero;

    //change windowRect for initial position and size.
    Rect windowRect = new Rect(20, 20, 300, 400);
    int windowId = 1;

    //list that contains the info to be printed to the window
    public List<PrintInfo> printList = new List<PrintInfo>();

    public bool DebugMode = true;

    private List<string> conditions = new List<string>();
    private List<LogType> logTypes = new List<LogType>();

    void Awake()
    {
        if (DebugMode)
        {
            Application.logMessageReceived += LogCallBackHandler;
        }
    }

    /*-----------------------*
     * Application Method
     *-----------------------*/
    //Debug.Logの情報を保存する
    void LogCallBackHandler(string condition, string stackTrace, LogType type)
    {
        if (DebugMode)
        {
            // 取得したログの情報を処理する
            conditions.Add(condition);
            logTypes.Add(type);

            switch (type)
            {
                case LogType.Error:
                    printList.Add(new PrintInfo(condition, type, true));
                    break;
                case LogType.Assert:
                    break;
                case LogType.Warning:
                    break;
                case LogType.Log:
                    printList.Add(new PrintInfo(condition, type, false));
                    break;
                case LogType.Exception:
                    break;
                default:
                    break;
            }
        }
    }

    //printInfo tells the output to be boxed or labeled
    public class PrintInfo
    {
        public bool boxBool;
        private string buffer;
        private LogType logType = LogType.Log;

        public PrintInfo(string buffer)
        {
            this.Buffer = buffer;
            boxBool = false;
        }

        public PrintInfo(string buffer, LogType logType, bool boxBool)
        {
            this.Buffer = buffer;
            this.logType = logType;
            this.boxBool = boxBool;
        }

        public string Buffer
        {
            get
            {
                return buffer;
            }

            set
            {
                buffer = value;
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        windowRect = new Rect(leftOffset, topOffset, Screen.width - leftOffset * 2, Screen.height / 3 - topOffset);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //the gui crap
    void OnGUI()
    {
        if (DebugMode)
        {
            windowRect = GUI.Window(windowId, windowRect, DebugWindow, "");
        }
    }

    void DebugWindow(int windowId)
    {

        GUILayout.BeginArea(new Rect(xTextPrintOffset, 0, windowRect.width, windowRect.height));

        GUILayout.Space(2);
        GUILayout.Box("Debug Window");

        if (GUILayout.Button("Close"))
        {
            printList.Clear();
            GameObject.Destroy(this);
        }
        if (GUILayout.Button("Clear"))
            printList.Clear();

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(windowRect.width - 10), GUILayout.Height(windowRect.height - 80));

        for (int i = 0; i < printList.Count; i++)
        {
            var t = printList[i];
            if (t.boxBool)
                GUILayout.Box(t.Buffer);
            else
                GUILayout.Label(t.Buffer);
        }

        GUILayout.EndScrollView();

        GUILayout.EndArea();

        GUI.DragWindow();
    }


}

