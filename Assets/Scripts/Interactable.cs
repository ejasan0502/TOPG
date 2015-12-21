using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour {

    public InteractType interactType;

    public bool isUI {
        get {
            return isDraggable;
        }
    }
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
    pet
}
