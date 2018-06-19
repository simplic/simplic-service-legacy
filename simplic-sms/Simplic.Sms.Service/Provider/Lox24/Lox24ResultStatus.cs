namespace Simplic.Sms.Service.Provider.Lox24
{
    public enum Lox24ResultStatus
    {        
        /// <summary>
        /// SMS transmitted successfully
        /// </summary>
        SmsSuccess = 100,

        /// <summary>
        /// Successful query
        /// </summary>
        SuccessfulQuery = 101,

        /// <summary>
        /// Command executed
        /// </summary>
        CommandExecuted = 102,

        /// <summary>
        /// User-ID, Password or Service-ID wrong
        /// </summary>
        WrongCredentials = 200,

        /// <summary>
        /// No Text
        /// </summary>
        NoText = 201,

        /// <summary>
        /// No Recipient
        /// </summary>
        NoRecipient = 202,

        /// <summary>
        /// Text too long (>1520 characters)
        /// </summary>
        TextTooLong = 203,

        /// <summary>
        /// Not enough credit line / account balance.
        /// </summary>
        NotEnoughCredit = 204,

        /// <summary>
        /// No MMS data
        /// </summary>
        NoMmsData = 205,

        /// <summary>
        /// MMS file size too big
        /// </summary>
        MmsTooBig = 206,

        /// <summary>
        /// Invalid IP
        /// </summary>
        InvalidIp = 207,

        /// <summary>
        /// Destination country or network is blocked
        /// </summary>
        DestinationNetworkBlocked = 208,

        /// <summary>
        /// Invalid XML-Schema
        /// </summary>
        InvalidXmlSchema = 209,

        /// <summary>
        /// Invalid Sender-ID
        /// </summary>
        InvalidSenderId = 210,

        /// <summary>
        /// Invalid Parameter for Encoding
        /// </summary>
        InvalidEncoding = 211
    }
}
