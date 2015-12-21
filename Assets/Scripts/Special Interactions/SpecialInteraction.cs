using UnityEngine;
using System.Collections;

public interface SpecialInteraction {
    void RemoveSelf();

    void OnEnter();
    void OnHold();
    void OnDrag();
    void OnExit();
}
