using UnityEngine;

public class Classe : MonoBehaviour
{
    [DebugMethod("teste", "descricao", "teste")]
    public void Teste()
    {
        Debug.Log("Esse é um teste");
    }
}
