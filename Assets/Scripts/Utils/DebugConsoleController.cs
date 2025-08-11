using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class DebugConsoleController : MonoBehaviour
{
    private List<DebugCommandBase> m_commandList = new List<DebugCommandBase>();
    private List<DebugCommandInfo> m_expoxedCommands = new List<DebugCommandInfo>();

    [SerializeField] private bool m_showConsole = false;
    private string m_input;

    private void Start()
    {
        GetDebugCommands();
    }

    private void OnGUI()
    {
        if (!m_showConsole) return;

        float y = 0f;

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0,0,0,0);
        m_input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), m_input);
    }

    [ContextMenu("HandleInput")]
    private void HandleInput()
    {
        for (int i = 0; i < m_commandList.Count; i++)
        {
            DebugCommandBase commandBase = m_commandList[i];

            if(m_input.Contains(commandBase.commandID))
            {
                if (m_commandList[i] as DebugCommand != null)
                {
                    (m_commandList[i] as DebugCommand).Invoke();
                }
            }
        }
    }

    private void OnToggleDebug()
    {
        m_showConsole = !m_showConsole;
    }

    [ContextMenu("ReturnInput")]
    private void ReturnInput()
    {
        if (m_showConsole)
        {
            HandleInput();
            m_input = "";
        }
    }

    private void GetDebugCommands()
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (Assembly assembly in assemblies)
        {
            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
            {
                BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                MethodInfo[] members = type.GetMethods(flags);

                foreach (MethodInfo member in members)
                {

                    DebugMethodAttribute attribute = member.GetCustomAttribute<DebugMethodAttribute>();

                    if (attribute != null)
                    {
                        m_expoxedCommands.Add(new DebugCommandInfo(member, attribute));
                    }

                }
            }
        }

        RegisterCommands();
    }

    private void RegisterCommands()
    {
        foreach (DebugCommandInfo command in m_expoxedCommands)
        {
            DebugMethodAttribute attribute = command.debugMethodAttribute;

            DebugCommand debugCommand = new DebugCommand(
                attribute.commandID, 
                attribute.commandDescription, 
                attribute.commandFormat,
                command.methodInfo);

            m_commandList.Add(debugCommand);

        }
    }

    public struct DebugCommandInfo
    {
        public MethodInfo methodInfo;
        public DebugMethodAttribute debugMethodAttribute;

        public DebugCommandInfo(MethodInfo info, DebugMethodAttribute attribute)
        {
            methodInfo = info;
            debugMethodAttribute = attribute;
        }
    }
}
