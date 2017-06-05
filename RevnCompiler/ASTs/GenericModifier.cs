namespace RevnCompiler.ASTs
{
    public enum Accessibility
    {
        Public,
        Private,
        Internal,
        Protected
    }

    public class GenericModifier
    {
        internal Accessibility Accessibility = Accessibility.Public;
        internal string Static = string.Empty;
    }
}
