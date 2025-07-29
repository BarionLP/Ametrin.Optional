namespace Ametrin.Optional.Generator;

[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
internal sealed class GenerateAsyncExtensionsAttribute : Attribute;

/// <summary>
/// tell the generator to generate an extension method for this on Task{this}
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
internal sealed class AsyncExtensionAttribute : Attribute;