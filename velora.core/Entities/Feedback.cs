using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using velora.core.Entities.IdentityEntities;
using velora.core.Entities;
using Microsoft.AspNetCore.Identity;
public class Feedback : BaseEntity<int>
{
	public string? UserId { get; set; } 

	[Required]
	public string Name { get; set; }

	[Required]
	public string Email { get; set; }

	[Required]
	public string Comment { get; set; }

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public bool IsApproved { get; set; }

	
	// public virtual IdentityUser? User { get; set; }
}