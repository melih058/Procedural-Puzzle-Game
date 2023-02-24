using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : BaseSingleton<GameManager>
{
    public void onLevelSuccess()
    {
        UIManager.instance.showSuccess();
    }
}
