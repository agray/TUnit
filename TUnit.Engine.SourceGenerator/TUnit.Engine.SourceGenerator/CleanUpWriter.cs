﻿using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using TUnit.Engine.SourceGenerator.Extensions;

namespace TUnit.Engine.SourceGenerator;

public class CleanUpWriter
{
    public static string GenerateCode(INamedTypeSymbol classType)
    {
        var cleanUp = classType
            .GetMembersIncludingBase()
            .OfType<IMethodSymbol>()
            .Where(x => !x.IsStatic)
            .Where(x => x.DeclaredAccessibility == Accessibility.Public)
            .Where(x => x.GetAttributes()
                .Any(x => x.AttributeClass?.ToDisplayString(DisplayFormats.FullyQualifiedNonGenericWithGlobalPrefix)
                          == "global::TUnit.Core.AfterEachTestAttribute")
            )
            .Reverse()
            .ToList();
        
        if(!cleanUp.Any())
        {
            return string.Empty;
        }

        var stringBuilder = new StringBuilder();
        
        foreach (var oneTimeSetUpMethod in cleanUp)
        {
            stringBuilder.AppendLine($$"""
                                                          await RunSafelyAsync(classInstance.{{oneTimeSetUpMethod.Name}}, teardownExceptions);
                                       """);
        }

        return stringBuilder.ToString();
    }
}