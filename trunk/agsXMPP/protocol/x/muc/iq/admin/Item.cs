// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Item.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.x.muc.iq.admin
{
    /// <summary>
    /// </summary>
    public class Item : muc.Item
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public Item() : base()
        {
            Namespace = Uri.MUC_ADMIN;
        }

        /// <summary>
        /// </summary>
        /// <param name="affiliation">
        /// </param>
        public Item(Affiliation affiliation) : this()
        {
            Affiliation = affiliation;
        }

        /// <summary>
        /// </summary>
        /// <param name="affiliation">
        /// </param>
        /// <param name="jid">
        /// </param>
        public Item(Affiliation affiliation, Jid jid) : this(affiliation)
        {
            Jid = jid;
        }

        /// <summary>
        /// </summary>
        /// <param name="role">
        /// </param>
        public Item(Role role) : this()
        {
            Role = role;
        }

        /// <summary>
        /// </summary>
        /// <param name="role">
        /// </param>
        /// <param name="jid">
        /// </param>
        public Item(Role role, Jid jid) : this(role)
        {
            Jid = jid;
        }

        /// <summary>
        /// </summary>
        /// <param name="jid">
        /// </param>
        public Item(Jid jid) : this()
        {
            Jid = jid;
        }

        /// <summary>
        /// </summary>
        /// <param name="affiliation">
        /// </param>
        /// <param name="role">
        /// </param>
        public Item(Affiliation affiliation, Role role) : this(affiliation)
        {
            Role = role;
        }

        /// <summary>
        /// </summary>
        /// <param name="affiliation">
        /// </param>
        /// <param name="role">
        /// </param>
        /// <param name="jid">
        /// </param>
        public Item(Affiliation affiliation, Role role, Jid jid) : this(affiliation, role)
        {
            Jid = jid;
        }

        /// <summary>
        /// </summary>
        /// <param name="affiliation">
        /// </param>
        /// <param name="role">
        /// </param>
        /// <param name="reason">
        /// </param>
        public Item(Affiliation affiliation, Role role, string reason) : this(affiliation, role)
        {
            Reason = reason;
        }

        /// <summary>
        /// </summary>
        /// <param name="affiliation">
        /// </param>
        /// <param name="role">
        /// </param>
        /// <param name="jid">
        /// </param>
        /// <param name="reason">
        /// </param>
        public Item(Affiliation affiliation, Role role, Jid jid, string reason) : this(affiliation, role, jid)
        {
            Reason = reason;
        }

        #endregion
    }
}