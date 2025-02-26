using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Database.Entities;

[Table("Cool")]
[Index(nameof(Name), IsUnique = true)]
public class CoolEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }

    [StringLength(64)]
    [Required]
    public string Name { get; set; }

    public virtual ICollection<MotorcycleEntity> Motorcycles { get; set; }
}