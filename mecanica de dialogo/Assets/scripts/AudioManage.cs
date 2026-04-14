
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
  // #region Singleton 
   public static AudioManager Instance;

   // Fonte 2D para sons de sistema/UI/background em loop
   private AudioSource _systemSource;

   // Lista de AudioSources 3D que estão tocando em loop (ativos)
   private List<AudioSource> _activeSources = new List<AudioSource>();

   private void Awake()
   {
       if (Instance == null)
      {
         Instance = this;
         DontDestroyOnLoad(gameObject);

         // Cria/configura systemSource no mesmo GameObject do AudioManager
         _systemSource = gameObject.GetComponent<AudioSource>();
         if (_systemSource == null)
         {
             _systemSource = gameObject.AddComponent<AudioSource>();
         }
         // Configurações padrão para sons 2D (UI/background)
         _systemSource.playOnAwake = false;
         _systemSource.loop = false; // controlamos quando deve loopar
         _systemSource.spatialBlend = 0f; // 2D
      }
      else
      {
         Destroy(gameObject);
      }
   }

   private void OnDestroy()
   {
       if (Instance == this)
           Instance = null;
   }

   // #endregion

   // ----------------- System (2D) Methods -----------------

   // Toca um clip em loop no systemSource. Se restart==true e já estiver tocando
   // o mesmo clip, reinicia; caso contrário, ignora.
   public void PlaySystemLoop(AudioClip clip, float volume = 1f, bool restart = true)
   {
       if (clip == null)
       {
           Debug.LogWarning("PlaySystemLoop: clip is null");
           return;
       }

       if (_systemSource == null)
       {
           Debug.LogWarning("PlaySystemLoop: systemSource not available");
           return;
       }

       if (_systemSource.isPlaying && _systemSource.clip == clip)
       {
           if (!restart) return;
       }

       _systemSource.clip = clip;
       _systemSource.volume = Mathf.Clamp01(volume);
       _systemSource.loop = true;
       _systemSource.spatialBlend = 0f;
       _systemSource.Play();
   }

   public void PlaySystemOneShot(AudioClip clip, float volume = 1f)
   {
       if (clip == null)
       {
           Debug.LogWarning("PlaySystemOneShot: clip is null");
           return;
       }
       if (_systemSource == null)
       {
           Debug.LogWarning("PlaySystemOneShot: systemSource not available");
           return;
       }

       _systemSource.spatialBlend = 0f;
       _systemSource.PlayOneShot(clip, Mathf.Clamp01(volume));
   }

   public void StopSystem()
   {
       if (_systemSource == null) return;
       _systemSource.Stop();
       _systemSource.clip = null;
       _systemSource.loop = false;
   }

   public void PauseSystem()
   {
       if (_systemSource == null) return;
       if (_systemSource.isPlaying)
           _systemSource.Pause();
   }

   public void ResumeSystem()
   {
       if (_systemSource == null) return;
       if (!_systemSource.isPlaying && _systemSource.clip != null)
           _systemSource.UnPause();
   }

   // ----------------- 3D Methods -----------------

   // Cria um AudioSource 3D em posição e toca em loop. Retorna o AudioSource criado
   // para controle posterior (stop/pause/resume).
   public AudioSource Play3DLoop(AudioClip clip, Vector3 position, float volume = 1f,
       float spatialBlend = 1f, float minDistance = 1f, float maxDistance = 500f, bool restart = true)
   {
       if (clip == null)
       {
           Debug.LogWarning("Play3DLoop: clip is null");
           return null;
       }

       // Se já existir um source nessa posição com o mesmo clip e restart==false, ignorar
       // (opcional: checagem simples usando activeSources)

       GameObject go = new GameObject("3DSoundLoop_" + clip.name);
       go.transform.position = position;
       var src = go.AddComponent<AudioSource>();
       src.clip = clip;
       src.volume = Mathf.Clamp01(volume);
       src.loop = true;
       src.playOnAwake = false;
       src.spatialBlend = Mathf.Clamp01(spatialBlend); // 1 = fully 3D
       src.minDistance = Mathf.Max(0.01f, minDistance);
       src.maxDistance = Mathf.Max(src.minDistance, maxDistance);
       src.rolloffMode = AudioRolloffMode.Linear;
       src.Play();

       // Marca como não-destrutível automaticamente; gerencia pela lista
       _activeSources.Add(src);
       return src;
   }

   // Toca um clip 3D uma única vez e destroi o GameObject após terminar
   public void Play3DOneShot(AudioClip clip, Vector3 position, float volume = 1f,
       float spatialBlend = 1f, float minDistance = 1f, float maxDistance = 500f)
   {
       if (clip == null)
       {
           Debug.LogWarning("Play3DOneShot: clip is null");
           return;
       }

       GameObject go = new GameObject("3DSoundOneShot_" + clip.name);
       go.transform.position = position;
       var src = go.AddComponent<AudioSource>();
       src.clip = clip;
       src.volume = Mathf.Clamp01(volume);
       src.loop = false;
       src.playOnAwake = false;
       src.spatialBlend = Mathf.Clamp01(spatialBlend);
       src.minDistance = Mathf.Max(0.01f, minDistance);
       src.maxDistance = Mathf.Max(src.minDistance, maxDistance);
       src.rolloffMode = AudioRolloffMode.Linear;
       src.Play();

       // Destroi o GameObject quando o clip terminar
       Destroy(go, clip.length + 0.1f);
   }

   // Para um AudioSource 3D que esteja na lista de ativos
   public void Stop3D(AudioSource source)
   {
       if (source == null) return;
       if (_activeSources.Contains(source))
           _activeSources.Remove(source);

       source.Stop();
       if (source.gameObject != null)
           Destroy(source.gameObject);
   }

   public void Pause3D(AudioSource source)
   {
       if (source == null) return;
       source.Pause();
   }

   public void Resume3D(AudioSource source)
   {
       if (source == null) return;
       source.UnPause();
   }

   // Utilities para gerenciar todos os 3D loops registrados
   public void StopAll3DLoops()
   {
       for (int i = _activeSources.Count - 1; i >= 0; i--)
       {
           var s = _activeSources[i];
           if (s == null) continue;
           s.Stop();
           if (s.gameObject != null) Destroy(s.gameObject);
       }
       _activeSources.Clear();
   }

   public void PauseAll3DLoops()
   {
       foreach (var s in _activeSources)
       {
           if (s == null) continue;
           s.Pause();
       }
   }

   public void ResumeAll3DLoops()
   {
       foreach (var s in _activeSources)
       {
           if (s == null) continue;
           s.UnPause();
       }
   }

   // Opcional: limpeza periódica de referências null
   private void Update()
   {
       for (int i = _activeSources.Count - 1; i >= 0; i--)
       {
           if (_activeSources[i] == null)
               _activeSources.RemoveAt(i);
       }
   }

}
