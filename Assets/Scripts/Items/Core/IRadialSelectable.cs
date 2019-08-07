﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRadialSelectable
{
    Sprite GetIcon();
    void Select(bool value);
}
