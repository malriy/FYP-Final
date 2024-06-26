using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    void Interact() { }
}

public interface IAdvanced : Interactable
{
    void NotYetInteract() { }
}
