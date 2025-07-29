namespace Ametrin.Optional.Generator;

[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
internal sealed class GenerateAsyncExtensionsAttribute : Attribute;

[AttributeUsage(AttributeTargets.Method)]
internal sealed class AsyncExtensionAttribute : Attribute;