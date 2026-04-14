using UnityEngine;
using System.Collections;

public class SplashController : MonoBehaviour
{
    IEnumerator Start()
    {
        // Espera o tempo do Splash
        yield return new WaitForSeconds(2f);

        if (GameManager.Instance != null)
        {
            // Apenas solicita, o GM faz o resto
            GameManager.Instance.SolicitarIrParaMenu();
        }
    }
}