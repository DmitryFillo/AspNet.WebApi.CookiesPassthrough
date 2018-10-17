using System;
using System.Diagnostics;

namespace AspNet.WebApi.CookiesPassthrough
{
    /// <inheritdoc />
    /// <summary>
    /// Encapsulates cookie record
    /// </summary>
    [DebuggerDisplay("{Name}={Value}")]
    public class CookieDescriptor : IEquatable<CookieDescriptor>
    {
        /// <summary>
        /// Creates cookie record
        /// </summary>
        /// <param name="name">Name of the cookie</param>
        /// <param name="value">Cookie value</param>
        public CookieDescriptor(string name, string value)
        {
            Value = value;
            Name = name;
        }

        /// <summary>
        /// Cookie name
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// Cookie value
        /// </summary>
        public string Value { get; }
        
        /// <summary>
        /// HttpOnly flag
        /// </summary>
        public bool HttpOnly { get; set; }

        /// <summary>
        /// Secure flag
        /// </summary>
        public bool Secure { get; set; }

        /// <summary>
        /// Cookie path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Sets code status. Cookie value can be encoded or decoded depending on this value
        /// </summary>
        public CookieCodeStatus CodeStatus { get; set; }

        /// <summary>
        /// Cookie expires date
        /// </summary>
        public DateTime Expires { get; set; }

        /// <inheritdoc />
        public bool Equals(CookieDescriptor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && string.Equals(Value, other.Value);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((CookieDescriptor) obj);
        }

        /// <inheritdoc />
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
