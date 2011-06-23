// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="IQ.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.client
{
    #region usings

    using Base;
    using iq.bind;
    using iq.blocklist;
    using iq.jingle;
    using iq.session;
    using iq.vcard;
    using Xml.Dom;

    #endregion

    // a i know that i shouldnt use keywords for Enums. But its much easier this way
    // because of enum.ToString() and enum.Parse() Members
    /// <summary>
    /// </summary>
    public enum IqType
    {
        /// <summary>
        /// </summary>
        get, 

        /// <summary>
        /// </summary>
        set, 

        /// <summary>
        /// </summary>
        result, 

        /// <summary>
        /// </summary>
        error
    }

    /// <summary>
    /// Iq Stanza.
    /// </summary>
    public class IQ : Stanza
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public IQ()
        {
            TagName = "iq";
            Namespace = Uri.CLIENT;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public IQ(IqType type) : this()
        {
            Type = type;
        }

        /// <summary>
        /// </summary>
        /// <param name="from">
        /// </param>
        /// <param name="to">
        /// </param>
        public IQ(Jid from, Jid to) : this()
        {
            From = from;
            To = to;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="from">
        /// </param>
        /// <param name="to">
        /// </param>
        public IQ(IqType type, Jid from, Jid to) : this()
        {
            Type = type;
            From = from;
            To = to;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get or Set the Bind ELement if it is a BingIq
        /// </summary>
        public virtual Bind Bind
        {
            get { return SelectSingleElement(typeof (Bind)) as Bind; }

            set
            {
                RemoveTag(typeof (Bind));
                if (value != null)
                {
                    AddChild(value);
                }
            }
        }

        public virtual Blocklist Blocklist
        {
            get { return SelectSingleElement(typeof(Blocklist)) as Blocklist; }

            set
            {
                RemoveTag(typeof(Blocklist));
                if (value != null)
                {
                    AddChild(value);
                }
            }
        }

        public virtual Jingle Jingle
        {
            get { return SelectSingleElement(typeof(Jingle)) as Jingle; }

            set
            {
                RemoveTag(typeof(Jingle));
                if (value != null)
                {
                    AddChild(value);
                }
            }
        }

        /// <summary>
        /// Error Child Element
        /// </summary>
        public Error Error
        {
            get { return SelectSingleElement(typeof (Error)) as Error; }

            set
            {
                if (HasTag(typeof (Error)))
                {
                    RemoveTag(typeof (Error));
                }

                if (value != null)
                {
                    AddChild(value);
                }
            }
        }

        /// <summary>
        /// The query Element. Value can also be null which removes the Query tag when existing
        /// </summary>
        public Element Query
        {
            get { return SelectSingleElement("query"); }

            set
            {
                if (value != null)
                {
                    ReplaceChild(value);
                }
                else
                {
                    RemoveTag("query");
                }
            }
        }

        /// <summary>
        /// Get or Set the Session Element if it is a SessionIq
        /// </summary>
        public virtual Session Session
        {
            get { return SelectSingleElement(typeof (Session)) as Session; }

            set
            {
                RemoveTag(typeof (Session));
                if (value != null)
                {
                    AddChild(value);
                }
            }
        }

        /// <summary>
        /// </summary>
        public IqType Type
        {
            get { return (IqType) GetAttributeEnum("type", typeof (IqType)); }

            set { SetAttribute("type", value.ToString()); }
        }

        /// <summary>
        /// Get or Set the VCard if it is a Vcard IQ
        /// </summary>
        public virtual Vcard Vcard
        {
            get { return SelectSingleElement("vCard") as Vcard; }

            set
            {
                if (value != null)
                {
                    ReplaceChild(value);
                }
                else
                {
                    RemoveTag("vCard");
                }
            }
        }

        #endregion
    }
}