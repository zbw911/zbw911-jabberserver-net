// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="History.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.x.muc
{
    #region usings

    using System;
    using util;
    using Xml.Dom;
    using Uri = Uri;

    #endregion

    /*
        Example 29. User Requests Limit on Number of Messages in History

        <presence
            from='hag66@shakespeare.lit/pda'
            to='darkcave@macbeth.shakespeare.lit/thirdwitch'>
          <x xmlns='http://jabber.org/protocol/muc'>
            <history maxstanzas='20'/>
          </x>
        </presence>
              

        Example 30. User Requests History in Last 3 Minutes

        <presence
            from='hag66@shakespeare.lit/pda'
            to='darkcave@macbeth.shakespeare.lit/thirdwitch'>
          <x xmlns='http://jabber.org/protocol/muc'>
            <history seconds='180'/>
          </x>
        </presence>
              

        Example 31. User Requests All History Since the Beginning of the Unix Era

        <presence
            from='hag66@shakespeare.lit/pda'
            to='darkcave@macbeth.shakespeare.lit/thirdwitch'>
          <x xmlns='http://jabber.org/protocol/muc'>
            <history since='1970-01-01T00:00Z'/>
          </x>
        </presence>
    */

    /// <summary>
    /// This is used to get the history of a muc room
    /// </summary>
    public class History : Element
    {
        #region Constructor

        /// <summary>
        /// Empty default constructor
        /// </summary>
        public History()
        {
            TagName = "history";
            Namespace = Uri.MUC;
        }

        /// <summary>
        /// get the history starting from a given date when available
        /// </summary>
        /// <param name="date">
        /// </param>
        public History(DateTime date) : this()
        {
            Since = date;
        }

        /// <summary>
        /// Specify the maximum nunber of messages to retrieve from the history
        /// </summary>
        /// <param name="max">
        /// </param>
        public History(int max) : this()
        {
            MaxStanzas = max;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Limit the total number of characters in the history to "X" 
        /// (where the character count is the characters of the complete XML stanzas, 
        /// not only their XML character data).
        /// </summary>
        public int MaxCharacters
        {
            get { return GetAttributeInt("maxchars"); }

            set { SetAttribute("maxchars", value); }
        }

        /// <summary>
        /// Request maximum stanzas of history when available
        /// </summary>
        public int MaxStanzas
        {
            get { return GetAttributeInt("maxstanzas"); }

            set { SetAttribute("maxstanzas", value); }
        }

        /// <summary>
        /// request the last xxx seconds of history when available
        /// </summary>
        public int Seconds
        {
            get { return GetAttributeInt("seconds"); }

            set { SetAttribute("seconds", value); }
        }

        /// <summary>
        /// Request history from a given date when available
        /// </summary>
        public DateTime Since
        {
            get { return Time.ISO_8601Date(GetAttribute("since")); }

            set { SetAttribute("since", Time.ISO_8601Date(value)); }
        }

        #endregion
    }
}