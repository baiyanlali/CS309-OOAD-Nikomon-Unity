using System;

namespace PokemonCore.Combat
{
    /// <summary>
    /// Used to report the actions and their effects
    /// </summary>
    public class BattleReporter
    {
        public Action<string> OnReport;

        public BattleReporter(Battle battle)
        {
            battle.OnHit += ObserveBattle;
            battle.OnPokemonFainting += (poke) => { OnReport?.Invoke($"宝可梦 {poke.pokemon.Name} 倒下了！"); };
            battle.OnThisTurnEnd += ShowState;
            battle.OnReplacePokemon += (o1, o2) => { OnReport?.Invoke($"回来吧！{o1.pokemon.Name}。就是你了!{o2.pokemon.Name}");};
            battle.OnBattleEnd += (o) => { OnReport?.Invoke($"最后的结果是----{o}!");};
            battle.OnCatchPokemon += (o) =>
            {
                if (o)
                {
                    OnReport?.Invoke("捕捉成功！");
                }
                else
                {
                    OnReport?.Invoke("捕捉失败");

                }
            };
        }

        public static int ReportNum = 0;
        //TODO: Add multi language support
        public string ObserveBattle(Damage dmg)
        {
            string poke_sponsor = dmg.sponsor.Name;
            string poke_target = dmg.target.Name;
            string move_name = dmg.combatMove.move._baseData.innerName;
            string dis = "";
            if (dmg.typeRate > 1)
            {
                dis = "拔群";
            }
            else if (dmg.typeRate == 0)
            {
                dis = "没有";
            }
            else if (dmg.typeRate < 1)
            {
                dis = "不好";
            }
            else dis = "一般";

            OnReport?.Invoke($"#{++ReportNum}{poke_sponsor} 对 {poke_target} 使用了 {move_name}! 效果{dis}");
            // UnityEngine.Debug.Log($"{poke_sponsor} 对 {poke_target} 使用了 {move_name}! 效果{dis}");
            return $"{poke_sponsor} 对 {poke_target} 使用了 {move_name}! 效果{dis}";
        }

        public void ShowState()
        {
            UnityEngine.Debug.Log(Battle.Instance?.GetBattleInfo());
        }
    }
}