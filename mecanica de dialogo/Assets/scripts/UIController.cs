using UnityEngine;
using TMPro; // Use 'using UnityEngine.UI;' se estiver usando o texto antigo (Legacy)

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoMoedas; // Altere para 'Text' se for o antigo

    private void OnEnable()
    {
        // INSCRIÇÃO: Começa a ouvir o canal de moedas
        PlayerObserverManager.OnMoedaColetada += AtualizarTextoMoedas;
    }

    private void OnDisable()
    {
        // DESINSCRIÇÃO: Boa prática para evitar erros de memória ao fechar o jogo
        PlayerObserverManager.OnMoedaColetada -= AtualizarTextoMoedas;
    }

    // Essa função roda automaticamente sempre que o evento acontece
    private void AtualizarTextoMoedas(int totalMoedas)
    {
        textoMoedas.text = "Moedas: " + totalMoedas;
    }
}