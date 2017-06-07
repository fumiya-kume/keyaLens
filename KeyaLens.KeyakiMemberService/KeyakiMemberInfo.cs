using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyaLens.KeyakiMemberService
{
    public class KeyakiMemberInfo : IEquatable<KeyakiMemberInfo>
    {
        public string Name { get; set; }
        public string ProfileImageURL { get; set; }
        public string memberPageURL { get; set; }

        public override int GetHashCode()
        {
            var NameHashCode = Name == null ? 0 : Name.GetHashCode();
            var profileImageURLHashCode = ProfileImageURL == null ? 0 : ProfileImageURL.GetHashCode();
            var MemberPageURLHashCode = memberPageURL == null ? 0 : memberPageURL.GetHashCode();
            return NameHashCode ^ profileImageURLHashCode ^ MemberPageURLHashCode;
        }

        public bool Equals(KeyakiMemberInfo other)
        {
            return Name.Equals(other.Name) &&
                ProfileImageURL.Equals(other.ProfileImageURL) &&
                memberPageURL.Equals(other.memberPageURL);
        }
    }
}
