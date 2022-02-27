using System;

namespace Monetrics.MDE.Consumer.DTO
{
    /// <summary>
    /// Interface for Data Transfer Objects
    /// </summary>
    /// <history created="Paresh B"></history>
    /// <history date="Nov 11, 2007"></history>
    public interface IDto
    {
        /// <summary>
        /// Unique identifier for the DTO. 
        /// For new DTO it's -1 else id of the DTO
        /// </summary>
        /// <history created="Paresh B"></history>
        /// <history date="Nov 11, 2007"></history>
        int Id
        {
            get;
            set;
        }
    }
}
