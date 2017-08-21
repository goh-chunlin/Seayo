using System;

namespace Seayo.Models
{
	public abstract class BaseEntity
    {
        public string CreatedBy { get; set; }

		public DateTime CreatedAt { get; set; }

		public bool IsActive { get; set; } = true;
    }
}