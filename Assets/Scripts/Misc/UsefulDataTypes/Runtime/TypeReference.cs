using System;
using UnityEngine;

[System.Serializable]
public class TypeReference : ISerializationCallbackReceiver, IEquatable<TypeReference>, IEquatable<System.Type>
{
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.ShowInInspector]
#endif
    public System.Type Type;

    [SerializeField, HideInInspector]
    protected string _typeString;

    public TypeReference() { }
    public TypeReference(System.Type type)
    {
        Type = type;
        _typeString = Type?.AssemblyQualifiedName;
    }

    public void SetType(TypeReference typeReference) => SetType(typeReference.Type);

    public void SetType(System.Type type)
    {
        Type = type;
        _typeString = Type?.AssemblyQualifiedName;
    }

    public static implicit operator System.Type(TypeReference typeReference) => typeReference?.Type;
    public static implicit operator TypeReference(System.Type type) => new TypeReference(type);

    public void OnAfterDeserialize()
    {
        try
        {
            Type = System.Type.GetType(_typeString);
        }
        catch
        {
            Type = null;
        }
    }

    public void OnBeforeSerialize()
    {
        try
        {
            _typeString = Type?.AssemblyQualifiedName;
        }
        catch
        {
            _typeString = "";
        }
    }

    public bool Equals(TypeReference p)
    {
        if (ReferenceEquals(p, null))
        {
            return false;
        }

        if (ReferenceEquals(this, p))
        {
            return true;
        }

        return this.Type == p.Type;
    }

    public static bool operator ==(TypeReference lhs, TypeReference rhs)
    {
        return lhs?.Equals(rhs) ?? ReferenceEquals(rhs, null);
    }

    public static bool operator !=(TypeReference lhs, TypeReference rhs)
    {
        return !(lhs == rhs);
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as TypeReference);
    }

    public override int GetHashCode()
    {
        return Type == null ? 0 : Type.GetHashCode();
    }

    public bool Equals(Type other)
    {
        if (Type is null)
            return false;

        return Type.Equals(other);
    }
}