using System.Diagnostics.CodeAnalysis;
using src.features.shared.Interfaces;

namespace src.Utilities;

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Types
{
    internal static readonly Type Program = typeof(Program);
    internal static readonly Type IBaseRepository = typeof(IBaseRepository<>);
    internal static readonly Type IEndpoint = typeof(IEndpoint);
}