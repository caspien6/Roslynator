﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Roslynator.CSharp
{
    //TODO: ?
    internal static class RefactoringUtility
    {
        public static InvocationExpressionSyntax ChangeInvokedMethodName(InvocationExpressionSyntax invocation, string newName)
        {
            ExpressionSyntax expression = invocation.Expression;

            if (expression != null)
            {
                SyntaxKind kind = expression.Kind();

                if (kind == SyntaxKind.SimpleMemberAccessExpression)
                {
                    var memberAccess = (MemberAccessExpressionSyntax)expression;
                    SimpleNameSyntax simpleName = memberAccess.Name;

                    if (simpleName != null)
                    {
                        SimpleNameSyntax newSimpleName = ChangeName(simpleName);

                        return invocation.WithExpression(memberAccess.WithName(newSimpleName));
                    }
                }
                else if (kind == SyntaxKind.MemberBindingExpression)
                {
                    var memberBinding = (MemberBindingExpressionSyntax)expression;
                    SimpleNameSyntax simpleName = memberBinding.Name;

                    if (simpleName != null)
                    {
                        SimpleNameSyntax newSimpleName = ChangeName(simpleName);

                        return invocation.WithExpression(memberBinding.WithName(newSimpleName));
                    }
                }
                else
                {
                    if (expression is SimpleNameSyntax simpleName)
                    {
                        SimpleNameSyntax newSimpleName = ChangeName(simpleName);

                        return invocation.WithExpression(newSimpleName);
                    }

                    Debug.Fail(kind.ToString());
                }
            }

            return invocation;

            SimpleNameSyntax ChangeName(SimpleNameSyntax simpleName)
            {
                return simpleName.WithIdentifier(
                    Identifier(
                        simpleName.GetLeadingTrivia(),
                        newName,
                        simpleName.GetTrailingTrivia()));
            }
        }

        public static bool ContainsOutArgumentWithLocal(
            ExpressionSyntax expression,
            SemanticModel semanticModel,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (SyntaxNode node in expression.DescendantNodes())
            {
                if (node.Kind() == SyntaxKind.Argument)
                {
                    var argument = (ArgumentSyntax)node;

                    if (argument.RefOrOutKeyword.Kind() == SyntaxKind.OutKeyword)
                    {
                        ExpressionSyntax argumentExpression = argument.Expression;

                        if (argumentExpression?.IsMissing == false
                            && semanticModel.GetSymbol(argumentExpression, cancellationToken)?.Kind == SymbolKind.Local)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static (SyntaxKind contentKind, string methodName, ImmutableArray<ArgumentSyntax> arguments)
            ConvertInterpolatedStringToStringBuilderMethod(InterpolatedStringContentSyntax content, bool isVerbatim)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            SyntaxKind kind = content.Kind();

            switch (kind)
            {
                case SyntaxKind.Interpolation:
                    {
                        var interpolation = (InterpolationSyntax)content;

                        InterpolationAlignmentClauseSyntax alignmentClause = interpolation.AlignmentClause;
                        InterpolationFormatClauseSyntax formatClause = interpolation.FormatClause;

                        if (alignmentClause != null
                            || formatClause != null)
                        {
                            StringBuilder sb = StringBuilderCache.GetInstance();

                            sb.Append("\"{0");

                            if (alignmentClause != null)
                            {
                                sb.Append(',');
                                sb.Append(alignmentClause.Value.ToString());
                            }

                            if (formatClause != null)
                            {
                                sb.Append(':');
                                sb.Append(formatClause.FormatStringToken.Text);
                            }

                            sb.Append("}\"");

                            ExpressionSyntax expression = ParseExpression(StringBuilderCache.GetStringAndFree(sb));

                            return (kind, "AppendFormat", ImmutableArray.Create(Argument(expression), Argument(interpolation.Expression)));
                        }
                        else
                        {
                            return (kind, "Append", ImmutableArray.Create(Argument(interpolation.Expression)));
                        }
                    }
                case SyntaxKind.InterpolatedStringText:
                    {
                        var interpolatedStringText = (InterpolatedStringTextSyntax)content;

                        string text = interpolatedStringText.TextToken.Text;

                        text = (isVerbatim)
                            ? "@\"" + text + "\""
                            : "\"" + text + "\"";

                        ExpressionSyntax stringLiteral = ParseExpression(text);

                        return (kind, "Append", ImmutableArray.Create(Argument(stringLiteral)));
                    }
                default:
                    {
                        throw new ArgumentException("", nameof(content));
                    }
            }
        }
    }
}
