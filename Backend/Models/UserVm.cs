namespace UGHApi.Models
{
    public class UserVm
    {
        public int User_Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Email_Address { get; set; }
        public string Password { get; set; }
        public string SaltKey { get; set; }
        public bool IsEmailVerified { get; set; }
        public string Facebook_link { get; set; }
        public string Link_RS { get; set; }
        public string Link_VS { get; set; }

        public UGH_Enums.VerificationState VerificationState { get; set; }
        public Membership CurrentMembership { get; set; }
        public int membershipId { get; set; }
    }
}
