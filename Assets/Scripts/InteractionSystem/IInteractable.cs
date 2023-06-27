// Source: https://www.youtube.com/watch?v=THmW4YolDok

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string InteractionPrompt { get; }
    public bool Interact (Interactor interactor);
}
