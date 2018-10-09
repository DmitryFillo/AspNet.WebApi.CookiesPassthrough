using System;
using System.Diagnostics;

namespace AspNet.WebApi.CookiesPassthrough
{
    [DebuggerDisplay("{Name}={Value}")]
    public class CookieDescriptor : IEquatable<CookieDescriptor>
    {
        public CookieDescriptor(string name, string value)
        {
            Value = value;
            Name = name;
        }

        public string Name { get; }
        public string Value { get; }
        public bool HttpOnly { get; set; }
        public DateTime Expires { get; set; }

        public bool Equals(CookieDescriptor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((CookieDescriptor) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }

        public static bool operator ==(CookieDescriptor left, CookieDescriptor right) => Equals(left, right);

        public static bool operator !=(CookieDescriptor left, CookieDescriptor right) => !Equals(left, right);
    }
}
