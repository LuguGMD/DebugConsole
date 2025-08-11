using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method)]
public class DebugMethodAttribute : Attribute
{
    public string commandID;
    public string commandDescription;
    public string commandFormat;
    public object[] parameters;

    public DebugMethodAttribute(string commandID, string commandDescription, string commandFormat, params object[] parameters)
    {
        this.commandID = commandID;
        this.commandDescription = commandDescription;
        this.commandFormat = commandFormat;
        this.parameters = parameters;
    }
}
