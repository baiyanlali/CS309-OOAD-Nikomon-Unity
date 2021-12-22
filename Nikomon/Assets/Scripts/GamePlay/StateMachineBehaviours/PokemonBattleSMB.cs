using System;
using GamePlay.Utilities;
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
            // Debug.Log($">>>>>{pokemonIndentity.name}  enter:{name} {animator.GetAnimateName(stateInfo)}");
            
            if(pokemonIndentity!=null)
                pokemonIndentity.OnAnimStart();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            pokemonIndentity=pokemonIndentity? pokemonIndentity: animator.gameObject.GetComponentInParent<PokemonIndentity>();
            // Debug.Log($"<<<<<{pokemonIndentity.name} exit:{name} {animator.GetAnimateName(stateInfo)}");
            if (pokemonIndentity != null)
            {
                
                pokemonIndentity.OnAnimEnd(stateInfo.shortNameHash);
            }
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