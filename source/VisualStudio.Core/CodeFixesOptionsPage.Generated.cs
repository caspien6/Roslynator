﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// <auto-generated>

using System.Collections.Generic;
using Roslynator.CSharp.CodeFixes;

namespace Roslynator.VisualStudio
{
    public partial class CodeFixesOptionsPage
    {
        public CodeFixesOptionsPage()
        {
            DisabledCodeFixes = $"{CodeFixIdentifiers.AddSemicolon}";
        }

        public void Fill(ICollection<BaseModel> codeFixes)
        {
            codeFixes.Clear();
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.RemoveUnusedVariable, "Remove unused variable (fixes CS0168, CS0219)", IsEnabled(CodeFixIdentifiers.RemoveUnusedVariable)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.AddBreakStatementToSwitchSection, "Add break statement to switch section (fixes CS0163, CS8070)", IsEnabled(CodeFixIdentifiers.AddBreakStatementToSwitchSection)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.CreateSingletonArray, "Create singleton array (fixes CS0266)", IsEnabled(CodeFixIdentifiers.CreateSingletonArray)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.AddOutModifierToArgument, "Add 'out' modifier to argument (fixes CS1620)", IsEnabled(CodeFixIdentifiers.AddOutModifierToArgument)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.MoveBaseClassBeforeAnyInterface, "Base base class before any interface (fixes CS1722)", IsEnabled(CodeFixIdentifiers.MoveBaseClassBeforeAnyInterface)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.AddOverrideModifier, "Add 'override' modifier (fixes CS0114)", IsEnabled(CodeFixIdentifiers.AddOverrideModifier)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.AddNewModifier, "Add 'new' modifier (fixes CS0114)", IsEnabled(CodeFixIdentifiers.AddNewModifier)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.ExtractDeclarationFromUsingStatement, "Extract declaration from using statement (fixes CS1674)", IsEnabled(CodeFixIdentifiers.ExtractDeclarationFromUsingStatement)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.AddBracesToDeclarationOrLabeledStatement, "Add braces to declaration or labeled statement (fixes CS1023)", IsEnabled(CodeFixIdentifiers.AddBracesToDeclarationOrLabeledStatement)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.MarkOperatorAsPublicAndStatic, "Mark operator as 'public' and 'static' (fixes CS0558)", IsEnabled(CodeFixIdentifiers.MarkOperatorAsPublicAndStatic)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.RemoveDuplicateModifier, "Remove duplicate modifier (fixes CS1004)", IsEnabled(CodeFixIdentifiers.RemoveDuplicateModifier)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.RemoveDuplicateAttribute, "Remove duplicate attribute (fixes CS0579)", IsEnabled(CodeFixIdentifiers.RemoveDuplicateAttribute)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.RemoveNewModifier, "Remove new modifier (fixes CS0109)", IsEnabled(CodeFixIdentifiers.RemoveNewModifier)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.RemoveUnusedLabel, "Remove unused label (fixes CS0164)", IsEnabled(CodeFixIdentifiers.RemoveUnusedLabel)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.OverridingMemberCannotChangeAccessModifiers, "Overriding member cannot change access modifiers (fixes CS0507)", IsEnabled(CodeFixIdentifiers.OverridingMemberCannotChangeAccessModifiers)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.AddDocumentationComment, "Add documentation comment (fixes CS1591)", IsEnabled(CodeFixIdentifiers.AddDocumentationComment)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.RemoveUnreachableCode, "Unreachable code detected (fixes CS0162)", IsEnabled(CodeFixIdentifiers.RemoveUnreachableCode)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.MemberReturnTypeMustMatchOverriddenMemberReturnType, "Member return type must match overridden member return type (fixes CS0508)", IsEnabled(CodeFixIdentifiers.MemberReturnTypeMustMatchOverriddenMemberReturnType)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.MemberTypeMustMatchOverriddenMemberType, "Member type must match overridden member type (fixes CS1715)", IsEnabled(CodeFixIdentifiers.MemberTypeMustMatchOverriddenMemberType)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.AddReturnStatementThatReturnsDefaultValue, "Add return statement that returns default value (fixes CS0161)", IsEnabled(CodeFixIdentifiers.AddReturnStatementThatReturnsDefaultValue)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.UseYieldReturnInsteadOfReturn, "Use yield return instead of return (fixes CS0029, CS1622)", IsEnabled(CodeFixIdentifiers.UseYieldReturnInsteadOfReturn)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.ReplaceStringLiteralWithCharacterLiteral, "Replace string literal with character literal (fixes CS0029)", IsEnabled(CodeFixIdentifiers.ReplaceStringLiteralWithCharacterLiteral)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.AddComparisonWithBooleanLiteral, "Add comparison with boolean literal (fixes CS0019, CS0266)", IsEnabled(CodeFixIdentifiers.AddComparisonWithBooleanLiteral)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.RemovePropertyOrFieldInitializer, "Remove property or field initializer (fixes CS0573)", IsEnabled(CodeFixIdentifiers.RemovePropertyOrFieldInitializer)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.AddPartialModifier, "Add partial modifier (fixes CS0260, CS0751)", IsEnabled(CodeFixIdentifiers.AddPartialModifier)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.AddMethodBody, "Add method body (fixes CS0756)", IsEnabled(CodeFixIdentifiers.AddMethodBody)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.RemoveRefModifier, "Remove 'ref' modifier (fixes CS1615)", IsEnabled(CodeFixIdentifiers.RemoveRefModifier)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.RemoveRedundantAssignment, "Remove redundant assignment (fixes CS1717)", IsEnabled(CodeFixIdentifiers.RemoveRedundantAssignment)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.ChangeAccessibility, "Change accessibility (fixes CS0628, CS1057)", IsEnabled(CodeFixIdentifiers.ChangeAccessibility)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.ChangeTypeOfParamsParameter, "Change type of 'params' parameter (fixes CS0225)", IsEnabled(CodeFixIdentifiers.ChangeTypeOfParamsParameter)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.UseUncheckedExpression, "Use unchecked expression (fixes CS0221)", IsEnabled(CodeFixIdentifiers.UseUncheckedExpression)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.RemoveImplementationFromAbstractMember, "Remove implementation from abstract member (fixes CS0069, CS0500, CS0531)", IsEnabled(CodeFixIdentifiers.RemoveImplementationFromAbstractMember)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.AddStaticModifier, "Add 'static' modifier (fixes CS0708, CS0710)", IsEnabled(CodeFixIdentifiers.AddStaticModifier)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.MakeContainingClassAbstract, "Make containing class abstract (fixes CS0513)", IsEnabled(CodeFixIdentifiers.MakeContainingClassAbstract)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.AddSemicolon, "Add semicolon (fixes CS1002)", IsEnabled(CodeFixIdentifiers.AddSemicolon)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.SynchronizeAccessibility, "SynchronizeAccessibility (fixes CS0266)", IsEnabled(CodeFixIdentifiers.SynchronizeAccessibility)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.RemoveArgumentList, "Remove argument list (fixes CS1955)", IsEnabled(CodeFixIdentifiers.RemoveArgumentList)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.FixMemberAccessName, "Fix member access name (fixes CS1061)", IsEnabled(CodeFixIdentifiers.FixMemberAccessName)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.AddArgumentList, "Add argument list (fixes CS0428, CS0023)", IsEnabled(CodeFixIdentifiers.AddArgumentList)));
            codeFixes.Add(new BaseModel(CodeFixIdentifiers.InitializeLocalVariableWithDefaultValue, "Initialize local variable with default value (fixes CS0165)", IsEnabled(CodeFixIdentifiers.InitializeLocalVariableWithDefaultValue)));
        }
    }
}