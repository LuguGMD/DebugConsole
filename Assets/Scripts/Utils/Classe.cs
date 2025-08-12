using UnityEngine;
using Lugu.Console;

public class Classe : MonoBehaviour
{
    [DebugMethod("teste")]
    public static void Teste()
    {
        Debug.Log("Esse é um teste");
    }

    [DebugMethod("teste2")]
    public void Teste2()
    {
        Debug.Log("Esse é um teste");
    }
}
