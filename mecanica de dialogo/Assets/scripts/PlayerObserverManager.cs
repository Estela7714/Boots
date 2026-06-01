using System;

// Note que não tem ": MonoBehaviour" aqui. Ela é uma classe estática pura!
public static class PlayerObserverManager
{
    // O canal por onde a informação das moedas vai passar
    public static event Action<int> OnMoedaColetada;

    // A função usada para transmitir a mensagem
    public static void DispararMoedaColetada(int totalMoedas)
    {
        // Se a UI estiver "escutando", ela recebe o total de moedas
        OnMoedaColetada?.Invoke(totalMoedas);
    }
}