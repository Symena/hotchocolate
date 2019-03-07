﻿using System;
using System.Collections.Generic;
using System.Reflection;
using HotChocolate.Language;
using HotChocolate.Resolvers.CodeGeneration;
using HotChocolate.Types.Descriptors.Definitions;
using HotChocolate.Utilities;

namespace HotChocolate.Types.Descriptors
{
    public class InterfaceFieldDescriptor
        : OutputFieldDescriptorBase<InterfaceFieldDefinition>
        , IInterfaceFieldDescriptor
    {
        private bool _argumentsInitialized;

        public InterfaceFieldDescriptor(
            IDescriptorContext context,
            NameString fieldName)
            : base(context)
        {
            Definition.Name = fieldName.EnsureNotEmpty(nameof(fieldName));
        }

        public InterfaceFieldDescriptor(
            IDescriptorContext context,
            MemberInfo member)
            : base(context)
        {
            Definition.Member = member
                ?? throw new ArgumentNullException(nameof(member));

            Definition.Name = context.Naming.GetMemberName(
                member, MemberKind.InputObjectField);
            Definition.Description = context.Naming.GetMemberDescription(
                member, MemberKind.InputObjectField);
            Definition.Type = context.Inspector.GetOutputReturnType(member);
        }

        protected override InterfaceFieldDefinition Definition { get; } =
            new InterfaceFieldDefinition();

        protected override void OnCreateDefinition(
            InterfaceFieldDefinition definition)
        {
            CompleteArguments(definition);
        }

        private void CompleteArguments(InterfaceFieldDefinition definition)
        {
            if (!_argumentsInitialized)
            {
                FieldDescriptorUtilities.DiscoverArguments(
                    definition.Arguments,
                    definition.Member);
                _argumentsInitialized = true;
            }
        }

        public new IInterfaceFieldDescriptor SyntaxNode(
            FieldDefinitionNode fieldDefinitionNode)
        {
            base.SyntaxNode(fieldDefinitionNode);
            return this;
        }

        public new IInterfaceFieldDescriptor Name(
            NameString name)
        {
            base.Name(name);
            return this;
        }

        public new IInterfaceFieldDescriptor Description(
            string description)
        {
            base.Description(description);
            return this;
        }

        public new IInterfaceFieldDescriptor DeprecationReason(
            string deprecationReason)
        {
            base.DeprecationReason(deprecationReason);
            return this;
        }

        public new IInterfaceFieldDescriptor Type<TOutputType>()
            where TOutputType : IOutputType
        {
            base.Type<TOutputType>();
            return this;
        }

        public new IInterfaceFieldDescriptor Type<TOutputType>(
            TOutputType outputType)
            where TOutputType : class, IOutputType
        {
            base.Type<TOutputType>(outputType);
            return this;
        }

        public new IInterfaceFieldDescriptor Type(ITypeNode type)
        {
            base.Type(type);
            return this;
        }

        public IInterfaceFieldDescriptor Argument(
            NameString name,
            Action<IArgumentDescriptor> argument)
        {
            base.Argument(name, argument);
            return this;
        }

        public new IInterfaceFieldDescriptor Ignore()
        {
            base.Ignore();
            return this;
        }

        public new IInterfaceFieldDescriptor Directive<T>(T directive)
            where T : class
        {
            base.Directive(directive);
            return this;
        }

        public new IInterfaceFieldDescriptor Directive<T>()
            where T : class, new()
        {
            base.Directive<T>();
            return this;
        }

        public new IInterfaceFieldDescriptor Directive(
            NameString name,
            params ArgumentNode[] arguments)
        {
            base.Directive(name, arguments);
            return this;
        }

        public static InterfaceFieldDescriptor New(
            IDescriptorContext context,
            NameString fieldName) =>
            new InterfaceFieldDescriptor(context, fieldName);

        public static InterfaceFieldDescriptor New(
            IDescriptorContext context,
            MemberInfo member) =>
            new InterfaceFieldDescriptor(context, member);
    }
}
