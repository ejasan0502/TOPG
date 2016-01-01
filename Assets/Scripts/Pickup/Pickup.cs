using UnityEngine;
using System.Collections;

public interface Pickup {
    void Initialize(int x);
    void Initialize(InventoryItem i);
    void Interact(Pet p);
}
