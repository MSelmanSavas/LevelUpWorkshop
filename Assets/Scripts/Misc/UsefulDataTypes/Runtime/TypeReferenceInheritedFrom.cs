[System.Serializable]
public class TypeReferenceInheritedFrom<T1> : TypeReference
{
    public static implicit operator System.Type(TypeReferenceInheritedFrom<T1> typeReference) => typeReference?.Type;
    public static implicit operator TypeReferenceInheritedFrom<T1>(System.Type type) => new TypeReferenceInheritedFrom<T1>(type);

    public TypeReferenceInheritedFrom() : base() { }
    public TypeReferenceInheritedFrom(System.Type type) : base(type)
    {

    }
}