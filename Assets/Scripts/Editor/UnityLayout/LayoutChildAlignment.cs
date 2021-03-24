public class LayoutChildAlignment
{
    public LayoutAlignment alignment;
    public bool useParent;

    public LayoutChildAlignment() : this(LayoutAlignment.Start, true) { }
    public LayoutChildAlignment(LayoutAlignment a) : this(a, false) { }

    private LayoutChildAlignment(LayoutAlignment a, bool uP)
    {
        alignment = a;
        useParent = uP;
    }

    // Factory methods
    public static LayoutChildAlignment Default()
    {
        return new LayoutChildAlignment();
    }
    public static LayoutChildAlignment Align(LayoutAlignment align)
    {
        return new LayoutChildAlignment(align);
    }
    
    public LayoutAlignment CompileAlignment(LayoutAlignment parentAlignment)
    {
        if (useParent) return parentAlignment;
        else return alignment;
    }
}
