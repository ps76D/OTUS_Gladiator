using System;
using System.Collections;
using CodeBase.Infrastructure;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
  public class SceneLoader
  {
    private readonly ICoroutineRunner _coroutineRunner;

    public SceneLoader(ICoroutineRunner coroutineRunner)
    {
      _coroutineRunner = coroutineRunner;
    }

    public void Load(string name, Action onLoaded = null, Action onLoadStart = null) =>
      _coroutineRunner.StartCoroutine(LoadSceneAsync(name, onLoaded, onLoadStart));
    
    public void ReLoad(string name, Action onLoaded = null, Action onLoadStart = null) =>
      _coroutineRunner.StartCoroutine(ReLoadSceneAsync(name, onLoaded, onLoadStart));
    
    public void LoadAdditive(string name, Action onLoaded = null, Action onLoadStart = null) =>
      _coroutineRunner.StartCoroutine(LoadSceneAsyncAdditive(name, onLoaded, onLoadStart));
    
    public void ReLoadAdditive(string name, Action onLoaded = null, Action onLoadStart = null) =>
      _coroutineRunner.StartCoroutine(ReLoadSceneAsyncAdditive(name, onLoaded, onLoadStart));
    public void UnLoadThenLoadSceneAdditive(string name, Action onLoaded = null, Action onLoadStart = null) =>
      _coroutineRunner.StartCoroutine(UnLoadThenLoadSceneAsyncAdditive(name, onLoaded, onLoadStart));
    
    public bool IsSceneLoaded(string sceneName)
    {
      Scene scene = SceneManager.GetSceneByName(sceneName);
      return scene.isLoaded;
    }

    public void UnloadIfSceneLoaded(string name, Action onLoadStart = null)
    {
      _coroutineRunner.StartCoroutine(UnloadIfSceneLoadedCoroutine(name, onLoadStart));
    }

    private IEnumerator UnloadIfSceneLoadedCoroutine(string name, Action onLoadStart)
    {
      if (IsSceneLoaded(name))
      {
        onLoadStart?.Invoke();

        yield return new WaitForSecondsRealtime(1f);
        SceneManager.UnloadSceneAsync(name);
        Debug.Log($"Unloading {name}");
      }
      else
      {
        yield return null;
      }
    }

    
    private IEnumerator LoadSceneAsync(string nextScene, Action onLoaded, Action onLoadStart)
    {
      if (SceneManager.GetActiveScene().name == nextScene)
      {
        onLoaded?.Invoke();
        yield break;
      }
      
      onLoadStart?.Invoke();

      yield return new WaitForSecondsRealtime(1f);
      
      AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);

      while (!waitNextScene.isDone)
        yield return null;

      onLoaded?.Invoke();
    }
    
    private IEnumerator ReLoadSceneAsync(string nextScene, Action onLoaded, Action onLoadStart)
    {
      onLoadStart?.Invoke();
      
      yield return new WaitForSecondsRealtime(1f);
      
      AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

      while (!waitNextScene.isDone)
        yield return null;
      
      onLoaded?.Invoke();
    }
    
    private IEnumerator LoadSceneAsyncAdditive(string nextScene, Action onLoaded, Action onLoadStart)
    {
      if (SceneManager.GetActiveScene().name == nextScene)
      {
        onLoaded?.Invoke();
        yield break;
      }
      
      onLoadStart?.Invoke();

      yield return new WaitForSecondsRealtime(1f);
      
      AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);

      while (!waitNextScene.isDone)
        yield return null;

      onLoaded?.Invoke();
    }
    
    private IEnumerator ReLoadSceneAsyncAdditive(string nextScene, Action onLoaded, Action onLoadStart)
    {
      onLoadStart?.Invoke();
      
      yield return new WaitForSecondsRealtime(1f);

      if (!SceneManager.GetSceneByName(nextScene).isLoaded)
      {
        AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);

        while (!waitNextScene.isDone)
          yield return null;
      }

      onLoaded?.Invoke();
    }
    
    private IEnumerator UnLoadThenLoadSceneAsyncAdditive(string nextScene, Action onLoaded, Action onLoadStart)
    {
      onLoadStart?.Invoke();
      
      yield return new WaitForSecondsRealtime(1f);

      if (!SceneManager.GetSceneByName(nextScene).isLoaded)
      {
        // Load again
        AsyncOperation load = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        while (!load.isDone)
          yield return null;
      }
      onLoaded?.Invoke();
    }
  }
}