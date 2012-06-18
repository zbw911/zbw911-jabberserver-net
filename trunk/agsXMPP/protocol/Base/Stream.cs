// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Stream.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.Base
{
    /// <summary>
    /// Summary description for Stream.
    /// </summary>
    public class Stream : DirectionalElement
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public Stream()
        {
            TagName = "stream";
        }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        public string Id
        {
            get { return GetAttribute("id"); }

            set { SetAttribute("id", value); }
        }

        /// <summary>        
        /// XML Language attribute
        /// </summary>
        /// <remarks>
        /// The language 'xml:lang' attribute  SHOULD be included by the initiating entity on the header for the initial stream 
        /// to specify the default language of any human-readable XML character data it sends over that stream. 
        /// If the attribute is included, the receiving entity SHOULD remember that value as the default for both the 
        /// initial stream and the response stream; if the attribute is not included, the receiving entity SHOULD use 
        /// a configurable default value for both streams, which it MUST communicate in the header for the response stream. 
        /// For all stanzas sent over the initial stream, if the initiating entity does not include an 'xml:lang' attribute, 
        /// the receiving entity SHOULD apply the default value; if the initiating entity does include an 'xml:lang' attribute, 
        /// the receiving entity MUST NOT modify or delete it (see also xml:langxml:lang). 
        /// The value of the 'xml:lang' attribute MUST conform to the format defined in RFC 3066 (Tags for the Identification of Languages, January 2001.[LANGTAGS]).
        /// </remarks>
        public string Language
        {
            get { return GetAttribute("xml:lang"); }

            set { SetAttribute("xml:lang", value); }
        }

        /// <summary>
        /// See XMPP-Core 4.4.1 "Version Support"
        /// </summary>
        public string Version
        {
            get { return GetAttribute("version"); }

            set { SetAttribute("version", value); }
        }

        #endregion
    }
}