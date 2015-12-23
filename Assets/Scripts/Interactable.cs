using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour {

    public bool isUI;

    public InteractType interactType;
    public bool isDraggable {
        get {
            return interactType == InteractType.draggable;
        }
    }
    public bool isPet {
        get {
            return interactType == InteractType.pet;
        }
    }
}

public enum InteractType {
    draggable,
    pet,
    petSelect
}
