namespace MoneyEntity
{
    public class Commands
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ParamsJson { get; set; }
        public MoneyEntityBase? ParamsDeserialized { get; set; }
        public ulong Handle { get; set; }
        public bool IsError { get; set; }
    }
}