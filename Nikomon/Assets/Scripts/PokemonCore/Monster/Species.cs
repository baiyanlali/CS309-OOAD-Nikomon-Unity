namespace PokemonCore.Monster
{
    /// <summary>
    /// 即宝可梦中的Category，但是因为这个词用作别的用途了，这里用Species替代一下
    /// </summary>
    public class Species
    {
        public int ID;
        public string Description;

        public Species(int ID = 0, string description = "")
        {
            this.ID = ID;
            this.Description = description;
        }
    }
}