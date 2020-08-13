using System.ComponentModel.DataAnnotations;

public class Contact
{
    public Contact()
    {

    }

    public Contact(Contact contact)
    {
        ContactId = contact.ContactId;
        FirstName = contact.FirstName;
        LastName = contact.LastName;
    }

    public int ContactId { get; set; }

    [Required]
    [StringLength(16, ErrorMessage = "First Name can only be 16 characters long.")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(16, ErrorMessage = "Last Name can only be 16 characters long.")]
    public string LastName { get; set; }
}