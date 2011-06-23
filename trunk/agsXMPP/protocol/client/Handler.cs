// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Handler.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.client
{
    /// <summary>
    /// </summary>
    /// <param name="sender">
    /// </param>
    /// <param name="msg">
    /// </param>
    public delegate void MessageHandler(object sender, Message msg);

    /// <summary>
    /// </summary>
    /// <param name="sender">
    /// </param>
    /// <param name="pres">
    /// </param>
    public delegate void PresenceHandler(object sender, Presence pres);

    /// <summary>
    /// </summary>
    /// <param name="sender">
    /// </param>
    /// <param name="iq">
    /// </param>
    public delegate void IqHandler(object sender, IQ iq);
}