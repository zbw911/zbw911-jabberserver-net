// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Stream.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Text;
using agsXMPP.protocol.stream;

namespace agsXMPP.protocol
{
    /// <summary>
    /// stream:stream Element
    /// This is the first Element we receive from the server.
    /// It encloses our whole xmpp session.
    /// </summary>
    public class Stream : Base.Stream
    {
        public Features Features
        {
            get { return this.SelectSingleElement<Features>(); }
            set { AddChild(value); }
        }

        #region Constructor

        /// <summary>
        /// </summary>
        public Stream()
        {
            Namespace = Uri.STREAM;
        }

        #endregion

        public override string ToString()
        {
            var taglen = (Uri.PREFIX + ":stream").Length;
            var s = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            s.Append(base.ToString());
            s.Insert(53, "xmlns=\"" + Uri.CLIENT + "\" ");
            if (HasChildElements)
            {
                s.Remove(s.Length - 16, 16);
            }
            else
            {
                s.Remove(s.Length - 2, 1);
            }
            return s.ToString();
        }
    }
}