using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Extensions.Logging.Prime.Database.Entity
{
    [PrimaryKey(nameof(Id))]
    internal class BaseEntity
    {
        [Comment("主键，雪花id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Comment("UTC时间")]
        public DateTime LogTime { get; set; }

        [Comment("系统登录用户id")]
        [Column(TypeName = "VARCHAR(64)")]
        public string? UserId { get; set; }

        [Comment("系统登录用户名")]
        [Column(TypeName = "VARCHAR(100)")]
        public string? UserName { get; set; }
    }
}
