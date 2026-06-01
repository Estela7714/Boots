using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections; // Importante para o IEnumerator funcionar

// O ENUM PRECISA FICAR FORA DA CLASSE (AQUI NO TOPO)
// Se ele estiver dentro de chaves erradas, dá erro em todo o projeto.
public enum GameState 
{ 
    Iniciando, 
    MenuPrincipal, 
    Gameplay 
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private GameState currentState;

    private void Awake()
    {
        if (Instance == null) 
        { 
            Instance = this; 
            DontDestroyOnLoad(gameObject); 
        }
        else 
        { 
            Destroy(gameObject); 
        }
    }

    private void Start()
    {
        ProcessarMudancaDeFase("Splash", GameState.Iniciando);
    }

    private void ProcessarMudancaDeFase(string nomeCena, GameState novoEstado)
    {
        currentState = novoEstado;
        Debug.Log("[GM] Mudando para Cena: " + nomeCena + " | Estado: " + novoEstado.ToString());
        SceneManager.LoadScene(nomeCena);
    }

    // --- SOLICITAÇÕES EXTERNAS ---

    public void SolicitarIrParaMenu()
    {
        ProcessarMudancaDeFase("MenuPrincipal", GameState.MenuPrincipal);
    }

    public void SolicitarIniciarJogo()
    {
        // Chama a corrotina usando o nome exato da função abaixo
        StartCoroutine(CarregarJogoEInterface());
    }

    // Corrotina para carregar a cena e depois a interface de moedas
    private IEnumerator CarregarJogoEInterface()
    {
        currentState = GameState.Gameplay;
        Debug.Log("[GM] Mudando para Estado: " + currentState.ToString());
        
        SceneManager.LoadScene("SampleScene");

        // Espera 1 frame para a SampleScene carregar antes de puxar a GUI
        yield return null; 

        SceneManager.LoadScene("GUI", LoadSceneMode.Additive);
        Debug.Log("[GM] Interface (GUI) carregada em modo Aditivo.");
    }

    public void AssignPlayerInput(PlayerInput input)
    {
        Debug.Log("Input alocado pelo GM.");
    }
}