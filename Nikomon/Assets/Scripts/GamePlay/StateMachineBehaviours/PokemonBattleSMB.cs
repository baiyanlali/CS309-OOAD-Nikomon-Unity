using System;
using GamePlay.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlay.StateMachineBehaviours
{
    public class PokemonBattleSMB : StateMachineBehaviour
    {
        public PokemonIdentity pokemonIdentity;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            pokemonIdentity=pokemonIdentity? pokemonIdentity: animator.gameObject.GetComponentInParent<PokemonIdentity>();
            // Debug.Log($">>>>>{pokemonIndentity.name}  enter:{name} {animator.GetAnimateName(stateInfo)}");
            
            if(pokemonIdentity!=null)
                pokemonIdentity.OnAnimStart();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            pokemonIdentity=pokemonIdentity? pokemonIdentity: animator.gameObject.GetComponentInParent<PokemonIdentity>();
            // Debug.Log($"<<<<<{pokemonIndentity.name} exit:{name} {animator.GetAnimateName(stateInfo)}");
            if (pokemonIdentity != null)
            {
                
                pokemonIdentity.OnAnimEnd(stateInfo.shortNameHash);
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