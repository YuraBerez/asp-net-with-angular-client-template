using System;
using System.ComponentModel.DataAnnotations;

namespace asp_net_with_angular_client_template.Models.Entity
{
    public class UserRefreshToken
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? CompanyId { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; }
        public DateTime? Revoked { get; set; }
        public string? ReplacedByToken { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;
        public virtual User User { get; set; }
    }
}

