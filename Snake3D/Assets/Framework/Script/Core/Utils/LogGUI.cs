//#define USE_TESTCONSOLE
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A console to display Unity's debug logs in-game.
/// </summary>
class LogGUI : MonoBehaviour
{
#if DEBUG_LOG
    struct Log
    {
        public string message;
        public string stackTrace;
        public LogType type;
    }

    #region Inspector Settings


    public float shakeAcceleration = 3f;

    public bool restrictLogCount = false;

    public int maxLogs = 1000;

    #endregion

    readonly List<Log> logs = new List<Log>( );
    Vector2 scrollPosition;
    bool visible = true;
    bool collapse;

    // Visual elements:
    static readonly Dictionary<LogType , Color> logTypeColors = new Dictionary<LogType , Color>
        {
            { LogType.Assert, Color.white },
            { LogType.Error, Color.red },
            { LogType.Exception, Color.red },
            { LogType.Log, Color.white },
            { LogType.Warning, Color.yellow },
        };

    const string windowTitle = "Console";
    const int margin = 20;
    static readonly GUIContent clearLabel = new GUIContent( "Clear" , "Clear the contents of the console." );
    static readonly GUIContent collapseLabel = new GUIContent( "Collapse" , "Hide repeated messages." );

    readonly Rect titleBarRect = new Rect( 0 , 0 , 1000 , 20 );
    Rect windowRect = new Rect( margin , margin , Screen.width - ( margin * 2 ) , Screen.height / 2 );

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void OnGUI()
    {
        windowRect = GUILayout.Window( 123456 , windowRect , DrawConsoleWindow , windowTitle );
    }

    void DrawConsoleWindow(int windowID)
    {
        DrawLogsList( );
        DrawToolbar( );

        GUI.DragWindow( titleBarRect );
    }

    private GUIStyle GetGUIStyle
    {
        get
        {
            GUIStyle gUIStyle = new GUIStyle( GUI.skin.label );
            gUIStyle.fontSize = 35;
            return gUIStyle;
        }
    }

    void DrawLogsList()
    {
        scrollPosition = GUILayout.BeginScrollView( scrollPosition );

        // Iterate through the recorded logs.
        for ( var i = 0; i < logs.Count; i++ )
        {
            var log = logs [ i ];

            // Combine identical messages if collapse option is chosen.
            if ( collapse && i > 0 )
            {
                var previousMessage = logs [ i - 1 ].message;

                if ( log.message == previousMessage )
                {
                    continue;
                }
            }

            GUI.contentColor = logTypeColors [ log.type ];
            GUILayout.Label( log.message , GetGUIStyle );
        }

        GUILayout.EndScrollView( );

        GUI.contentColor = Color.white;
    }

    void DrawToolbar()
    {
        GUILayout.BeginHorizontal( );

        if ( GUILayout.Button( clearLabel ) )
        {
            logs.Clear( );
        }

        collapse = GUILayout.Toggle( collapse , collapseLabel , GUILayout.ExpandWidth( false ) );

        GUILayout.EndHorizontal( );
    }

    /// <summary>
    /// Records a log from the log callback.
    /// </summary>
    /// <param name="message">Message.</param>
    /// <param name="stackTrace">Trace of where the message came from.</param>
    /// <param name="type">Type of message (error, exception, warning, assert).</param>
    void HandleLog(string message , string stackTrace , LogType type)
    {
        logs.Add( new Log
        {
            message = message ,
            stackTrace = stackTrace ,
            type = type ,
        } );

        TrimExcessLogs( );
    }

    /// <summary>
    /// Removes old logs that exceed the maximum number allowed.
    /// </summary>
    void TrimExcessLogs()
    {
        if ( !restrictLogCount )
        {
            return;
        }

        var amountToRemove = Mathf.Max( logs.Count - maxLogs , 0 );

        if ( amountToRemove == 0 )
        {
            return;
        }

        logs.RemoveRange( 0 , amountToRemove );
    }
#endif
}