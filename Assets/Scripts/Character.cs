using UnityEngine;
using System.Collections;

public interface Character {
    GameObject obj {
        get;
    }
    float damage {
        get;
    }
    bool isAlive {
        get;
    }
    void Hit(float dmg, Character c);
    void OnAttackEnd();
}
