using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Core.Models;

public class MotorcycleTypeModel : IObjectValidator<uint>
{
    public uint Id { get; set; }

    public string Name { get; set; }

    public MotorcycleTypeModel()
    {
    }

    public MotorcycleTypeModel(uint id, string name)
    {
        Id = id;
        Name = name;
    }

    public MotorcycleTypeModel(MotorcycleTypeEntity entity)
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
        return obj is MotorcycleTypeModel model &&
               this.Id == model.Id &&
               this.Name == model.Name;
    }
}
