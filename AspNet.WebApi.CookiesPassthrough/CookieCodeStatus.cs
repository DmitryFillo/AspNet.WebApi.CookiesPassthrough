namespace AspNet.WebApi.CookiesPassthrough
{
    /// <summary>
    /// Cookie code status
    /// </summary>
    public enum CookieCodeStatus
    {
        /// <summary>
        /// Nothing to do with the cookie value
        /// </summary>
        Nothing = 0,

        /// <summary>
        /// Encodes cookie value
        /// </summary>
        Encode = 1,

        /// <summary>
        /// Decodes cookie value
        /// </summary>
        Decode = 2,
    }
}
