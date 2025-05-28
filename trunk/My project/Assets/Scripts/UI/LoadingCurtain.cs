using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace UI
{
  public class LoadingCurtain : MonoBehaviour
  {
    [SerializeField] private CanvasGroup _curtain;

    private void Awake()
    {
      DontDestroyOnLoad(this);
    }

    public void Show() => DoFadeOut();
    
    public void Hide() => DoFadeIn();

    private void DoFadeOut()
    {
      _curtain.DOFade(0, 1f).SetUpdate(true);;
    }

    private void DoFadeIn()
    {
      _curtain.DOFade(1, 1f).SetUpdate(true);;
    }
  }
}