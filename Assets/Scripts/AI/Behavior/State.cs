﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA {
    [CreateAssetMenu]
    public class State : ScriptableObject
    {
        public StateActions[] onEnter;
        public StateActions[] onExit;
        public StateActions[] onFixed;
        public StateActions[] onUpdate;

        public int idCount;
        [SerializeField]
        public List<Transition> transitions = new List<Transition>();
        
        public void OnEnter(StateManager states)
        {
            ExectuteActions(states, onEnter);
        }
        
        public void Tick(StateManager states)
        {

            ExectuteActions(states, onUpdate);
            CheckTransitions(states);
        }

        public void FixedTick(StateManager states)
        {
            ExectuteActions(states, onFixed);
        }
        
        public void OnExit(StateManager states)
        {
            ExectuteActions(states, onExit);
        }

        public void CheckTransitions(StateManager states)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].disable)
                {
                    continue;
                } 
                if (transitions[i].condition.CheckCondition(states))
                {
                    if (transitions[i].targetState != null)
                    {
                        states.currentState = transitions[i].targetState;
                        OnExit(states);
                        states.currentState.OnEnter(states);
                    }
                    
                    return;
                }
            }
        }

        public void ExectuteActions(StateManager states, StateActions[] l)
        {
            for (int i = 0; i < l.Length; i++)
            {
                if (l[i] != null)
                {
                    l[i].Execute(states);
                }
            }
        }
        
        public Transition AddTransition()
        {
            Transition retVal = new Transition();
            transitions.Add(retVal);
            retVal.id = idCount;
            idCount++;
            return retVal;
        }
        public Transition GetTransition(int id)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                if(transitions[i].id == id)
                {
                    return transitions[id];
                }
            }
            return null;
        }
    }
}
