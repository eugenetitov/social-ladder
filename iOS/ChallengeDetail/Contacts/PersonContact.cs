using Contacts;

namespace SocialLadder.iOS.Challenges
{
    public class PersonContact
    {
        public int ContactID { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public CNLabeledValue<CNPhoneNumber>[] PhoneNumbers { get; set; }
        public bool SelectedContact { get; set; }
    }
}