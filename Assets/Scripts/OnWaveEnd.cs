using UnityEngine;
using System.Collections;

public class OnWaveEnd : StateMachineBehaviour {
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        DebugWindow.LogSystem(GetType().Name,System.Reflection.MethodBase.GetCurrentMethod().Name);
        animator.SetBool("Wave",false);
    }
}
