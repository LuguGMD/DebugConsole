using Codice.CM.Common;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace Lugu.Console
{
    public class DebugConsoleController : MonoBehaviour
    {
        private static List<DebugCommandBase> m_commandList = new List<DebugCommandBase>(); //List of static commands
        private static Dictionary<Type, List<DebugCommandBase>> m_classCommands = new Dictionary<Type, List<DebugCommandBase>>();
        private static List<DebugCommandInfo> m_expoxedCommands;

        private static bool m_showConsole = false;
        private static bool m_showExtraPanel = false;
        private static string m_input = "";
        private static bool m_isLoaded = false;

        private static string[] m_extraText = new string[0];

        [SerializeField] InputActionReference m_consoleInput;
        [SerializeField] InputActionReference m_confirmInput;

        private static SelectObjectInfo[] m_selectionList;
        private static SelectObjectInfo m_selectedObject = new SelectObjectInfo(null, null);

        #region Extra Panel
        private static Vector2 m_scroll;

        private const int EXTRA_PANEL_HEIGHT = 100;
        private const int EXTRA_PANEL_LABEL_HEIGHT = 20;

        #endregion

        #region Properties

        public static SelectObjectInfo[] selectionList
        {
            get { return m_selectionList; }
            set { m_selectionList = value; }
        }

        public static SelectObjectInfo selectedObject
        {
            get { return m_selectedObject; }
            set { m_selectedObject = value; }
        }

        public static List<DebugCommandBase> commandList
        {
            get { return m_commandList; }
            private set { m_commandList = value; }
        }

        public static Dictionary<Type, List<DebugCommandBase>> classCommands
        {
            get { return m_classCommands; }
            private set { m_classCommands = value; }
        }

        public static bool showConsole
        {
            get { return m_showConsole; }
            private set { m_showConsole = value; }
        }

        public static bool showExtraPanel
        {
            get { return m_showExtraPanel; }
            private set { m_showExtraPanel = value; }
        }

        public static string input
        {
            get { return m_input; }
            private set { m_input = value; }
        }

        public static string[] extraText
        {
            get { return m_extraText; }
            set
            {
                m_extraText = value;
                m_showExtraPanel = true;
            }
        }

        #endregion

        private void Start()
        {
            if(m_expoxedCommands == null)
                GetDebugCommands();
        }

        private void OnEnable()
        {
            if (m_consoleInput != null)
            {
                m_consoleInput.ToInputAction().performed += OnToggleDebug;
            }

            if (m_confirmInput != null)
            {
                m_confirmInput.ToInputAction().performed += ReturnInput;
            }
        }

        private void OnDisable()
        {
            if (m_consoleInput != null)
            {
                m_consoleInput.ToInputAction().performed -= OnToggleDebug;
            }

            if (m_confirmInput != null)
            {
                m_confirmInput.ToInputAction().performed -= ReturnInput;
            }
        }

        private void OnGUI()
        {
            if (!m_showConsole) return;

            float y = 0f;

            if (m_showExtraPanel)
            {
                GUI.Box(new Rect(0, y, Screen.width, EXTRA_PANEL_HEIGHT), "");

                Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * m_extraText.Length);

                m_scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), m_scroll, viewport);

                for (int i = 0; i < m_extraText.Length; i++)
                {
                    string label = m_extraText[i];

                    Rect labelRect = new Rect(5, EXTRA_PANEL_LABEL_HEIGHT * i, viewport.width - 100, EXTRA_PANEL_LABEL_HEIGHT);

                    GUI.Label(labelRect, label);
                }

                GUI.EndScrollView();

                y += EXTRA_PANEL_HEIGHT;
            }

            GUI.Box(new Rect(0, y, Screen.width, 30), "");
            GUI.backgroundColor = new Color(0, 0, 0, 0);

            m_input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), m_input);

            if (!m_isLoaded)
            {
                m_input = "LOADING...";
            }

        }

        public static void ErrorMessage(params string[] text)
        {
            extraText = text;
        }

        #region Handling Console Input

        private void HandleInput()
        {
            if (!m_isLoaded) return;

            HandleStaticMethod();

            if (m_selectedObject.objectValue != null)
            {
                HandleMethod();
            }
        }

        private void HandleStaticMethod()
        {
            for (int i = 0; i < m_commandList.Count; i++)
            {
                DebugCommandBase commandBase = m_commandList[i];

                string[] parameters = m_input.Split(" ");

                if (parameters[0] == commandBase.commandID)
                {
                    if (commandBase as DebugCommand != null)
                    {
                        object[] parametersObjects = HandleParameters(commandBase as DebugCommand, parameters);
                        if ((parameters.Length > 1 || (commandBase as DebugCommand).parameters.Length >= 1) && parametersObjects == null)
                            return;
                        

                        (commandBase as DebugCommand).Invoke(this, parametersObjects);
                    }
                }
            }
        }

        private void HandleMethod()
        {
            Type type = m_selectedObject.objectValue.GetType();

            if (!m_classCommands.ContainsKey(type)) return;

            for (int i = 0; i < m_classCommands[type].Count; ++i)
            {
                DebugCommandBase commandBase = m_classCommands[type][i];

                string[] parameters = m_input.Split(" ");

                if (parameters[0] == commandBase.commandID)
                {
                    if (commandBase as DebugCommand != null)
                    {
                        object[] parametersObjects = HandleParameters(commandBase as DebugCommand, parameters);
                        if (parameters.Length > 1 && parametersObjects == null)
                            return;

                        (commandBase as DebugCommand).Invoke(m_selectedObject.objectValue, parametersObjects);
                    }
                }
            }

        }

        private object[] HandleParameters(DebugCommand debug, string[] parameters)
        {
            List<object> parametersObjs = new List<object>();

            if(parameters.Length-1 != debug.parameters.Length)
            {
                ErrorMessage($"parameters don't match {debug.commandID}");
                return null;
            }

            for (int i = 1; i < parameters.Length; i++)
            {
                bool canParse = true;

                string parameterName = debug.parameters[i-1].ToString();
                parameterName = parameterName.Replace("System.", "");
                switch (parameterName)
                {
                    case "String":
                        parametersObjs.Add(parameters[i]);
                        break;
                    case "Int32":
                        canParse = int.TryParse(parameters[i], out int valueInt);
                        if (canParse) parametersObjs.Add(valueInt);
                        break;
                    case "Single":
                        canParse = float.TryParse(parameters[i], out float valueFloat);
                        if (canParse) parametersObjs.Add(valueFloat);
                        break;
                    case "Boolean":
                        canParse = bool.TryParse(parameters[i], out bool valueBool);
                        if (canParse) parametersObjs.Add(valueBool);
                        break;
                    default:
                        ErrorMessage($"{debug.commandID} has unsupported parameter: {parameterName}");
                        break;
                }

                if(!canParse)
                {
                    ErrorMessage($"invalid parameter in {debug.commandID}: {parameterName}");
                }

            }

            if (parametersObjs.Count != debug.parameters.Length)
            {
                ErrorMessage($"parameters don't match {debug.commandID}");
                return null;
            }

            return parametersObjs.ToArray();
        }

        #endregion

        #region User Inputs

        private void OnToggleDebug(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                m_showConsole = !m_showConsole;

                if (!m_showConsole)
                {
                    m_selectedObject.objectValue = null;
                }
                else if(m_expoxedCommands == null)
                {
                    GetDebugCommands();
                }
            }
        }

        private void ReturnInput(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                if (m_showConsole)
                {
                    HandleInput();
                    m_input = "";
                }
            }
        }

        public static void SelectObject(DebugConsoleController.SelectObjectInfo selectedObjectInfo)
        {
            object obj = selectedObjectInfo.objectValue;
            if (m_classCommands.ContainsKey(obj.GetType()))
            {
                m_selectedObject.name = selectedObjectInfo.name;
                m_selectedObject.objectValue = obj;

                List<string> objectInfo = new List<string>();

                List<DebugCommandBase> commandsList = m_classCommands[obj.GetType()];

                objectInfo.Add($"{m_selectedObject.name} COMMANDS:");
                objectInfo.Add("");

                foreach (DebugCommandBase command in commandsList)
                {
                    string text = $"{command.commandID} ";
                    if (command is DebugCommand)
                    {
                        foreach (Type type in (command as DebugCommand).parameters)
                        {
                            text += $"/{type.ToString().Replace("System.", "")}/ ";
                        }
                    }
                    text += $" - {command.commandDescription}";
                    objectInfo.Add(text);
                }

                extraText = objectInfo.ToArray();

            }
        }

        #endregion

        #region Starting commands

        private async void GetDebugCommands()
        {
            m_expoxedCommands = new List<DebugCommandInfo>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            await Task.Run(() =>
            {
                foreach (Assembly assembly in assemblies)
                {
                    Type[] types = assembly.GetTypes();

                    foreach (Type type in types)
                    {
                        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
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
            }
            );
            RegisterCommands();
        }

        private void RegisterCommands()
        {
            foreach (DebugCommandInfo command in m_expoxedCommands)
            {
                DebugMethodAttribute attribute = command.debugMethodAttribute;

                ParameterInfo[] parameters = command.methodInfo.GetParameters();
                Type[] types = new Type[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    types[i] = parameters[i].ParameterType;
                }

                DebugCommand debugCommand = new DebugCommand(
                    attribute.commandID,
                    attribute.commandDescription,
                    command.methodInfo,
                    types
                    );



                if (command.methodInfo.IsStatic)
                {
                    m_commandList.Add(debugCommand);
                }
                else
                {
                    Type type = command.methodInfo.ReflectedType;
                    if (!m_classCommands.ContainsKey(type))
                    {
                        m_classCommands.Add(type, new List<DebugCommandBase>());
                    }

                    m_classCommands[type].Add(debugCommand);
                }

            }

            m_isLoaded = true;
            m_input = "";
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

        public struct SelectObjectInfo
        {
            public string name;
            public object objectValue;

            public SelectObjectInfo(string name, object objectValue)
            {
                this.name = name;
                this.objectValue = objectValue;
            }
        }

        #endregion
    }
}