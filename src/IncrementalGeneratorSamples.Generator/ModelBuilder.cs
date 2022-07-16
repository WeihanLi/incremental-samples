﻿using IncrementalGeneratorSamples.InternalModels;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Threading;

namespace IncrementalGeneratorSamples
{
    public class ModelBuilder
    {
        public static InitialClassModel GetInitialModel(
                                      ISymbol symbol,
                                      CancellationToken cancellationToken)
        {
            if (!(symbol is ITypeSymbol typeSymbol))
            { return null; }

            var properties = new List<InitialPropertyModel>();
            foreach (var property in typeSymbol.GetMembers().OfType<IPropertySymbol>())
            {
                // since we do not know how big this list is, so we will check cancellation token
                cancellationToken.ThrowIfCancellationRequested();
                properties.Add(new InitialPropertyModel(property.Name,
                                                     property.GetDocumentationCommentXml(),
                                                     property.Type.ToString(),
                                                     property.AttributenamesAndValues()));
            }
            return new InitialClassModel(typeSymbol.Name,
                                         typeSymbol.ContainingNamespace.Name,
                                         typeSymbol.GetDocumentationCommentXml(),
                                         typeSymbol.AttributenamesAndValues(),
                                         properties);
        }

        public static CommandModel GetModel(InitialClassModel classModel,
                                             CancellationToken cancellationToken)
        {
            if (classModel is null) { return null; }

            var options = new List<OptionModel>();
            foreach (var property in classModel.Properties)
            {
                // since we do not know how big this list is, so we will check cancellation token
                cancellationToken.ThrowIfCancellationRequested();
                options.Add(new OptionModel(
                    $"{property.Name.AsKebabCase()}",
                    property.Name,
                    property.Name.AsPublicSymbol(),
                    property.Name.AsPrivateSymbol(),
                    Helpers.GetXmlDescription(property.XmlComments),
                    property.Type.ToString()));
            }
            return new CommandModel(
                    $"--{classModel.Name.AsKebabCase()}",
                    classModel.Name,
                    classModel.Name.AsPublicSymbol(),
                    classModel.Name.AsPrivateSymbol(),
                    Helpers.GetXmlDescription(classModel.XmlComments),
                    options);
        }

    }
}