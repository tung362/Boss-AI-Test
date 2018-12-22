using System;

[Flags]
public enum OldDebugType : byte
{
    Basic = 1,
    Intermediate = 2,
    Advanced = 4,
    Spam = 8,
    Break = 16,
    Visual = 32,
    Unstable = 64,
    Logged = 128
}