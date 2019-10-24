namespace Keys
{
    public enum Accidental
    {
        s = 1,
        x = 2,
        ts = 3,
        f = -1,
        ff = -2,
        tf = -3,
        n = 0,
    }

    public enum Semitone
    {
        c = 0,
        dflat = 1,
        d = 2,
        eflat = 3,
        e = 4,
        f = 5,
        gflat = 6, 
        g = 7,
        aflat = 8,
        a = 9,
        bflat = 10,
        b = 11,
    }

    public enum NoteLetters
    {
        c = 0,
        d = 2,
        e = 4,
        f = 5,
        g = 7,
        a = 9,
        b = 11,
    }

    public enum Mode
    {
        Major = 0,
        Minor = 5,

        Ionian = 0,
        Dorian = 1,
        Phrygian = 2,
        Lydian = 3,
        Mixolydian = 4,
        Aeolian = 5,
        Locrian = 6,
    }
}
