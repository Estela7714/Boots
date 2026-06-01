using UnityEngine;

public class Moeda : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private int valorDaMoeda = 1; // Deixe 1 aqui
    [SerializeField] private float velocidadeRotacao = 100f;

    // TRAVA DE SEGURANÇA: Garante que a moeda só seja coletada UMA vez
    private bool jaColetada = false;

    void Update()
    {
        transform.Rotate(Vector3.up * velocidadeRotacao * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Se já foi coletada, ignora qualquer outra colisão
        if (jaColetada) return;

        if (other.CompareTag("Player"))
        {
            PlayerController jogador = other.GetComponent<PlayerController>();

            if (jogador != null)
            {
                jaColetada = true; // Ativa a trava imediatamente!
                
                jogador.AdicionarMoeda(valorDaMoeda);
                Destroy(gameObject); 
            }
        }
    }
}