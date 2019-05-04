using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PawnEvent : UnityEvent<Pawn> { }

[System.Serializable]
public class EnergyEvent : UnityEvent<EnergyManager> { }