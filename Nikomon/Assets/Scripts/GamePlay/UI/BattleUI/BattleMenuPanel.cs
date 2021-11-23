using System;
using System.Collections.Generic;
using GamePlay.UI.PokemonChooserTable;
using GamePlay.UI.UIFramework;
using PokemonCore;
using PokemonCore.Attack;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using PokemonCore.Inventory;
using PokemonCore.Utility;
using UnityEngine.UI;

namespace GamePlay.UI.BattleUI
{
    public class BattleMenuPanel:BaseUI
    {
        private Button Fight;
        private Button Pokemon;
        private Button Bag;
        private Button Run;

        private string[] PokemonChooses = new[] {"Switch","Show Ability","Close"};
        
        
        private void HandlePokemonChoose(int index)
        {
            print(PokemonChooses[index]);
        }

        public override void Init(params object[] args)
        {
            base.Init(args);

            Fight = GET(Fight, nameof(Fight));
            Pokemon = GET(Pokemon, nameof(Pokemon));
            Bag = GET(Bag, nameof(Bag));
            Run = GET(Run, nameof(Run));

            FirstSelectable = Fight.gameObject;
            
            Fight.onClick.RemoveAllListeners();
            Pokemon.onClick.RemoveAllListeners();
            Bag.onClick.RemoveAllListeners();
            Run.onClick.RemoveAllListeners();

            Fight.onClick.AddListener(() =>
            {
                UIManager.Instance.Show<MovePanel>((Action<int>)ChooseMove);
            });
            Pokemon.onClick.AddListener(() =>
            {
                UIManager.Instance.Show<PokemonChooserPanelUI>(Game.trainer,
                    PokemonChooses,
                    (Action<int>)HandlePokemonChoose
                    );
            });
            Bag.onClick.AddListener(() => { UIManager.Instance.Show<BagPanelUI>(); });
            Run.onClick.AddListener(ToRun);
            
        }

        private CombatPokemon currentPoke => BattleHandler.Instance.CurrentPokemon;
        public void SwitchPokemon(int index)
        {
            UnityEngine.Debug.Log($"Choose switch to index:{index}");
            if (Game.trainer.party[index] == null || Game.trainer.pokemonOnTheBattle[index]) return;
            Instruction ins = new Instruction(currentPoke.CombatID, Command.SwitchPokemon, index,
                null);
            BuildInstrustruction(ins);
        }

        public void UseItem(Item item, int target)
        {
            UseItem(item, new List<int>() {target});
        }

        public void UseItem(Item item, List<int> target)
        {
            target.Insert(0, item.ID);
            Instruction ins = new Instruction(currentPoke.CombatID, Command.Items, (int) item.tag,
                target);
            BuildInstrustruction(ins);
        }

        public void ToRun()
        {
            Instruction ins = new Instruction(currentPoke.CombatID, Command.Run, Game.trainer.id,
                null);
            BuildInstrustruction(ins);
        }
        
        public void ChooseMove(int index)
        {
            Move move = currentPoke.pokemon.moves[index];
            //如果技能效果是针对对面宝可梦而且宝可梦只有一个的话
            if (move._baseData.Target == Targets.SELECTED_OPPONENT_POKEMON &&
                BattleHandler.Instance.OpponentPokemons.Count == 1)
            {
                Instruction instruction =
                    new Instruction(currentPoke.CombatID, Command.Move, index,
                        BattleHandler.Instance.OpponentPokemons[0].CombatID);
                BuildInstrustruction(instruction);
            }
            else
            {
                // TargetChooserHandler.ShowTargetChooser(move._baseData.Target);
                // TargetChooserHandler.OnCancelChoose = () => { MoveUI.SetActive(true); };
                Action<List<int>> onChoose = (o) => OnChooseTarget(o, index);
                Action onCancel = () => UIManager.Instance.Hide(this);
                UIManager.Instance.Show<TargetChooserPanel>(
                    ShowType.Type1,
                    BattleHandler.Instance.OpponentPokemons,
                    BattleHandler.Instance.AlliesPokemons,
                    move._baseData.Target,
                    onChoose,
                    onCancel
                        );
            }
            
            UIManager.Instance.Hide(this);
        }

        private void OnChooseTarget(List<int> targets,int index)
        {
            UnityEngine.Debug.Log(targets.ConverToString());
            Instruction instruction =
                new Instruction(currentPoke.CombatID, Command.Move, index,
                    targets);
            BuildInstrustruction(instruction);
        }
        
        
        public void BuildInstrustruction(Instruction instruction)
        {
            BattleHandler.Instance.ReceiveInstruction(instruction);
        }

    }
}