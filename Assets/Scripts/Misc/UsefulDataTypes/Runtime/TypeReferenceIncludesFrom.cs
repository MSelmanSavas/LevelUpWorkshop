
[System.Serializable]
public class TypeReferenceIncludesFrom<T1> : TypeReference
{
    public static implicit operator System.Type(TypeReferenceIncludesFrom<T1> typeReference) => typeReference?.Type;
    public static implicit operator TypeReferenceIncludesFrom<T1>(System.Type type) => new TypeReferenceIncludesFrom<T1>(type);

    public TypeReferenceIncludesFrom() : base() { }
    public TypeReferenceIncludesFrom(System.Type type) : base(type)
    {

    }
}
