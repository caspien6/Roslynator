﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.CodeFixes;
using Xunit;

#pragma warning disable RCS1090

namespace Roslynator.CSharp.Analysis.Tests
{
    public class RCS1077OptimizeLinqMethodCallTests : AbstractCSharpCodeFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.OptimizeLinqMethodCall;

        public override DiagnosticAnalyzer Analyzer { get; } = new InvocationExpressionAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new OptimizeLinqMethodCallCodeFixProvider();

        [Theory]
        [InlineData("Where(_ => true).Any()", "Any(_ => true)")]
        [InlineData("Where(_ => true).Count()", "Count(_ => true)")]
        [InlineData("Where(_ => true).First()", "First(_ => true)")]
        [InlineData("Where(_ => true).FirstOrDefault()", "FirstOrDefault(_ => true)")]
        [InlineData("Where(_ => true).Last()", "Last(_ => true)")]
        [InlineData("Where(_ => true).LastOrDefault()", "LastOrDefault(_ => true)")]
        [InlineData("Where(_ => true).LongCount()", "LongCount(_ => true)")]
        [InlineData("Where(_ => true).Single()", "Single(_ => true)")]
        public async Task Test_Where(string fromData, string toData)
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Linq;

class C
{
    void M()
    {
        var items = Enumerable.Empty<string>();

        var x = items.[||];
    }
}
", fromData, toData);
        }

        [Fact]
        public async Task Test_Where_Multiline()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;
using System.Linq;

class C
{
    void M()
    {
        var items = new List<string>();

        var x = items
            .[|Where(_ => true)
            .Any()|];
    }
}
", @"
using System.Collections.Generic;
using System.Linq;

class C
{
    void M()
    {
        var items = new List<string>();

        var x = items
            .Any(_ => true);
    }
}
");
        }

        [Theory]
        [InlineData("Where(_ => true).Any()", "Any(_ => true)")]
        [InlineData("Where(_ => true).Count()", "Count(_ => true)")]
        [InlineData("Where(_ => true).First()", "First(_ => true)")]
        [InlineData("Where(_ => true).FirstOrDefault()", "FirstOrDefault(_ => true)")]
        [InlineData("Where(_ => true).Last()", "Last(_ => true)")]
        [InlineData("Where(_ => true).LastOrDefault()", "LastOrDefault(_ => true)")]
        [InlineData("Where(_ => true).LongCount()", "LongCount(_ => true)")]
        [InlineData("Where(_ => true).Single()", "Single(_ => true)")]
        public async Task Test_Where_ImmutableArray(string fromData, string toData)
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Immutable;
using System.Linq;

class C
{
    void M()
    {
        ImmutableArray<string> items = ImmutableArray.Create<string>();

        var x = items.[||];
    }
}
", fromData, toData);
        }

        [Theory]
        [InlineData("Where(f => f is object).Cast<object>()", "OfType<object>()")]
        [InlineData("Where((f) => f is object).Cast<object>()", "OfType<object>()")]
        [InlineData(@"Where(f =>
        {
            return f is object;
        }).Cast<object>()", "OfType<object>()")]
        public async Task Test_CallOfTypeInsteadOfWhereAndCast(string fromData, string toData)
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;
using System.Linq;

class C
{
    void M()
    {
        var items = new List<string>();

        IEnumerable<object> q = items.[||];
    }
}
", fromData, toData);
        }

        [Theory]
        [InlineData(@"Where(f => f.StartsWith(""a"")).Any(f => f.StartsWith(""b""))", @"Any(f => f.StartsWith(""a"") && f.StartsWith(""b""))")]
        [InlineData(@"Where((f) => f.StartsWith(""a"")).Any(f => f.StartsWith(""b""))", @"Any((f) => f.StartsWith(""a"") && f.StartsWith(""b""))")]
        public async Task Test_CombineWhereAndAny(string fromData, string toData)
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;
using System.Linq;

class C
{
    void M()
    {
        var items = new List<string>();

        if (items.[||]) { }
    }
}
", fromData, toData);
        }

        [Theory]
        [InlineData(@"Where(f => f.StartsWith(""a"")).Any(f => f.StartsWith(""b""))", @"Any(f => f.StartsWith(""a"") && f.StartsWith(""b""))")]
        [InlineData(@"Where((f) => f.StartsWith(""a"")).Any(f => f.StartsWith(""b""))", @"Any((f) => f.StartsWith(""a"") && f.StartsWith(""b""))")]
        public async Task Test_CombineWhereAndAny_ImmutableArray(string fromData, string toData)
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Immutable;
using System.Linq;

class C
{
    void M()
    {
        ImmutableArray<string> items = ImmutableArray<string>.Empty;

        if (items.[||]) { }
    }
}
", fromData, toData);
        }

        [Theory]
        [InlineData("items.FirstOrDefault(_ => true) != null", "items.Any(_ => true)")]
        [InlineData("items.FirstOrDefault(_ => true) == null", "!items.Any(_ => true)")]
        [InlineData("items.FirstOrDefault(_ => true) is null", "!items.Any(_ => true)")]
        [InlineData("items.FirstOrDefault() != null", "items.Any()")]
        [InlineData("items.FirstOrDefault() == null", "!items.Any()")]
        [InlineData("items.FirstOrDefault() is null", "!items.Any()")]
        public async Task Test_FirstOrDefault_IEnumerableOfReferenceType(string fromData, string toData)
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Linq;

class C
{
    void M()
    {
        var items = Enumerable.Empty<string>();

        if ([||]) { }
    }
}
", fromData, toData);
        }

        [Theory]
        [InlineData("items.FirstOrDefault(_ => true) != null", "items.Any(_ => true)")]
        [InlineData("items.FirstOrDefault(_ => true) == null", "!items.Any(_ => true)")]
        [InlineData("items.FirstOrDefault(_ => true) is null", "!items.Any(_ => true)")]
        [InlineData("items.FirstOrDefault() != null", "items.Any()")]
        [InlineData("items.FirstOrDefault() == null", "!items.Any()")]
        [InlineData("items.FirstOrDefault() is null", "!items.Any()")]
        public async Task Test_FirstOrDefault_IEnumerableOfNullableType(string fromData, string toData)
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Linq;

class C
{
    void M()
    {
        var items = Enumerable.Empty<int?>();

        if ([||]) { }
    }
}
", fromData, toData);
        }

        [Theory]
        [InlineData("items.FirstOrDefault(_ => true) != null", "items.Any(_ => true)")]
        [InlineData("items.FirstOrDefault(_ => true) == null", "!items.Any(_ => true)")]
        [InlineData("items.FirstOrDefault(_ => true) is null", "!items.Any(_ => true)")]
        [InlineData("items.FirstOrDefault() != null", "items.Any()")]
        [InlineData("items.FirstOrDefault() == null", "!items.Any()")]
        [InlineData("items.FirstOrDefault() is null", "!items.Any()")]
        public async Task Test_FirstOrDefault_ImmutableArrayOfReferenceType(string fromData, string toData)
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Immutable;
using System.Linq;

class C
{
    void M()
    {
        ImmutableArray<string> items = ImmutableArray<string>.Empty;

        if ([||]) { }
    }
}
", fromData, toData);
        }

        [Theory]
        [InlineData("items.FirstOrDefault(_ => true) != null", "items.Any(_ => true)")]
        [InlineData("items.FirstOrDefault(_ => true) == null", "!items.Any(_ => true)")]
        [InlineData("items.FirstOrDefault(_ => true) is null", "!items.Any(_ => true)")]
        [InlineData("items.FirstOrDefault() != null", "items.Any()")]
        [InlineData("items.FirstOrDefault() == null", "!items.Any()")]
        [InlineData("items.FirstOrDefault() is null", "!items.Any()")]
        public async Task Test_FirstOrDefault_ImmutableArrayOfNullableType(string fromData, string toData)
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Immutable;
using System.Linq;

class C
{
    void M()
    {
        ImmutableArray<int?> items = ImmutableArray<int?>.Empty;

        if ([||]) { }
    }
}
", fromData, toData);
        }

        [Fact]
        public async Task Test_OptimizeOfType_ReferenceType()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;
using System.Linq;

class C
{
    void M()
    {
        var items = new List<C>();

        var q = items.[|OfType<C>()|];
    }
}
", @"
using System.Collections.Generic;
using System.Linq;

class C
{
    void M()
    {
        var items = new List<C>();

        var q = items.Where(f => f != null);
    }
}
");
        }

        [Fact]
        public async Task Test_OptimizeOfType_ValueType()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;
using System.Linq;

struct C
{
    void M()
    {
        var items = new List<C>();

        var q = items.[|OfType<C>()|];
    }
}
", @"
using System.Collections.Generic;
using System.Linq;

struct C
{
    void M()
    {
        var items = new List<C>();

        var q = items;
    }
}
");
        }

        [Fact]
        public async Task Test_OptimizeOfType_TypeParameterWithStructConstraint()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;
using System.Linq;

class C
{
    void M<T>() where T : struct
    {
        var items = new List<T>();

        var q = items.[|OfType<T>()|];
    }
}
", @"
using System.Collections.Generic;
using System.Linq;

class C
{
    void M<T>() where T : struct
    {
        var items = new List<T>();

        var q = items;
    }
}
");
        }

        [Fact]
        public async Task Test_CallCastInsteadOfSelect_ExtensionMethodCall()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Linq;

class C
{
    void M()
    {
        var items = Enumerable.Empty<string>().[|Select(f => (object)f)|];
    }
}
", @"
using System.Linq;

class C
{
    void M()
    {
        var items = Enumerable.Empty<string>().Cast<object>();
    }
}
");
        }

        [Fact]
        public async Task Test_CallCastInsteadOfSelect_StaticMethodCall()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Linq;

class C
{
    void M()
    {
        var items = Enumerable.[|Select(Enumerable.Empty<string>(), f => (object)f)|];
    }
}
", @"
using System.Linq;

class C
{
    void M()
    {
        var items = Enumerable.Cast<object>(Enumerable.Empty<string>());
    }
}
");
        }

        [Fact]
        public async Task Test_CallCastInsteadOfSelect_ParenthesizedLambda()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Linq;

class C
{
    void M()
    {
        var items = Enumerable.Empty<string>().[|Select((f) => (object)f)|];
    }
}
", @"
using System.Linq;

class C
{
    void M()
    {
        var items = Enumerable.Empty<string>().Cast<object>();
    }
}
");
        }

        [Fact]
        public async Task Test_CallCastInsteadOfSelect_LambdaWithBlock()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Linq;

class C
{
    void M()
    {
        var items = Enumerable.Empty<string>().[|Select(f =>
        {
            return (object)f;
        })|];
    }
}
", @"
using System.Linq;

class C
{
    void M()
    {
        var items = Enumerable.Empty<string>().Cast<object>();
    }
}
");
        }

        [Fact]
        public async Task Test_ReplaceFirstWithPeek_Queue()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;
using System.Linq;

class C
{
    void M()
    {
        var items = new Queue<object>();

        var x = items.[|First|]();
    }
}
", @"
using System.Collections.Generic;
using System.Linq;

class C
{
    void M()
    {
        var items = new Queue<object>();

        var x = items.Peek();
    }
}
");
        }

        [Fact]
        public async Task Test_ReplaceFirstWithPeek_Stack()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;
using System.Linq;

class C
{
    void M()
    {
        var items = new Stack<object>();

        var x = items.[|First|]();
    }
}
", @"
using System.Collections.Generic;
using System.Linq;

class C
{
    void M()
    {
        var items = new Stack<object>();

        var x = items.Peek();
    }
}
");
        }

        [Fact]
        public async Task Test_CallFindInsteadOfFirstOrDefault_List()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;
using System.Linq;

class C
{
    void M()
    {
        var items = new List<object>();

        var x = items.[|FirstOrDefault|](_ => true);
    }
}
", @"
using System.Collections.Generic;
using System.Linq;

class C
{
    void M()
    {
        var items = new List<object>();

        var x = items.Find(_ => true);
    }
}
");
        }

        [Fact]
        public async Task Test_CallFindInsteadOfFirstOrDefault_Array()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System;
using System.Linq;

class C
{
    void M()
    {
        var items = new object[0];

        var x = items.[|FirstOrDefault|](_ => true);
    }
}
", @"
using System;
using System.Linq;

class C
{
    void M()
    {
        var items = new object[0];

        var x = Array.Find(items, _ => true);
    }
}
");
        }

        [Theory]
        [InlineData("((List<object>)x).[|Count()|]", "((List<object>)x).Count")]
        [InlineData("((IList<object>)x).[|Count()|]", "((IList<object>)x).Count")]
        [InlineData("((IReadOnlyList<object>)x).[|Count()|]", "((IReadOnlyList<object>)x).Count")]
        [InlineData("((Collection<object>)x).[|Count()|]", "((Collection<object>)x).Count")]
        [InlineData("((ICollection<object>)x).[|Count()|]", "((ICollection<object>)x).Count")]
        [InlineData("((IReadOnlyCollection<object>)x).[|Count()|]", "((IReadOnlyCollection<object>)x).Count")]
        [InlineData("((ImmutableArray<object>)x).[|Count()|]", "((ImmutableArray<object>)x).Length")]
        [InlineData("((object[])x).[|Count()|]", "((object[])x).Length")]
        [InlineData("((string)x).[|Count()|]", "((string)x).Length")]
        public async Task Test_OptimizeCountCall_Array(string fromData, string toData)
        {
            await VerifyDiagnosticAndFixAsync(@"
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

class C
{
    void M()
    {
        object x = null;
        var count = [||];
    }
}
", fromData, toData);
        }

        [Theory]
        [InlineData("items.[|Count()|] != 0", "items.Any()")]
        [InlineData("items.[|Count()|] > 0", "items.Any()")]
        [InlineData("items.[|Count()|] >= 1", "items.Any()")]
        [InlineData("0 != items.[|Count()|]", "items.Any()")]
        [InlineData("0 < items.[|Count()|]", "items.Any()")]
        [InlineData("1 <= items.[|Count()|]", "items.Any()")]
        [InlineData("items.[|Count()|] == 0", "!items.Any()")]
        [InlineData("items.[|Count()|] < 1", "!items.Any()")]
        [InlineData("items.[|Count()|] <= 0", "!items.Any()")]
        [InlineData("0 == items.[|Count()|]", "!items.Any()")]
        [InlineData("1 > items.[|Count()|]", "!items.Any()")]
        [InlineData("0 >= items.[|Count()|]", "!items.Any()")]
        public async Task Test_CallAnyInsteadOfCount(string fromData, string toData)
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;
using System.Linq;

class C
{
    void M()
    {
        int i = 0;
        IEnumerable<object> items = null;

        if ([||])
        {
        }
    }
}
", fromData, toData);
        }

        [Fact]
        public async Task TestNoDiagnostic_CallOfTypeInsteadOfWhereAndCast()
        {
            await VerifyNoDiagnosticAsync(@"
using System.Collections.Generic;
using System.Linq;

class C
{
    void M()
    {
        var items = new List<string>();

        IEnumerable<object> q = items.Where(f => f is string).Cast<object>();
    }
}
");
        }

        [Fact]
        public async Task TestNoDiagnostic_CombineWhereAndAny()
        {
            await VerifyNoDiagnosticAsync(@"
using System.Collections.Generic;
using System.Linq;

class C
{
    void M()
    {
        var items = new List<string>();

        if (items.Where(f => f.StartsWith(""a"")).Any(g => g.StartsWith(""b""))) { }
    }
}
");
        }

        [Fact]
        public async Task TestNoDiagnostic_FirstOrDefault_ValueType()
        {
            await VerifyNoDiagnosticAsync(@"
using System.Collections.Immutable;
using System.Linq;

#pragma warning disable CS0472

class C
{
    void M()
    {
        var items = Enumerable.Empty<int>();

        if (items.FirstOrDefault(_ => true) != null) { }
        if (items.FirstOrDefault(_ => true) == null) { }
    }

    void M2()
    {
        ImmutableArray<int> items = ImmutableArray<int>.Empty;

        if (items.FirstOrDefault(_ => true) != null) { }
        if (items.FirstOrDefault(_ => true) == null) { }
    }
}
");
        }

        [Fact]
        public async Task TestNoDiagnostic_CallCastInsteadOfSelect_Conversion()
        {
            await VerifyNoDiagnosticAsync(@"
using System.Linq;

class C
{
    void M()
    {
            var items = Enumerable.Empty<C2>().Select(f => (C)f);

            var x1 = Enumerable.Empty<int>().Select(i => (byte)i);
            var x2 = Enumerable.Empty<int>().Select(i => (long)i);
            var x3 = Enumerable.Select(Enumerable.Empty<int>(), i => (byte)i);
            var x4 = Enumerable.Select(Enumerable.Empty<int>(), i => (long)i);
    }
}

class C2
{
    public static explicit operator C(C2 value)
    {
        return new C();
    }
}
");
        }

        [Fact]
        public async Task TestNoDiagnostic_OptimizeCountCall_IEnumerableOfT()
        {
            await VerifyNoDiagnosticAsync(@"
using System.Linq;

class C
{
    void M()
    {
        var count = Enumerable.Empty<object>().Count();
    }
}
");
        }

        [Fact]
        public async Task TestNoDiagnostic_CallAnyInsteadOfCount()
        {
            await VerifyNoDiagnosticAsync(@"
using System.Collections.Generic;
using System.Linq;

class C
{
    void M()
    {
        int i = 0;
        IEnumerable<object> items = null;

        if (items.Count() == 1) { }
        if (items.Count() == i) { }
        if (items.Count() != 1) { }
        if (items.Count() != i) { }
        if (items.Count() > i) { }
        if (items.Count() >= i) { }
        if (items.Count() <= i) { }
        if (items.Count() < i) { }
        if (1 == items.Count()) { }
        if (i == items.Count()) { }
        if (1 != items.Count()) { }
        if (i != items.Count()) { }
        if (i < items.Count()) { }
        if (i <= items.Count()) { }
        if (i >= items.Count()) { }
        if (i > items.Count()) { }
    }
}
");
        }
    }
}
