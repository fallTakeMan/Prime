using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Extensions.Logging.Prime.Database.Entity
{
    [PrimaryKey(nameof(Id))]
    public class SaveChangesAudit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Comment("系统登录用户id")]
        [Column(TypeName = "VARCHAR(64)")]
        public string? UserId { get; set; }

        [Comment("系统登录用户名")]
        [Column(TypeName = "VARCHAR(100)")]
        public string? UserName { get; set; }

        [Comment("UTC时间")]
        public DateTime StartTime { get; set; }

        [Comment("UTC时间")]
        public DateTime EndTime { get; set; }

        public bool Succeeded { get; set; }

        [Comment("错误信息")]
        [Column(TypeName = "TEXT")]
        public string? ErrorMessage { get; set; }

        public ICollection<EntityAudit> Entities { get; } = new List<EntityAudit>();
    }

    [PrimaryKey(nameof(Id))]
    public class EntityAudit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Comment("EntityState(2=Delete;3=Update;4=Insert)")]
        public EntityState State { get; set; }

        [Comment("审核信息")]
        [Column(TypeName = "TEXT")]
        public string? AuditMessage { get; set; }
        
        [JsonIgnore]
        public SaveChangesAudit SaveChangesAudit { get; set; }
    }
}
