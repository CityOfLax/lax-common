using System.Collections.Generic;
using Lax.Security.Claims;
using Microsoft.Extensions.Options;

namespace Lax.Security.Authentication.CustomWindows {

    public class CustomWindowsClaimsProviderOptions<TUser> : IOptions<CustomWindowsClaimsProviderOptions<TUser>> {

        public IList<ICustomClaimsTransformer<TUser>> ClaimsTransformer { get; set; } =
            new List<ICustomClaimsTransformer<TUser>>();

        public CustomWindowsClaimsProviderOptions<TUser> Value => this;

    }

}