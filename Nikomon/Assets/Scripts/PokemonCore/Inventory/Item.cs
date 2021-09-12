namespace PokemonCore.Inventory
{
    public class Item
    {
        public int ID { get; private set; }
        public string name { get; private set; }
        public string description { get; private set; }
        public int value;
    }
}