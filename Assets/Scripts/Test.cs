using UnityEngine;
using Lugu.Console;
using Lugu.Utils.Debug;

public class Test : MonoBehaviour
{
    [DebugMethod("Funcao1")]
    public void Funcao1()
    {
        Debug.Log("Isso é um teste rodado de: " + gameObject.name);
    }

    private void Awake()
    {
        DebugLogger.Log("A", "Inimigo");
        DebugLogger.LogWarning("B", "Combate");
        DebugLogger.LogError("C", "Teste");
    }
}
