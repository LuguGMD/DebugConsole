using UnityEngine;
using Lugu.Console;

public class Test : MonoBehaviour
{
    [DebugMethod("Funcao1")]
    public void Funcao1()
    {
        Debug.Log("Isso � um teste rodado de: " + gameObject.name);
    }
}
