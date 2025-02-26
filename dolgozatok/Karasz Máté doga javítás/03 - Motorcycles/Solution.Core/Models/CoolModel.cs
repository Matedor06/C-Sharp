public class CoolModel : IObjectValidator<uint>
{
    public uint Id { get; set; }

    public string Name { get; set; }

    public CoolModel()
    {
    }

    public CoolModel(uint id, string name)
    {
        Id = id;
        Name = name;
    }

    public CoolModel(CoolEntity entity)
    {
        if (entity is null)
        {
            return;
        }

        Id = entity.Id;
        Name = entity.Name;
    }

    public override bool Equals(object? obj)
    {
        return obj is CoolModel model &&
               this.Id == model.Id &&
               this.Name == model.Name;
    }
}
