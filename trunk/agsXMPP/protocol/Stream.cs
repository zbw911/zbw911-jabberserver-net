// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Stream.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol
{
    /// <summary>
    /// stream:stream Element
    /// This is the first Element we receive from the server.
    /// It encloses our whole xmpp session.
    /// </summary>
    public class Stream : Base.Stream
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public Stream()
        {
            Namespace = Uri.STREAM;
        }

        #endregion
    }
}