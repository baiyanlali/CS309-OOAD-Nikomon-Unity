namespace PokemonCore.Combat
{
    /// <summary>
    /// Used to report the actions and their effects
    /// </summary>
    public class BattleReporter
    {
        public BattleReporter(Battle battle)
        {
            battle.OnHit+=ObserveBattle;
            battle.OnThisTurnEnd += ShowState;
        }
        
        //TODO: Add multi language support
        public string ObserveBattle(Damage dmg)
        {
            string poke_sponsor = dmg.sponsor.Name;
            string poke_target = dmg.target.Name;
            string move_name = dmg.combatMove.move._baseData.innerName;
            string dis = "";
            if (dmg.damageMultiplyingPower>1)
            {
                dis = "拔群";
            }else if (dmg.damageMultiplyingPower==0)
            {
                dis = "没有";
            }else if (dmg.damageMultiplyingPower < 1)
            {
                dis = "不好";
            }
            else dis = "一般";
            // UnityEngine.Debug.Log($"{poke_sponsor} 对 {poke_target} 使用了 {move_name}! 效果{dis}");
            return $"{poke_sponsor} 对 {poke_target} 使用了 {move_name}! 效果{dis}";
        }

        public void ShowState()
        {
            UnityEngine.Debug.Log(Battle.Instance?.GetBattleInfo());
        }
        
    }
}