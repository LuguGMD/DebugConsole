using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Lugu.Console
{
    public class DebugConsoleController : MonoBehaviour
    {
        private List<DebugCommandBase> m_commandList = new List<DebugCommandBase>(); //List of static commands
        private Dictionary<Type, List<DebugCommandBase>> m_classCommands = new Dictionary<Type, List<DebugCommandBase>>();
        private List<DebugCommandInfo> m_expoxedCommands = new List<DebugCommandInfo>();

        [SerializeField] private bool m_showConsole = false;
        private string m_input = "";

        [SerializeField] InputActionReference m_consoleInput;
        [SerializeField] InputActionReference m_confirmInput;

        private static object m_selectedObject;

        #region Properties

        public static object selectedObject
        {
            get { return m_selectedObject; }
            set { m_selectedObject = value; }
        }

        #endregion

        private void Start()
        {
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

            GUI.Box(new Rect(0, y, Screen.width, 30), "");
            GUI.backgroundColor = new Color(0, 0, 0, 0);
            m_input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), m_input);

        }

        #region Handling Console Input

        private void HandleInput()
        {
            if (m_selectedObject == null)
            {
                HandleStaticMethod();
            }
            else
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

                if (parameters[0].Contains(commandBase.commandID))
                {
                    if (m_commandList[i] as DebugCommand != null)
                    {
                        object[] parametersObjects = HandleParameters(m_commandList[i] as DebugCommand, parameters);
                        if (parameters.Length > 1 && parametersObjects == null)
                            return;

                        (m_commandList[i] as DebugCommand).Invoke(this, parametersObjects);
                    }
                }
            }
        }

        private void HandleMethod()
        {
            Type type = m_selectedObject.GetType();

            if (!m_classCommands.ContainsKey(type)) return;

            for (int i = 0; i < m_classCommands[type].Count; ++i)
            {
                DebugCommandBase commandBase = m_classCommands[type][i];

                string[] parameters = m_input.Split(" ");

                if (parameters[0].Contains(commandBase.commandID))
                {
                    if (m_commandList[i] as DebugCommand != null)
                    {
                        object[] parametersObjects = HandleParameters(m_commandList[i] as DebugCommand, parameters);
                        if (parameters.Length > 1 && parametersObjects == null)
                            return;

                        (m_commandList[i] as DebugCommand).Invoke(m_selectedObject, parametersObjects);
                    }
                }
            }

        }

        private object[] HandleParameters(DebugCommand debug, string[] parameters)
        {
            List<object> parametersObjs = new List<object>();

            if(parameters.Length-1 != debug.parameters.Length)
            {
                return null;
            }

            for (int i = 1; i < parameters.Length; i++)
            {
                string parameterName = debug.parameters[i-1].ToString();
                switch (parameterName)
                {
                    case "System.String":
                        parametersObjs.Add(parameters[i]);
                        break;
                    case "System.Int32":
                        parametersObjs.Add(int.Parse(parameters[i]));
                        break;
                    case "System.Single":
                        parametersObjs.Add(float.Parse(parameters[i]));
                        break;
                    case "System.Double":
                        parametersObjs.Add(double.Parse(parameters[i]));
                        break;
                    case "System.Char":
                        parametersObjs.Add(char.Parse(parameters[i]));
                        break;
                    case "System.Boolean":
                        parametersObjs.Add(bool.Parse(parameters[i]));
                        break;
                    default:
                        Debug.LogWarning($"{debug.commandID} has unsupported parameter: {parameterName}");
                        break;
                }
            }

            if (parametersObjs.Count != debug.parameters.Length)
            {
                Debug.LogWarning($"parameters don't match {debug.commandID}");
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

                if(!m_showConsole)
                    m_selectedObject = null;
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

        /*private void SelectObject(object obj)
        {
            if(m_classCommands.ContainsKey(obj))
            {
                m_classCommands = obj;
            }
        }*/

        #endregion

        #region Starting commands

        private void GetDebugCommands()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

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

                Debug.Log(attribute.commandID);

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

        #endregion
    }
}