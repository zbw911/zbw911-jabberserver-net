// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="RosterItem.cs">
//   
// </copyright>
// <summary>
//   (c) Copyright Ascensio System Limited 2008-2009
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace agsXMPP.protocol.Base
{
    #region usings

    using Xml.Dom;

    #endregion

    // jabber:iq:roster
    // <iq from="gnauck@myjabber.net/Office" id="doroster_1" type="result">
    // 		<query xmlns="jabber:iq:roster">
    // 			<item subscription="both" name="Nachtkrapp" jid="50198521@icq.myjabber.net"><group>ICQ</group></item>
    // 			<item subscription="both" name="czerkasov" jid="62764180@icq.myjabber.net"><group>ICQ</group></item>
    // 			<item subscription="both" name="Poacher" jid="92179686@icq.myjabber.net"><group>ICQ</group></item>
    // 			<item subscription="both" name="Diabolo" jid="102840558@icq.myjabber.net"><group>ICQ</group></item>
    // 		</query>
    // </iq> 

    // # "none" -- the user does not have a subscription to the contact's presence information, and the contact does not have a subscription to the user's presence information
    // # "to" -- the user has a subscription to the contact's presence information, but the contact does not have a subscription to the user's presence information
    // # "from" -- the contact has a subscription to the user's presence information, but the user does not have a subscription to the contact's presence information
    // # "both" -- both the user and the contact have subscriptions to each other's presence information

    /// <summary>
    /// Item is used in jabber:iq:roster, x roster
    /// </summary>
    public class RosterItem : Item
    {
        #region Constructor

        /// <summary>
        /// </summary>
        public RosterItem() : base() {}

        #endregion

        #region Methods

        /// <summary>
        /// Groups a roster Item is assigned to
        /// </summary>
        public ElementList GetGroups()
        {
            return SelectElements("group");
        }

        /// <summary>
        /// Add a new group to the Rosteritem
        /// </summary>
        /// <param name="groupname">
        /// </param>
        public void AddGroup(string groupname)
        {
            Group g = new Group(groupname);
            AddChild(g);
        }

        /// <summary>
        /// </summary>
        /// <param name="groupname">
        /// </param>
        /// <returns>
        /// </returns>
        public bool HasGroup(string groupname)
        {
            ElementList groups = GetGroups();
            foreach (Group g in groups)
            {
                if (g.Name == groupname)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="groupname">
        /// </param>
        public void RemoveGroup(string groupname)
        {
            ElementList groups = GetGroups();
            foreach (Group g in groups)
            {
                if (g.Name == groupname)
                {
                    g.Remove();
                    return;
                }
            }
        }

        #endregion
    }
}