using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public enum GameState { Iniciando, MenuPrincipal, Gameplay }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameState currentState;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        // O próprio GM decide começar assim
        ProcessarMudancaDeFase("Splash", GameState.Iniciando);
    }

    // Centralizamos a mudança aqui. Ninguém mais toca no SceneManager ou no State.
    private void ProcessarMudancaDeFase(string nomeCena, GameState novoEstado)
    {
        currentState = novoEstado;
        Debug.Log($"<color=orange>[GM]</color> Mudando para Cena: <b>{nomeCena}</b> | Estado: <b>{novoEstado}</b>");
        SceneManager.LoadScene(nomeCena);
    }

    // --- SOLICITAÇÕES EXTERNAS ---

    public void SolicitarIrParaMenu()
    {
        ProcessarMudancaDeFase("MenuPrincipal", GameState.MenuPrincipal);
    }

    public void SolicitarIniciarJogo()
    {
        ProcessarMudancaDeFase("SampleScene", GameState.Gameplay);
    }

    public void AssignPlayerInput(PlayerInput input)
    {
        Debug.Log("Input alocado pelo GM.");
    }
}