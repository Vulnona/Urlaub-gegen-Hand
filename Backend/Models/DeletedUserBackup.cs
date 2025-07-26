using System;

namespace Backend.Models
{
    public class DeletedUserBackup
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Original UserId
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Skills { get; set; }
        public string Hobbies { get; set; }
        public string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime DeletedAt { get; set; } // Zeitpunkt der Löschung
    }
}
