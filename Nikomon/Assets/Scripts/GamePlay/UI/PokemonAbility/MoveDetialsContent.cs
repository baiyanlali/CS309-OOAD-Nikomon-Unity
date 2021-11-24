using UnityEngine;

namespace GamePlay.UI.PokemonAbility
{
    public class MoveDetialsContent : TabContent
    {

        private Object MoveUIPrefab;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">0 for pokemon</param>
        // public override void OnShow(params object[] args)
        // {
        //     base.OnShow();
        //
        //     MoveUIPrefab = GameResources.SpawnPrefab(typeof(MoveUI));
        //     
        //
        //     Pokemon pokemon = (Pokemon)args[0];
        //     
        // }
        public override void OnShow(params object[] args)
        {
            base.OnShow();
        
            MoveUIPrefab = GameResources.SpawnPrefab(typeof(MoveUI));
            
            
        }

        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
        }
    }
}