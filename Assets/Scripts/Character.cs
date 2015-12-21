using UnityEngine;
using System.Collections;

public interface Character {
    float damage {
        get;
    }
    bool isAlive {
        get;
    }
    void Hit(float dmg);
    void OnAttackEnd();
}
