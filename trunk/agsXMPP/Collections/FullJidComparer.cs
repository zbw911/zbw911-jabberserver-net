// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FullJidComparer.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region file header


#endregion

#region file header


#endregion

namespace agsXMPP.Collections
{
    #region usings

    using System;
    using System.Collections;

    #endregion

    /// <summary>
    /// </summary>
    public class FullJidComparer : IComparer
    {
        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="x">
        /// </param>
        /// <param name="y">
        /// </param>
        /// <returns>
        /// </returns>
        public int Compare(object x, object y)
        {
            if (x is Jid && y is Jid)
            {
                Jid jidX = (Jid) x;
                Jid jidY = (Jid) y;

                if (jidX.ToString() == jidY.ToString())
                {
                    return 0;
                }
                else
                {
                    return String.Compare(jidX.ToString(), jidY.ToString());
                }
            }

            throw new ArgumentException("the objects to compare must be Jids");
        }

        #endregion
    }
}