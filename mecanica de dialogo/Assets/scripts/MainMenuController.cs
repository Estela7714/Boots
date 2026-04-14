using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button btnIniciar;
    [SerializeField] private Button btnSair;

    private void Start()
    {
        if (btnIniciar != null)
            btnIniciar.onClick.AddListener(PedirParaIniciar);

        if (btnSair != null)
            btnSair.onClick.AddListener(Sair);
    }

    private void PedirParaIniciar()
    {
        // O Menu não sabe qual é a cena ou o estado, ele só pede para "Iniciar"
        GameManager.Instance.SolicitarIniciarJogo();
    }

    private void Sair()
    {
        Application.Quit();
    }
}