using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;
        // 전투가 시작중인지  전투가 취소 되었는 지 
        public void StartAction(IAction action)
        {
            if (currentAction == action) return;
            if (currentAction != null)
            {
                // if Combat // if Movement
                print($"Cancelling :{currentAction}");
                currentAction.Cancel();
            }
            
            currentAction = action; 

        }
        public void CancelCurrentAction()
        {
            StartAction(null); //새로운 행동으로 아무것도 안함 
        }
    }

}

