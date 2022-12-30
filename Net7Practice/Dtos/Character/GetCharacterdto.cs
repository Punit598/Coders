namespace Net7Practice.Dtos.Character
{
    public class GetCharacterdto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HitPoints { get; set; }
        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Defense { get; set; }
        public Rpg Class { get; set; }
    }
}
