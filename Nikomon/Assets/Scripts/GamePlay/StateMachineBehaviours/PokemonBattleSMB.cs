using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlay.StateMachineBehaviours
{
    public class PokemonBattleSMB : StateMachineBehaviour
    {
        public PokemonIndentity pokemonIndentity;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            pokemonIndentity=pokemonIndentity? pokemonIndentity: animator.gameObject.GetComponentInParent<PokemonIndentity>();
            pokemonIndentity.OnAnimStart();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            pokemonIndentity=pokemonIndentity? pokemonIndentity: animator.gameObject.GetComponentInParent<PokemonIndentity>();
            pokemonIndentity.OnAnimEnd();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }
    }
}