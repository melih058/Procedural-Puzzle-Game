using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseSingleton<UIManager>
{
    [SerializeField] private GameObject _successPanel;

    private void Start()
    {
        onInitialize();
    }
    private void onInitialize()
    {
        _successPanel.SetActive(false);
    }
    public void showSuccess()
    {
        _successPanel.SetActive(true);
    }

}
