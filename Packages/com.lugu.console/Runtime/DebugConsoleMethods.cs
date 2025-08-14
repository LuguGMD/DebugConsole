using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lugu.Console
{
    public class DebugConsoleMethods : MonoBehaviour
    {
        [DebugMethod("TIME_SCALE", "Changes the Time Scale of the game")]
        public static void SetTimeScale(float timeScale)
        {
            Time.timeScale = timeScale;
        }

        [DebugMethod("HELP", "Show all available static methods")]
        public static void ConsoleHelp()
        {
            List<string> commandsStrings= new List<string>();

            List<DebugCommandBase> commandsList = DebugConsoleController.commandList;

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
                commandsStrings.Add(text);
            }

            DebugConsoleController.extraText = commandsStrings.ToArray(); 
        }

        [DebugMethod("HELP_SEARCH", "Shows the list of classes that can be searched with the Search method")]
        public static void SearchHelp()
        {
            List<string> classesStrings = new List<string>();

            List<Type> classesList = new List<Type>(DebugConsoleController.classCommands.Keys);

            classesStrings.Add("CLASSES TO BE SEARCHED: ");
            classesStrings.Add("");

            foreach (Type type in classesList)
            {
                classesStrings.Add(type.Name);
            }

            DebugConsoleController.extraText = classesStrings.ToArray();
        }

        [DebugMethod("SEARCH", "Search for an instance type")]
        public static void Search(string typeName)
        {
            List<Type> classesList = new List<Type>(DebugConsoleController.classCommands.Keys);
            Type selectedType = null;

            foreach (Type type in classesList)
            {
                if (type.Name == typeName)
                {
                    selectedType = type;
                }
            }

            if(selectedType == null)
            {
                DebugConsoleController.extraText = new string[] { "This is not a valid class" };
                return;
            }

            List<string> objectsStrings = new List<string>();
            UnityEngine.Object[] objects = GameObject.FindObjectsByType(selectedType, FindObjectsSortMode.InstanceID);

            if (objects.Length == 0)
            {
                DebugConsoleController.ErrorMessage("NO OBJECTS FOUND");
                return;
            }
            else
            { 
                objectsStrings.Add("AVAILABLE OBJECTS: ");
                objectsStrings.Add("");

                DebugConsoleController.selectionList = new DebugConsoleController.SelectObjectInfo[objects.Length];

                for (int i = 0; i < objects.Length; i++)
                {
                    objectsStrings.Add($"{i} - {objects[i].name}");
                    DebugConsoleController.selectionList[i].objectValue = objects[i];
                    DebugConsoleController.selectionList[i].name = objects[i].name;
                }
                
            }

            DebugConsoleController.extraText = objectsStrings.ToArray();

            
        }

        [DebugMethod("CHOOSE", "Chooses an instance from the search list")]
        public static void Choose(int index)
        {
            if(DebugConsoleController.selectionList == null)
            {
                DebugConsoleController.ErrorMessage("Search an Object first");
            }
            if(DebugConsoleController.selectionList.Length == 0)
            {
                DebugConsoleController.ErrorMessage("Search an Object first");
            }

            DebugConsoleController.SelectObjectInfo selectObjectInfo = DebugConsoleController.selectionList[index];
            DebugConsoleController.SelectObject(selectObjectInfo);
        }

        [DebugMethod("LOAD_SCENE", "Loads a scene using the build index")]
        public static void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }
        [DebugMethod("DESTROY", "Destroys the selected object")]
        public static void Destroy()
        {

        }

    }
}
