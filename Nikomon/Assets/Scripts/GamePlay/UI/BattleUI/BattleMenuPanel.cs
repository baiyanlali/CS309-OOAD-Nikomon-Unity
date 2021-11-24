using System;
using System.Collections.Generic;
using System.Linq;
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
    public class BattleMenuPanel : BaseUI
    {
        private Button Fight;
        private Button Pokemon;
        private Button Bag;
        private Button Run;

        private readonly string[] PokemonChooses = new[] {"Switch", "Show Ability", "Cancel"};
        private readonly string[] BagChooses = new[] {"Use", "Cancel"};


        private void HandlePokemonChoose(int chooseIndex, int bagIndex)
        {
            // print(PokemonChooses[index]);
            switch (chooseIndex)
            {
                case 0: //Switch
                    if (Game.trainer.party[bagIndex] == null || Game.trainer.pokemonOnTheBattle[bagIndex]) return;
                    Instruction ins = new Instruction(currentPoke.CombatID, Command.SwitchPokemon, bagIndex,
                        null);
                    BuildInstrustruction(ins);
                    break;

                case 1: //Show Ability

                    break;
                case 2: //Cancel
                    // UIManager.Instance.Hide(this);
                    break;
            }
        }

        private void HandleItem(int optionIndex, Item item)
        {
            switch (optionIndex)
            {
                case 0:
                    // UIManager.Instance.Show<TargetChooserPanel>();
                    //TODO:实现更复杂的效果
                    UseItem(item,BattleHandler.Instance.OpponentPokemons[0].CombatID);
                    break;
                case 1:
                    break;
            }
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

            //Init Fight
            Fight.onClick.AddListener(() => { UIManager.Instance.Show<MovePanel>((Action<int>) ChooseMove); });

            //Init Pokemon
            Pokemon.onClick.AddListener(() =>
            {
                UIManager.Instance.Show<PokemonChooserPanelUI>(Game.trainer,
                    PokemonChooses,
                    (Action<int, int>) HandlePokemonChoose
                );
            });

            //Init Bag
            Bag.onClick.AddListener(() =>
            {
                UIManager.Instance.Show<BagPanelUI>(Game.bag, BagChooses.ToList(), (Action<int,Item>) HandleItem);
            });


            //Init Run
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

        private void OnChooseTarget(List<int> targets, int index)
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