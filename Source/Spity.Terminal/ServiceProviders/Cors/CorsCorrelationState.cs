using System;

namespace Spity.Terminal.ServiceProviders.Cors
{
    /// <summary>
    ///     CorsCorrelationState.
    /// </summary>
    public sealed class CorsCorrelationState
    {
        /// <summary>
        ///     Authorization.
        /// </summary>
        public string Authorization { get; set; }

        /// <summary>
        ///     Empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return string.IsNullOrWhiteSpace(Origin); }
        }

        /// <summary>
        ///     Origin.
        /// </summary>
        public string Origin { get; set; }
    }
}
