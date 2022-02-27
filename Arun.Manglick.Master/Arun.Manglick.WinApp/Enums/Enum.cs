using System;

/// <summary>
/// Active Flag
/// </summary>
public enum Active
{
    /// <summary>
    /// Active = False
    /// </summary>
    Inactive = 0,
    /// <summary>
    /// Active = True
    /// </summary>
    Active = 1,
    /// <summary>
    /// Both 
    /// </summary>
    Both = 2
}

[Flags]
public enum Days2
{
    None = 0x0,
    Sunday = 0x1,
    Monday = 0x2,
    Tuesday = 0x4,
    Wednesday = 0x8,
    Thursday = 0x10,
    Friday = 0x20,
    Saturday = 0x40
}
