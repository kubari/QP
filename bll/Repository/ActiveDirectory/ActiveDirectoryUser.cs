using System;
using System.Linq;
using Novell.Directory.Ldap;

namespace Quantumart.QP8.BLL.Repository.ActiveDirectory
{
    public class ActiveDirectoryUser : ActiveDirectoryEntityBase
    {
        private readonly string AccountDescriptor = "DC=";
        private readonly string _domain;

        public string FirstName { get; }
        public string LastName { get; }
        public string Mail { get; }
        public string AccountName { get; }
        public UserAccountControlDescription AccountControl { get; }
        public bool IsDisabled { get; }

        public ActiveDirectoryUser(LdapEntry user, string domain = null)
            : base(user)
        {
            _domain = domain;
            LdapAttributeSet attributes = user.GetAttributeSet();
            FirstName = GetAttrbibuteValue<string>(attributes, "givenName", false);
            LastName = GetAttrbibuteValue<string>(attributes, "sn", false);
            Mail = GetAttrbibuteValue<string>(attributes, "mail", false);
            AccountName = GetDomain() + "\\" + GetAttrbibuteValue<string>(attributes, "sAMAccountName", true);
            AccountControl = (UserAccountControlDescription)int.Parse(GetAttrbibuteValue<string>(attributes, "userAccountControl", true));
            IsDisabled = (AccountControl & UserAccountControlDescription.ACCOUNTDISABLE) == UserAccountControlDescription.ACCOUNTDISABLE;
        }

        private string GetDomain()
        {
            return !string.IsNullOrEmpty(_domain)
                ? _domain
                : ReferencedPath.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(s => s.StartsWith(AccountDescriptor))
                    .Select(s => s.Replace(AccountDescriptor, string.Empty))
                    .FirstOrDefault();
        }
    }
}
