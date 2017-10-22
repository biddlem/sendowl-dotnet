namespace SendOwl.Model
{
    public enum LimitingType
    {
        Unknown = 0,
        /// <summary>
        /// One discount code that can be used many times
        /// </summary>
        One_code_unlimited_uses = 1,
        /// <summary>
        /// One discount code that can only be used a set number of times
        /// </summary>
        One_code_limited_uses = 2,
        /// <summary>
        /// Many single use discount codes
        /// </summary>
        Many_codes_one_use = 3,
    }
}
