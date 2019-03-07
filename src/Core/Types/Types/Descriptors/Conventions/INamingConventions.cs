using System.Reflection;
using System;
using HotChocolate.Types.Descriptors.Definitions;

namespace HotChocolate.Types.Descriptors
{
    public interface INamingConventions
    {
        NameString GetTypeName(Type type, TypeKind kind);

        string GetTypeDescription(Type type, TypeKind kind);

        NameString GetMemberName(MemberInfo member, MemberKind kind);

        string GetMemberDescription(MemberInfo member, MemberKind kind);

        NameString GetArgumentName(ParameterInfo parameter);

        string GetArgumentDescription(ParameterInfo parameter);

        NameString GetEnumValueName(object value);
    }
}
