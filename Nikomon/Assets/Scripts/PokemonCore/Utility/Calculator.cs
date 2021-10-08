using PokemonCore.Attack.Data;
using PokemonCore.Monster;
using PokemonCore.Monster.Data;
using System;

namespace PokemonCore.Utility
{
    public static class Calculator
    {
        /// <summary>
        /// 计算血量
        /// </summary>
        /// <param name="baseHP">种族值</param>
        /// <param name="EV">基础点数</param>
        /// <param name="IV">个体值</param>
        /// <param name="level">等级</param>
        /// <returns>HP能力值</returns>
        public static int HPStatus(int baseHP, int EV, int IV, int level)
        {
            //（种族值×2＋基础点数÷4＋个体值）×等级÷100＋等级＋10
            int HP = (baseHP * 2 + EV / 4 + IV) * level / 100 + level + 10;

            return HP;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseStatus"></param>
        /// <param name="EV">基础点数</param>
        /// <param name="IV">个体值</param>
        /// <param name="level">等级</param>
        /// <returns>非HP能力值(ATK DEF SPA SPD SPE)</returns>
        public static int PokemonStatus(int statsFlag, int baseStatus, int EV, int IV, int level, Nature nature)
        {
            int value = 0;
            if (statsFlag == 1)
            {
                value = ((baseStatus * 2 + EV / 4 + IV) * level / 100 + 5) * (int)nature.ATK;
            }
            else if (statsFlag == 2)
            {
                value = ((baseStatus * 2 + EV / 4 + IV) * level / 100 + 5) * (int)nature.DEF;
            }
            else if (statsFlag == 3)
            {
                value = ((baseStatus * 2 + EV / 4 + IV) * level / 100 + 5) * (int)nature.SPA;
            }
            else if (statsFlag == 4)
            {
                value = ((baseStatus * 2 + EV / 4 + IV) * level / 100 + 5) * (int)nature.SPD;
            }
            else
            {
                value = ((baseStatus * 2 + EV / 4 + IV) * level / 100 + 5) * (int)nature.SPE;
            }
            return value;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="EV"></param>
        /// <param name="EV"></param>
        /// <param name="IV">个体值</param>
        /// <param name="level">等级</param>
        /// <returns>Combat Power</returns>
        public static int EV(PokemonData enemy, Pokemon mine)
        {
            int max = findMax(enemy);
            if (mine.EV[max] < 255 && findSum(mine.EV) < 511)
                mine.EV[max]++;
            //TODO:ev值加多少？还有嗑药的问题
            return 0;
        }

        public static int findMax(PokemonData enemy)
        {
            int[] array =
                { enemy.BaseStatsHP, enemy.BaseStatsATK, enemy.BaseStatsDEF, enemy.BaseStatsSPA,
                enemy.BaseStatsSPD, enemy.BaseStatsSPE };
            int max = enemy.BaseStatsHP;
            int result = 0;
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] > max)
                {
                    max = array[i];
                    result = i;
                }
            }
            return result;
        }
        public static int findSum(byte[] array)
        {
            int result = 0;
            for (int i = 0; i < array.Length; i++)
            {
                result += array[i];
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseball">精灵球种类</param>
        /// <param name="MaxHP"></param>
        /// <param name="HP">个体值</param>
        /// <param name="wild">要抓的那个宝可梦个体</param>
        /// <param name="enemy">要抓的那个宝可梦种类</param>
        /// <returns>catchValue</returns>1-255，越大越容易被捕获。255即被立刻捕获。
        public static int CatchRate(int baseball, Pokemon mine, Pokemon wild)
        {
            PokemonData enemy = wild._base;
            //TODO: 地形的判断（精灵球里面）
            Random ran = new Random();
            //int catchValue = ran.Next(1,255);
            float baseballValue = 1;
            if (baseball == 0)
            {
                baseballValue = 1;
            }
            else if (baseball == 1)
            {
                baseballValue = 1.5f;
            }
            else if (baseball == 2)
            {
                baseballValue = 2;
            }
            else if (baseball == 3)
            {
                baseballValue = 255;
            }
            else if (baseball == 4)
            {
                if (mine.Level <= wild.Level)
                {
                    baseballValue = 1;
                }
                else if (mine.Level > wild.Level && mine.Level < wild.Level * 2)
                {
                    baseballValue = 2;
                }
                else if (mine.Level < wild.Level * 4 && mine.Level >= wild.Level * 2)
                {
                    baseballValue = 4;
                }
                else if (mine.Level >= wild.Level * 4)
                {
                    baseballValue = 8;
                }
            }
            else
            {
                //TODO: 其他的精灵球
            }
            //TODO: 状态修正
            float result = (3 * wild.TotalHp - 2 * wild.HP) * enemy.CatchRate * baseballValue / (3 * wild.TotalHp);
            return (int)result;
        }

        /// <summary>判定精灵球是否捕获成功
        public static bool Catch(int baseball,  Pokemon mine, Pokemon wild)
        {
            if (baseball == 3)
                return true;
            double B = CatchRate(baseball,  mine, wild);
            double number = 1048560;//FFFF0
            double num = 16711680;
            double G = number / Math.Sqrt(Math.Sqrt((num / B)));
            Random ran = new Random();
            if (ran.Next(1, 255) < G)
            {
                if (ran.Next(1, 255) < G)
                {
                    if (ran.Next(1, 255) < G)
                    {
                        if (ran.Next(1, 255) < G)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EXP">经验</param>
        /// <param name="s">参加对战且不处于濒死状态的宝可梦数量</param>
        /// <param name="flagK">学习装置是否打开</param>
        /// <param name="flagT">宝可梦是不是自己收服的</param>
        /// <param name="flagE">有没有幸运蛋</param>
        /// <param name="enemy">要抓的那个宝可梦种类</param>
        /// <param name="fight">该宝可梦是否参与战斗</param>
        /// <param name="death">该宝可梦是否濒死</param>
        /// <returns>experience</returns>获得的经验值。
        public static int EXP(PokemonData exp, Pokemon enemy, Pokemon mine, int enemyFlag, int s, bool flagK, bool flagT, bool flagE, bool fight, bool death)
        {
            //((b*l)/(5*s*k) *Math.power((2*l+10)/(l+L+10),2.5)+1) * t * e * f * v * p
            int b = exp.BasicExp;//宝可梦基础经验值
            int l = enemy.Level; int Lp = mine.Level;
            double k = 1;
            if (fight && !death)
            {//战斗了，且不是濒死
                k = 1;
            }
            else if (flagK && !fight && !death)
            {//打开了学习装置，没有战斗，但不是濒死
                k = 0.5;
            }
            else
            {
                k = 0;
            }
            int t = (int)(flagT ? 1 : 1.5);
            int e = (int)(flagE ? 1.5 : 1);
            //TODO:f：宝可拍乐乐或宝可清爽乐中友好度达二颗心以上则为1.2，否则为1
            int f = 1;
            //TODO:v：第六世代以前为1；第六世代起，如果该宝可梦能够通过提升等级进化且当前等级大于等于进化等级，则该值为1.2，否则为1
            int v = 1;
            //TODO:p：释出之力和O力量中经验之力的效果或经验碰碰。经验之力↓↓↓为0.5，
            //经验之力↓↓为0.66，经验之力↓为0.8，经验之力↑或经验之力 Lv.1为1.2，
            //经验之力↑↑或经验之力 Lv.2或经验碰碰为1.5，
            //经验之力↑↑↑或经验之力 S或经验之力 Max或经验之力 Lv.3为2
            int p = 1;
            double temp = Math.Pow((2 * l + 10) / (l + Lp + 10), 2.5);
            int experience = (int)((b * l) / (5 * s * k) * temp + 1) * t * e * f * v * p;
            return experience;
        }

        /// <summary>
        /// 伤害值
        /// </summary>
        /// <param name="baseHP">种族值</param>
        /// <param name="EV">基础点数</param>
        /// <param name="IV">个体值</param>
        /// <param name="level">等级</param>
        /// <returns>HP能力值</returns>
        public static int Demage(Pokemon mine, Pokemon enemy, MoveData move)
        {
            int result = 1;//最少一滴血
            switch (move.Category)
            {
                case (Category.Status):
                    result = 0;
                    break;
                case (Category.Physical):
                    result = (int)((mine.Level * 2 + 10) / 250 * (mine.ATK / enemy.DEF) * move.Power + 2);
                    break;
                case Category.Special:
                    result = (int)((mine.Level * 2 + 10) / 250 * (mine.SPA / enemy.SPD) * move.Power + 2);
                    break;
            }

            return result;
        }

    }
}