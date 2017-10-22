namespace SendOwl.Model
{
    public enum ResendEmailOption
    {
        Unknown = 0,
        /// <summary>
        /// Resend the order or gift order notification
        /// </summary>
        Order = 1,
        /// <summary>
        /// Resend the receipt notification for each transaction in the order
        /// </summary>
        Transactions = 2,
        /// <summary>
        /// Resend both the order and receipt notifications
        /// </summary>
        All = 3
    }
}
